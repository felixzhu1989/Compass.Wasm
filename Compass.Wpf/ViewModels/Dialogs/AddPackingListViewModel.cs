using MaterialDesignThemes.Wpf;
using Prism.Mvvm;

namespace Compass.Wpf.ViewModels.Dialogs;

public class AddPackingListViewModel : BindableBase, IDialogHostAware
{
    #region ctor
    public AddPackingListViewModel()
    {
        SaveCommand=new DelegateCommand(Save);
        CancelCommand=new DelegateCommand(Cancel);
    }
    public string DialogHostName { get; set; }
    public DelegateCommand SaveCommand { get; set; }
    public DelegateCommand CancelCommand { get; set; }
    #endregion

    #region 属性
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
        set { warning = value; RaisePropertyChanged();}
    }

    #endregion

    private void Cancel()
    {
        if (DialogHost.IsDialogOpen(DialogHostName))
            //取消时只返回No，告知操作结束
            DialogHost.Close(DialogHostName, new DialogResult(ButtonResult.No));
    }
    private void Save()
    {
        //验证数据已填写
        if (PackingList.Product == Product_e.Ceiling && string.IsNullOrWhiteSpace(PackingList.AssyPath))
        {
            Warning = "请选择天花烟罩总装配地址!";
            return;
        }
        Warning = string.Empty;
        if (!DialogHost.IsDialogOpen(DialogHostName)) return;
        DialogParameters param = new() { { "Value", PackingList } };
        //保存时传递参数param
        DialogHost.Close(DialogHostName, new DialogResult(ButtonResult.OK, param));
    }
    private void GetEnumNames()
    {
        //初始化一些枚举值
        Products=Enum.GetNames(typeof(Product_e));
        PackingTypes=new []{ "BasePallet-WoodenSupport(基本托盘-木条加固)", "FullCrate(全箱包装)", "Other(其它)" };
        DeliveryTypes=new []{ "ByClient-Ex-Work(顾客亲自提货)", "ByHalton-DeliveryCharged(本司提供货运)", "FOBShanghaiPort(FOB上海港)" };
    }

    public void OnDialogOpen(IDialogParameters parameters)
    {
        GetEnumNames();
        if (!parameters.ContainsKey("Value")) Cancel();
        PackingList = parameters.GetValue<PackingListDto>("Value");
        if (PackingList.Id != null && packingList.Id != Guid.Empty) return;
        PackingList.Product= Product_e.Hood;
        PackingList.PackingType =PackingTypes[0];
        PackingList.DeliveryType=DeliveryTypes[0];
    }
    
}