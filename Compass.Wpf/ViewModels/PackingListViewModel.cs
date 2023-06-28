using System.Collections.Generic;
using System.Threading.Tasks;
using Compass.Wasm.Shared.Categories;
using Compass.Wasm.Shared.Params;
using Compass.Wpf.ApiServices.Plans;
using Compass.Wpf.ApiServices.Projects;
using Compass.Wpf.BatchWorks;

namespace Compass.Wpf.ViewModels;

public class PackingListViewModel : NavigationViewModel
{


    #region ctor
    private readonly IContainerProvider _provider;
    private readonly IPackingListService _packingListService;
    private readonly IPackingItemService _packingItemService;
    private readonly IProjectService  _projectService;
    public PackingListViewModel(IContainerProvider provider) : base(provider)
    {
        _provider = provider;
        _packingListService = provider.Resolve<IPackingListService>();
        _packingItemService = provider.Resolve<IPackingItemService>();
        _projectService = provider.Resolve<IProjectService>();
        PackingList = new PackingListDto();
        ExecuteCommand = new DelegateCommand<string>(Execute);
        UpdateItem = new DelegateCommand<PackingItemDto>(UpdatePackingItem);
        DeleteItem = new DelegateCommand<PackingItemDto>(DeletePackingItem);
    }
    public DelegateCommand<string> ExecuteCommand { get; }
    public DelegateCommand<PackingItemDto> UpdateItem { get; }
    public DelegateCommand<PackingItemDto> DeleteItem { get; }
    #endregion

    #region 属性
    private PackingListDto packingList;
    public PackingListDto PackingList
    {
        get => packingList;
        set { packingList = value; RaisePropertyChanged(); }
    }

    #endregion

    #region 执行操作
    private void Execute(string obj)
    {
        switch (obj)
        {
            case "UpdatePackingList": UpdatePackingList(); break;
            case "ReCreate": ReCreate(); break;
            case "AddStdMaterial": AddStdMaterial(); break;
            case "AddSplMaterial": AddSplMaterial(); break;
            case "Print": Print(); break;
        }
    }
    /// <summary>
    /// 更新装箱清单属性
    /// </summary>
    private async Task UpdatePackingList()
    {
        //弹窗添加PackingList
        var dialogParams = new DialogParameters { { "Value", PackingList } };
        var dialogResult = await DialogHost.ShowDialog("AddPackingListView", dialogParams);
        if (dialogResult.Result != ButtonResult.OK) return;
        PackingList = dialogResult.Parameters.GetValue<PackingListDto>("Value");
        await _packingListService.UpdateAsync(PackingList.Id.Value, PackingList);
    }
    /// <summary>
    /// 重新生成清单
    /// </summary>
    private async Task ReCreate()
    {
        switch (PackingList.Product)
        {
            case Product_e.Hood:
                {
                    //查询该批次下的Modules
                    var param = new ProjectParam { ProjectId = PackingList.ProjectId };
                    var mResult = await _projectService.GetModuleListAsync(param);
                    var modulesDtos = new List<ModuleDto>();
                    if (mResult.Status)
                    {
                        modulesDtos.AddRange(mResult.Result.Where(x=>x.Batch==packingList.Batch.Value));
                    }
                    //循环添加到PackingItem中，
                    foreach (var module in modulesDtos)
                    {
                        var newItem = new PackingItemDto()
                        {
                            PackingListId = packingList.Id,
                            MtlNumber = $"{module.ItemNumber}({module.Name})",
                            Description = $"{module.ItemNumber}({module.Name})",
                            Type = module.ModelName.Split('_')[0],
                            Quantity = 1,
                            Unit = Unit_e.PCS,
                            Length = module.Length.ToString(),
                            Width = module.Width.ToString(),
                            Height = module.Height.ToString(),
                            Pallet=true,//单独托盘
                        };
                        //更新内容,PackingListId和物料编码相同时
                        var oldItem = PackingList.PackingItemDtos.SingleOrDefault(x =>x.PackingListId==PackingList.Id && x.MtlNumber.Equals(newItem.MtlNumber));
                        if (oldItem != null)
                        {
                            //更新
                            await _packingItemService.UpdateAsync(oldItem.Id.Value, newItem);
                        }
                        else
                        {
                            //添加
                            await _packingItemService.AddAsync(newItem);
                        }
                    }
                    break;
                }
            case Product_e.Ceiling:
            {
                break;
            }
            case Product_e.NA:
            default:
                {
                    break;
                }
        }

        await RefreshDataAsync();
    }
    /// <summary>
    /// 添加标准物料
    /// </summary>
    private async Task AddStdMaterial()
    {
        //弹窗添加PackingList
        var dialogParams = new DialogParameters { { "Value", PackingList } };
        var dialogResult = await DialogHost.ShowDialog("AddStdMaterialView", dialogParams);
        if (dialogResult.Result != ButtonResult.OK) return;
        var packItems = dialogResult.Parameters.GetValue<List<PackingItemDto>>("Value");
        //判断PackingList.PackingItemDtos中
        foreach (var item in packItems)
        {
            //存在则修改
            if (PackingList.PackingItemDtos.Any(x =>
                    x.MtlNumber.Equals(item.MtlNumber, StringComparison.OrdinalIgnoreCase)))
            {
                var dto = PackingList.PackingItemDtos.SingleOrDefault(x =>
                    x.MtlNumber.Equals(item.MtlNumber, StringComparison.OrdinalIgnoreCase));
                item.Id = dto.Id;
               await _packingItemService.UpdateAsync(dto.Id.Value, item);
            }
            else
            {
                //不存在就添加
               await _packingItemService.AddAsync(item);
            }
        }

        await RefreshDataAsync();
    }

    /// <summary>
    /// 添加特殊自定义物料
    /// </summary>
    private async Task AddSplMaterial()
    {
        //弹出修改界面
        var obj = new PackingItemDto { PackingListId = PackingList.Id, Type="配件",Quantity=1, Unit=Unit_e.PCS, Pallet =false, NoLabel=false, OneLabel =true};
        var dialogParams = new DialogParameters { { "Value", obj } };
        var dialogResult = await DialogHost.ShowDialog("AddSplMaterialView", dialogParams);
        if (dialogResult.Result != ButtonResult.OK) return;
        var newItem = dialogResult.Parameters.GetValue<PackingItemDto>("Value");
        await _packingItemService.AddAsync(newItem);
        await RefreshDataAsync();
    }
    
    private async void UpdatePackingItem(PackingItemDto obj)
    {
        //弹出修改界面
        var dialogParams = new DialogParameters { { "Value", obj } };
        var dialogResult = await DialogHost.ShowDialog("AddSplMaterialView", dialogParams);
        if (dialogResult.Result != ButtonResult.OK) return;
        var newItem = dialogResult.Parameters.GetValue<PackingItemDto>("Value");
        await _packingItemService.UpdateAsync(obj.Id.Value, newItem);
        await RefreshDataAsync();
    }
    private async void DeletePackingItem(PackingItemDto obj)
    {
        //删除询问
        var dialogResult = await DialogHost.Question("删除确认", $"确认删除物料：{obj.MtlNumber} {obj.Description} 吗?");
        if (dialogResult.Result != ButtonResult.OK) return;
        await _packingItemService.DeleteAsync(obj.Id.Value);
        await RefreshDataAsync();
    }

    /// <summary>
    /// 打印装箱清单
    /// </summary>
    private async Task Print()
    {
        //打印询问
        var dialogResult = await DialogHost.Question("打印确认", "确认要打印装箱清单吗?");
        if (dialogResult.Result != ButtonResult.OK) return;
        await Task.Delay(500);//防止卡屏
        var printsService = _provider.Resolve<IPrintsService>();
        await printsService.PrintPackingListAsync(PackingList);
    }

    #endregion

    #region 初始化

    private async Task RefreshDataAsync()
    {
        //重新查找列表
        var packingListParam = new PackingListParam
        {
            ProjectId = PackingList.ProjectId,
            Batch = PackingList.Batch
        };
        await  GetDataAsync(packingListParam);
    }


    private async Task GetDataAsync(PackingListParam param)
    {
        //根据项目ID和分批获取装箱清单
        var response = await _packingListService.GetSingleByProjectIdAndBathAsync(param);
        if (!response.Status)
        {
            //如果获取失败，则添加装箱清单
            var dto = new PackingListDto
            {
                ProjectId = param.ProjectId,
                Batch = param.Batch,
                Product = Product_e.Hood
            };
            //弹窗添加PackingList
            var dialogParams = new DialogParameters { { "Value", dto } };
            var dialogResult = await DialogHost.ShowDialog("AddPackingListView", dialogParams);
            if (dialogResult.Result != ButtonResult.OK) return;
            //从弹窗获取修改后的数据
            PackingList = dialogResult.Parameters.GetValue<PackingListDto>("Value");
            //执行添加到数据库
            await _packingListService.AddByProjectIdAndBathAsync(PackingList);
            await Task.Delay(1000);
            //延迟1s后重新获取装箱清单信息
           await GetDataAsync(param);
        }
        PackingList = response.Result;
    }
    public override async void OnNavigatedTo(NavigationContext navigationContext)
    {
        base.OnNavigatedTo(navigationContext);
        var param = navigationContext.Parameters.ContainsKey("Value") ?
            navigationContext.Parameters.GetValue<PackingListParam>("Value")
            : null;
        if (param != null) { if (Journal is { CanGoBack: true }) Journal.GoBack(); }
       await GetDataAsync(param);
    }
    #endregion
}