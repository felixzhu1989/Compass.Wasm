using Compass.Wasm.Shared.Params;
using Prism.Regions;
using System.Collections.Generic;

namespace Compass.Wpf.ViewModels;

public class PackingInfoViewModel : NavigationViewModel
{
    #region ctor
    private readonly IPackingListService _packingListService;
    private readonly IPackingItemService _packingItemService;
    private readonly IPrintsService _printsService;
    public PackingInfoViewModel(IContainerProvider provider) : base(provider)
    {
        _packingListService = provider.Resolve<IPackingListService>();
        _packingItemService = provider.Resolve<IPackingItemService>();
        _printsService=provider.Resolve<IPrintsService>();
        PackingList = new PackingListDto();
        ExecuteCommand = new DelegateCommand<string>(Execute);
        DeleteItem = new DelegateCommand<PackingItemDto>(DeletePackingItem);
        IsEnable = true;
        UpdateRoles = "admin,prod,pmc";
        PrintLabelRoles = "admin,whse";
    }
    public DelegateCommand<string> ExecuteCommand { get; }
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
    //是否显示预览
    private bool preview;
    public bool Preview
    {
        get => preview;
        set { preview = value; RaisePropertyChanged(); }
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
    private void Execute(string obj)
    {
        switch (obj)
        {
            case "PackingList": PackingListNavigate(); break;
            case "AddPallet": AddPallet(); break;
            case "Save": Save(); break;
            case "ExportPackingInfo": ExportPackingInfo(); break;
            case "PrintLabel": PrintLabel();break;
        }
    }

    

    /// <summary>
    /// 导航到装箱清单
    /// </summary>
    private void PackingListNavigate()
    {
        var packingListParam = new PackingListParam
        {
            ProjectId = PackingList.ProjectId,
            Batch = PackingList.Batch
        };
        var param = new NavigationParameters { { "Value", packingListParam } };
        RegionManager.Regions[PrismManager.MainViewRegionName].RequestNavigate("PackingListView", back =>
        {
            Journal = back.Context.NavigationService.Journal;
        }, param);
    }

    /// <summary>
    /// 添加自定义托盘（配件或者是天花）
    /// </summary>
    private void AddPallet()
    {
        PackingList.PackingItemDtos.Add(new PackingItemDto
        {
            PackingListId = PackingList.Id,
            Type="托盘",
            Pallet = true,
            Order = 99,
            NoLabel = true,
            OneLabel = false,
        });
        Aggregator.SendMessage("已添加行，请使用滚轮定位到最后一行填写信息！");
    }

    /// <summary>
    /// 删除条目
    /// </summary>
    private async void DeletePackingItem(PackingItemDto obj)
    {
        //删除询问
        var dialogResult = await DialogHost.Question("删除确认", $"确认删除托盘：{obj.PalletNumber}/{obj.MtlNumber} 吗?");
        if (dialogResult.Result != ButtonResult.OK) return;

        if (obj.Id != null && obj.Id != Guid.Empty)
        {
            //从数据库删除
           await _packingItemService.DeleteAsync(obj.Id.Value);
        }
        PackingList.PackingItemDtos.Remove(obj);
        Aggregator.SendMessage("删除托盘信息完成！");
    }

    /// <summary>
    /// 保存信息
    /// </summary>
    private async Task Save()
    {
        foreach (var item in PackingList.PackingItemDtos)
        {
            if (item.Id != null && item.Id != Guid.Empty)
            {
                //更新Pallet
                await _packingItemService.UpdatePalletAsync(item.Id.Value, item);
            }
            else
            {
                //更新Pallet
                if (string.IsNullOrEmpty(item.PalletNumber) || string.IsNullOrWhiteSpace(item.MtlNumber))
                {
                    //await DialogHost.Question("提示", "请填写托盘号，产品编号（用来描述产品,请勿重复）");
                    //空的不添加
                    continue;
                }
                await _packingItemService.AddPalletAsync(item);
            }
        }
        Aggregator.SendMessage("装箱信息已保存！");
    }

    /// <summary>
    /// 导出Excel到桌面
    /// </summary>
    private async Task ExportPackingInfo()
    {
        IsEnable = false;
        await Save();
        //导出Excel文件
        await  _printsService.ExportPackingInfoAsync(packingList);
        IsEnable = true;
    }

    /// <summary>
    /// 打印托盘标签
    /// </summary>
    private async void PrintLabel()
    {
        IsEnable = false;
        //打印询问
        var dialogResult = await DialogHost.Question("打印确认", "确认要打印标签吗?");
        if (dialogResult.Result != ButtonResult.OK)
        {
            IsEnable = true;
            return;
        }
        await Task.Delay(500);//防止卡屏

        //获取勾选的Item，并筛选托盘
        var selectItemDtos = PackingList.PackingItemDtos.Where(x => x.IsSelected && x.Pallet).ToList();
        if (selectItemDtos.Count == 0)
        {
            Aggregator.SendMessage("请勾选（或全选）要打印的托盘行！");
            IsEnable = true;
            return;
        }
        await _printsService.PrintPalletLabelAsync(selectItemDtos);
        Aggregator.SendMessage("打印标签完成！");
        IsEnable = true;
    }

    #endregion
    
    #region 初始化
    private async Task GetDataAsync(PackingListParam param)
    {
        //根据项目ID和分批获取装箱信息表
        var response = await _packingListService.GetPackingInfoAsync(param);
        if (response.Status)
        {
            PackingList = response.Result;
        }
        else
        {
            Aggregator.SendMessage("技术部还没出装箱清单！");
            //返回到主计划页面
            RegionManager.Regions[PrismManager.MainViewRegionName].RequestNavigate("MainPlansView", back =>
            {
                Journal = back.Context.NavigationService.Journal;
            });
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
        var param = navigationContext.Parameters.ContainsKey("Value") ?
            navigationContext.Parameters.GetValue<PackingListParam>("Value")
            : null;
        await GetDataAsync(param);
    }
    #endregion
}