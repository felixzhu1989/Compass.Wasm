using Compass.Wasm.Shared.Params;

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
    private bool preview;
    public bool Preview
    {
        get => preview;
        set { preview = value; RaisePropertyChanged(); }
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

        }
    }

    /// <summary>
    /// 装箱清单
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

    private void AddPallet()
    {
        PackingList.PackingItemDtos.Add(new PackingItemDto
        {
            PackingListId = PackingList.Id,
            Type="托盘",
            Pallet = true
        });
        Aggregator.SendMessage("已添加行，请使用滚轮定位到最后一行填写信息！");
    }
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

    private async Task ExportPackingInfo()
    {
        await Save();
        //导出Excel文件
        await  _printsService.ExportPackingInfoAsync(packingList);
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
        await GetDataAsync(packingListParam);
    }
    private async Task GetDataAsync(PackingListParam param)
    {
        //根据项目ID和分批获取装箱信息表
        var response = await _packingListService.GetPackingInfoAsync(param);
        if (response.Status) PackingList = response.Result;
    }
    public override async void OnNavigatedTo(NavigationContext navigationContext)
    {
        base.OnNavigatedTo(navigationContext);
        var param = navigationContext.Parameters.ContainsKey("Value") ?
            navigationContext.Parameters.GetValue<PackingListParam>("Value")
            : null;
        await GetDataAsync(param);
        if (PackingList.Id==null||PackingList.Id.Equals(Guid.Empty))
        {
            navigationContext.NavigationService.Journal.GoBack();
        }
    }
    #endregion
}