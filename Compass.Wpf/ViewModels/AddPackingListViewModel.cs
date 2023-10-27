using Compass.Wasm.Shared.Params;
using Compass.Wpf.ApiServices.Plans;

namespace Compass.Wpf.ViewModels;

public class AddPackingListViewModel : NavigationViewModel
{
    #region ctor
    private readonly IPackingListService _packingListService;

    public AddPackingListViewModel(IContainerProvider provider) : base(provider)
    {
        _packingListService = provider.Resolve<IPackingListService>();
        SaveCommand = new DelegateCommand(Save);
        CancelCommand = new DelegateCommand(Cancel);
    }
    public DelegateCommand SaveCommand { get; set; }
    public DelegateCommand CancelCommand { get; set; }
    #endregion

    #region 属性

    public PackingListParam PklParam { get; set; }
    private PackingListDto packingList;
    public PackingListDto PackingList
    {
        get => packingList;
        set { packingList = value; RaisePropertyChanged(); }
    }
    private string[] products = null!;
    public string[] Products
    {
        get => products;
        set { products = value; RaisePropertyChanged(); }
    }
    private string[] packingTypes = null!;
    public string[] PackingTypes
    {
        get => packingTypes;
        set { packingTypes = value; RaisePropertyChanged(); }
    }
    private string[] deliveryTypes = null!;
    public string[] DeliveryTypes
    {
        get => deliveryTypes;
        set { deliveryTypes = value; RaisePropertyChanged(); }
    }
    private string warning;
    public string Warning
    {
        get => warning;
        set { warning = value; RaisePropertyChanged(); }
    }

    #endregion

    private void Cancel()
    {
        //返回首页
        RegionManager.Regions[PrismManager.MainViewRegionName].RequestNavigate("IndexView", back => { Journal = back.Context.NavigationService.Journal; });
    }
    private async void Save()
    {
        //验证数据已填写
        if (PackingList.Product == Product_e.Ceiling && string.IsNullOrWhiteSpace(PackingList.AssyPath))
        {
            Warning = "请选择天花烟罩总装配地址!";
            return;
        }
        Warning = string.Empty;

        //执行添加到数据库
        var response = await _packingListService.AddByProjectIdAndBathAsync(PackingList);
        //跳转回PackingListView
        if (response.Status)
        {
            var navParam = new NavigationParameters { { "Value", PklParam } };
            RegionManager.Regions[PrismManager.MainViewRegionName].RequestNavigate("PackingListView", back =>
            {
                Journal = back.Context.NavigationService.Journal;
            }, navParam);
        }
        else
        {
            Cancel();
        }
    }

    private void GetEnumNames()
    {
        //初始化一些枚举值
        Products = Enum.GetNames(typeof(Product_e));
        PackingTypes = new[] { "BasePallet-WoodenSupport(基本托盘-木条加固)", "FullCrate(全箱包装)", "Other(其它)" };
        DeliveryTypes = new[] { "ByClient-Ex-Work(顾客亲自提货)", "ByHalton-DeliveryCharged(本司提供货运)", "FOBShanghaiPort(FOB上海港)" };
    }

    public override void OnNavigatedTo(NavigationContext navigationContext)
    {

        base.OnNavigatedTo(navigationContext);
        GetEnumNames();
        if (!navigationContext.Parameters.ContainsKey("Value")) Cancel();
        PklParam = navigationContext.Parameters.GetValue<PackingListParam>("Value");
        PackingList = new PackingListDto
        {
            ProjectId = PklParam.ProjectId,
            Batch = PklParam.Batch,
            Product = Product_e.Hood,
            PackingType = PackingTypes[0],
            DeliveryType = DeliveryTypes[0]
        };
    }
}