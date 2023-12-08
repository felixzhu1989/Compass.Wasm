using System.Collections.Generic;
using System.DirectoryServices;
using System.IO;
using Compass.Wasm.Shared.Categories;
using Compass.Wasm.Shared.Params;

namespace Compass.Wpf.ViewModels;

public class PackingListViewModel : NavigationViewModel
{
    #region ctor
    private readonly IPackingListService _packingListService;
    private readonly IPackingItemService _packingItemService;
    private readonly IMaterialItemService _mtlItemService;
    private readonly IProjectService _projectService;
    private readonly IPrintsService _printsService;
    public PackingListViewModel(IContainerProvider provider) : base(provider)
    {
        _packingListService = provider.Resolve<IPackingListService>();
        _packingItemService = provider.Resolve<IPackingItemService>();
        _mtlItemService = provider.Resolve<IMaterialItemService>();
        _projectService = provider.Resolve<IProjectService>();
        _printsService=provider.Resolve<IPrintsService>();
        PackingList = new PackingListDto();
        ExecuteCommand = new DelegateCommand<string>(Execute);
        UpdateItem = new DelegateCommand<PackingItemDto>(UpdatePackingItem);
        DeleteItem = new DelegateCommand<PackingItemDto>(DeletePackingItem);
        IsEnable = true;
        UpdateRoles = "admin,pm,mgr,dsr";
        PrintLabelRoles = "admin,prod";
    }
    public DelegateCommand<string> ExecuteCommand { get; }
    public DelegateCommand<PackingItemDto> UpdateItem { get; }
    public DelegateCommand<PackingItemDto> DeleteItem { get; }
    #endregion
    #region 角色控制属性
    private string updateRoles;
    public string UpdateRoles
    {
        get => updateRoles;
        set { updateRoles = value; RaisePropertyChanged(); }
    }
    private string printLabelRoles;
    public string PrintLabelRoles
    {
        get => printLabelRoles;
        set { printLabelRoles = value; RaisePropertyChanged(); }
    }
    #endregion
    #region 属性
    private bool isEnable;
    public bool IsEnable
    {
        get => isEnable;
        set { isEnable = value; RaisePropertyChanged(); }
    }
    private PackingListDto packingList;
    public PackingListDto PackingList
    {
        get => packingList;
        set { packingList = value; RaisePropertyChanged(); }
    }
    private PackingListParam pklParam;
    public PackingListParam PklParam
    {
        get => pklParam;
        set { pklParam = value; RaisePropertyChanged(); }
    }
    public bool? IsAllItemsSelected
    {
        get
        {
            var selected = PackingList.PackingItemDtos.Select(item => item.IsSelected).Distinct().ToList();
            return selected.Count == 1 ? selected.Single() : null;
        }
        set
        {
            if (value.HasValue)
            {
                SelectAll(value.Value, PackingList.PackingItemDtos);
                RaisePropertyChanged();
            }
        }
    }
    private static void SelectAll(bool select, IEnumerable<PackingItemDto> items)
    {
        foreach (var item in items)
        {
            item.IsSelected = select;
        }
    }
    #endregion

    #region 执行操作
    private async void Execute(string obj)
    {
        switch (obj)
        {
            case "UpdatePackingList": await UpdatePackingList(); break;
            case "ReCreate": await ReCreate(); break;
            case "AddStdMaterial": await AddStdMaterial(); break;
            case "AddSplMaterial": await AddSplMaterial(); break;
            case "Print": await Print(); break;
            case "PrintLabel": await PrintLabel(); break;
            case "PackingInfo": PackingInfo(); break;
        }
    }
    /// <summary>
    /// 更新装箱清单属性
    /// </summary>
    private async Task UpdatePackingList()
    {
        var navParam = new NavigationParameters { { "Value", PklParam }, { "Dto", PackingList } };
        RegionManager.Regions[PrismManager.MainViewRegionName].RequestNavigate("AddPackingListView", back =>
        {
            Journal = back.Context.NavigationService.Journal;
        }, navParam);
    }
    /// <summary>
    /// 重新生成清单
    /// </summary>
    private async Task ReCreate()
    {
        IsEnable = false;
        await Task.Delay(500);
        switch (PackingList.Product)
        {
            case Product_e.Hood:
                {
                    await CreateHoodPackingList();
                    break;
                }
            case Product_e.Ceiling:
                {
                    await CreateCeilingPackingList();
                    break;
                }
            case Product_e.NA:
            default:
                {
                    //两者都执行
                    await CreateHoodPackingList();
                    await CreateCeilingPackingList();
                    break;
                }
        }
        await GetDataAsync();
        Aggregator.SendMessage("重新生成清单完成！");
        IsEnable = true;
    }
    /// <summary>
    /// 创建或更新普通烟罩装箱清单
    /// </summary>
    private async Task CreateHoodPackingList()
    {
        //查询该批次下的Modules
        var param = new ProjectParam { ProjectId = PackingList.ProjectId };
        var mResult = await _projectService.GetModuleListAsync(param);
        var modulesDtos = new List<ModuleDto>();
        if (mResult.Status)
        {
            //只需要独立托盘的item
            modulesDtos.AddRange(mResult.Result.Where(x => x.Batch==PackingList.Batch.Value && x.Pallet));
        }
        //循环添加到PackingItem中，
        foreach (var module in modulesDtos)
        {
            var newItem = new PackingItemDto()
            {
                PackingListId = PackingList.Id,
                MtlNumber = $"{module.ItemNumber}({module.Name})",
                Description = $"{module.ItemNumber}({module.Name})",
                EnDescription = $"{module.ItemNumber}({module.Name})",
                Type = module.ModelName.Split('_')[0],
                Quantity = 1,
                Unit = Unit_e.PCS,
                Length = module.Length.ToString(),
                Width = module.Width.ToString(),
                Height = module.Height.ToString(),
                Pallet=true,//单独托盘
                Order = 0
            };
            //查询发货清单，看看是否有重复，重复则直接覆盖
            await AddOrUpdatePackingItem(newItem);
        }
        //更新（重排）托盘号
        await _packingItemService.UpdatePalletNumberAsync(PackingList.Id.Value);
    }
    /// <summary>
    /// 创建或更新天花烟罩装箱清单
    /// </summary>
    private async Task CreateCeilingPackingList()
    {
        //如果地址不存在，就报错退出
        if (!File.Exists(PackingList.AssyPath))
        {
            throw new FileNotFoundException("总装配地址不存在，请认证检查");
        }
        //PackingList.AssyPath，调用扩展方法，返回一个列表
        var swApp = SwUtility.ConnectSw(Aggregator);
        var mtlItems = swApp.GetMaterialItems(PackingList.AssyPath);
        foreach (var mtlItem in mtlItems)
        {
            //查询模板，更改值
            var mtlItemParam = new MaterialItemParam { Type = mtlItem.Type };
            var tempResult =await _mtlItemService.GetFirstOrDefaultByTypeAsync(mtlItemParam);
            if (!tempResult.Status)
            {
                throw new Exception($"{mtlItem.Type}，没有找到这种型号的物料信息，请联系管理员查看情况");
            }
            var mtlItemTemp = tempResult.Result;
            //修改编号，长，宽，数量
            var newItem = new PackingItemDto()
            {
                PackingListId = PackingList.Id,
                MtlNumber = mtlItem.MtlNumber,//修改编号
                Description = mtlItemTemp.Description,
                EnDescription = mtlItemTemp.EnDescription,
                Type = mtlItemTemp.Type,
                Quantity = mtlItem.Quantity,//数量
                Unit = mtlItemTemp.Unit,
                Length = string.IsNullOrEmpty(mtlItem.Length)? mtlItemTemp.Length: mtlItem.Length,//长
                Width = string.IsNullOrEmpty(mtlItem.Width)? mtlItemTemp.Width : mtlItem.Width,//宽
                Height = mtlItemTemp.Height,
                Material = mtlItemTemp.Material,
                CalcRule = mtlItemTemp.CalcRule,
                Pallet=false,//单独托盘，天花烟罩不需要单独的托盘
                Order = mtlItemTemp.Order,
                NoLabel = mtlItemTemp.NoLabel,
                OneLabel = mtlItemTemp.OneLabel,
            };
            //查询发货清单，看看是否有重复，重复则直接覆盖
            await AddOrUpdatePackingItem(newItem);
        }
    }
    /// <summary>
    /// 添加或更新发货清单条目
    /// </summary>
    private async Task AddOrUpdatePackingItem(PackingItemDto newItem)
    {
        //更新内容,PackingListId和物料编码及相同时
        var oldItem = PackingList.PackingItemDtos.SingleOrDefault(x => x.PackingListId==PackingList.Id && x.MtlNumber.Equals(newItem.MtlNumber));

        if (oldItem != null)
        {
            //更新
            newItem.Remark = oldItem.Remark;//保留Remarks
            await _packingItemService.UpdateAsync(oldItem.Id.Value, newItem);
        }
        else
        {
            //添加
            await _packingItemService.AddAsync(newItem);
        }
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

        await GetDataAsync();
    }

    /// <summary>
    /// 添加特殊自定义物料
    /// </summary>
    private async Task AddSplMaterial()
    {
        //弹出修改界面
        var obj = new PackingItemDto { PackingListId = PackingList.Id, Type="配件", Quantity=1, Unit=Unit_e.PCS, Pallet =false, NoLabel=false, OneLabel =false, Order = 1 };
        var dialogParams = new DialogParameters { { "Value", obj } };
        var dialogResult = await DialogHost.ShowDialog("AddSplMaterialView", dialogParams);
        if (dialogResult.Result != ButtonResult.OK) return;
        var newItem = dialogResult.Parameters.GetValue<PackingItemDto>("Value");
        await _packingItemService.AddAsync(newItem);
        await GetDataAsync();
    }

    private async void UpdatePackingItem(PackingItemDto obj)
    {
        //弹出修改界面
        var dialogParams = new DialogParameters { { "Value", obj } };
        var dialogResult = await DialogHost.ShowDialog("AddSplMaterialView", dialogParams);
        if (dialogResult.Result != ButtonResult.OK) return;
        var newItem = dialogResult.Parameters.GetValue<PackingItemDto>("Value");
        await _packingItemService.UpdateAsync(obj.Id.Value, newItem);
        await GetDataAsync();
    }
    private async void DeletePackingItem(PackingItemDto obj)
    {
        //删除询问
        var dialogResult = await DialogHost.Question("删除确认", $"确认删除物料：{obj.MtlNumber} {obj.Description} 吗?");
        if (dialogResult.Result != ButtonResult.OK) return;
        await _packingItemService.DeleteAsync(obj.Id.Value);
        await GetDataAsync();
    }

    /// <summary>
    /// 打印装箱清单
    /// </summary>
    private async Task Print()
    {
        IsEnable = false;
        //打印询问
        var dialogResult = await DialogHost.Question("打印确认", "确认要打印装箱清单吗?");
        if (dialogResult.Result != ButtonResult.OK) return;
        await Task.Delay(500);//防止卡屏
        await _printsService.PrintPackingListAsync(PackingList);
        Aggregator.SendMessage("打印装箱清单完成！");
        IsEnable = true;
    }

    /// <summary>
    /// 打印标签
    /// </summary>
    private async Task PrintLabel()
    {
        IsEnable = false;
        //打印询问
        var dialogResult = await DialogHost.Question("打印确认", "确认要打印标签吗?");
        if (dialogResult.Result != ButtonResult.OK) return;
        await Task.Delay(500);//防止卡屏

        //获取勾选的Item
        var selectItemDtos = PackingList.PackingItemDtos.Where(x => x.IsSelected && x.NoLabel==false).ToList();
        if (selectItemDtos.Count == 0)
        {
            Aggregator.SendMessage("请勾选（或全选）要打印的清单行！");
            IsEnable = true;
            return;
        }

        //await _printsService.PrintPackingItemLabelAsync(selectItemDtos);

        await Task.Delay(3000);

        Aggregator.SendMessage("打印标签完成！");
        IsEnable = true;
    }

    /// <summary>
    /// 导航到装箱信息页面
    /// </summary>
    private void PackingInfo()
    {
        var packingListParam = new PackingListParam
        {
            ProjectId = PackingList.ProjectId,
            Batch = PackingList.Batch
        };
        var param = new NavigationParameters { { "Value", packingListParam } };
        RegionManager.Regions[PrismManager.MainViewRegionName].RequestNavigate("PackingInfoView", back =>
        {
            Journal = back.Context.NavigationService.Journal;
        }, param);
    }


    #endregion

    #region 初始化
    private async Task GetDataAsync()
    {
        //根据项目ID和分批获取装箱清单
        var response = await _packingListService.GetSingleByProjectIdAndBathAsync(PklParam);
        if (response.Status)
        {
            PackingList = response.Result;
        }
        else
        {
            //todo:跳转到新页面添加装箱清单PackingListDto，在返回此地
            var navParam = new NavigationParameters { { "Value", PklParam } };
            RegionManager.Regions[PrismManager.MainViewRegionName].RequestNavigate("AddPackingListView", back =>
            {
                Journal = back.Context.NavigationService.Journal;
            }, navParam);
        }

        //绑定勾选数据变更
        foreach (var item in PackingList.PackingItemDtos)
        {
            item.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(ModuleDto.IsSelected))
                    RaisePropertyChanged(nameof(IsAllItemsSelected));
            };
        }
        IsAllItemsSelected = false;

    }
    public override async void OnNavigatedTo(NavigationContext navigationContext)
    {
        base.OnNavigatedTo(navigationContext);
        if (!navigationContext.Parameters.ContainsKey("Value"))
        {
            if (Journal is { CanGoBack: true }) Journal.GoBack();
        }
        PklParam = navigationContext.Parameters.GetValue<PackingListParam>("Value");
        await GetDataAsync();
    }
    #endregion
}