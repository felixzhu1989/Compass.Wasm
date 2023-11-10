﻿using System.Collections.Generic;
using Compass.Wasm.Shared.Categories;
using Compass.Wasm.Shared.Params;

namespace Compass.Wpf.ViewModels;

public class PackingListViewModel : NavigationViewModel
{
    #region ctor
    private readonly IPackingListService _packingListService;
    private readonly IPackingItemService _packingItemService;
    private readonly IProjectService  _projectService;
    private readonly IPrintsService _printsService;
    public PackingListViewModel(IContainerProvider provider) : base(provider)
    {
        _packingListService = provider.Resolve<IPackingListService>();
        _packingItemService = provider.Resolve<IPackingItemService>();
        _projectService = provider.Resolve<IProjectService>();
        _printsService=provider.Resolve<IPrintsService>();
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
    private PackingListParam pklParam;
    public PackingListParam PklParam
    {
        get => pklParam;
        set { pklParam = value; RaisePropertyChanged();}
    }

    #endregion

    #region 执行操作
    private async void Execute(string obj)
    {
        switch (obj)
        {
            case "UpdatePackingList":await UpdatePackingList(); break;
            case "ReCreate":await ReCreate(); break;
            case "AddStdMaterial":await AddStdMaterial(); break;
            case "AddSplMaterial":await AddSplMaterial(); break;
            case "Print":await Print(); break;
            case "PackingInfo": PackingInfo(); break;
        }
    }
    /// <summary>
    /// 更新装箱清单属性
    /// </summary>
    private async Task UpdatePackingList()
    {
        var navParam = new NavigationParameters { { "Value", PklParam },{ "Dto", PackingList } };
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
                        modulesDtos.AddRange(mResult.Result.Where(x=>x.Batch==packingList.Batch.Value && x.Pallet));//非独立托盘的item不需要
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
                            Order = 0
                        };
                        //更新内容,PackingListId和物料编码及其托盘相同时
                        var oldItem = PackingList.PackingItemDtos.SingleOrDefault(x =>x.PackingListId==PackingList.Id && x.MtlNumber.Equals(newItem.MtlNumber));
                        
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
                    //更新托盘号
                    await _packingItemService.UpdatePalletNumberAsync(packingList.Id.Value);
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
        await GetDataAsync();
        Aggregator.SendMessage("重新生成清单完成！");
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
        var obj = new PackingItemDto { PackingListId = PackingList.Id, Type="配件",Quantity=1, Unit=Unit_e.PCS, Pallet =false, NoLabel=false, OneLabel =false,Order = 1};
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
        //打印询问
        var dialogResult = await DialogHost.Question("打印确认", "确认要打印装箱清单吗?");
        if (dialogResult.Result != ButtonResult.OK) return;
        await Task.Delay(500);//防止卡屏
        await _printsService.PrintPackingListAsync(PackingList);
        Aggregator.SendMessage("打印装箱清单完成！");
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