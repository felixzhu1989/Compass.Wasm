using Compass.Wasm.Shared.Categories;
using MaterialDesignThemes.Wpf;
using Prism.Mvvm;

namespace Compass.Wpf.ViewModels.Dialogs;

public class AddStdMaterialViewModel : BindableBase, IDialogHostAware
{
    #region ctor
    private readonly IMaterialItemService _mtlItemService;
    public readonly IEventAggregator _aggregator;
    public AddStdMaterialViewModel(IContainerProvider provider)
    {
        _mtlItemService = provider.Resolve<IMaterialItemService>();
        _aggregator = provider.Resolve<IEventAggregator>();
        SaveCommand =new DelegateCommand(Save);
        CancelCommand=new DelegateCommand(Cancel);
        ExecuteCommand = new DelegateCommand<string>(Execute);
        MaterialItemDtos = new ObservableCollection<MaterialItemDto>();
        FilterMtlItemDtos = new ObservableCollection<MaterialItemDto>();
    }
    public string DialogHostName { get; set; }
    public DelegateCommand SaveCommand { get; set; }
    public DelegateCommand CancelCommand { get; set; }
    public DelegateCommand<string> ExecuteCommand { get; }
    #endregion

    #region 属性
    private PackingListDto packingList;
    public PackingListDto PackingList
    {
        get => packingList;
        set { packingList = value; RaisePropertyChanged(); }
    }
    
    public ObservableCollection<MaterialItemDto> MaterialItemDtos { get; }
    public ObservableCollection<MaterialItemDto> FilterMtlItemDtos { get; }
    #endregion

    #region 筛选属性
    private string search;
    /// <summary>
    /// 搜索条件属性
    /// </summary>
    public string Search
    {
        get => search;
        set { search = value; RaisePropertyChanged(); }
    }
    //使用枚举初始化下拉框
    private string[] products = null!;
    public string[] Products
    {
        get => products;
        set { products = value; RaisePropertyChanged(); }
    }

    private int selectedProduct;
    /// <summary>
    /// 选中状态，用于搜索筛选
    /// </summary>
    public int SelectedProduct
    {
        get => selectedProduct;
        set { selectedProduct = value; RaisePropertyChanged(); }
    }

    #endregion

    private void Execute(string obj)
    {
        switch (obj)
        {

            case "Query": Query(); break;
            //case "Add": Add(); break;
            //case "Save": Save(); break;
        }
    }

    private void Query()
    {
        if(MaterialItemDtos.Count==0)return;
        var product = (Product_e)SelectedProduct;
        FilterMtlItemDtos.Clear();
        switch (product)
        {
            
            case Product_e.Hood:
                FilterMtlItemDtos.AddRange(MaterialItemDtos.Where(x => x.Hood));
                break;
            case Product_e.Ceiling:
                FilterMtlItemDtos.AddRange(MaterialItemDtos.Where(x => x.Ceiling));
                break;
            case Product_e.NA:
            default:
                FilterMtlItemDtos.AddRange(MaterialItemDtos); 
                break;
        }

    }


    private void Cancel()
    {
        if (DialogHost.IsDialogOpen(DialogHostName))
            //取消时只返回No，告知操作结束
            DialogHost.Close(DialogHostName, new DialogResult(ButtonResult.No));
    }

    private void Save()
    {
        //筛选数量不为0的MaterialItems，转化成PackingItems
        var mtlItems = FilterMtlItemDtos.Where(x => x.Quantity != 0).ToList();
        if (!mtlItems.Any())
        {
            _aggregator.SendMessage("请修改数量");
            return;
        }
        var packItems = mtlItems.Select(x => new PackingItemDto
            {
                PackingListId = PackingList.Id,
                MtlNumber = x.MtlNumber,
                Description = x.Description,
                EnDescription = x.EnDescription,
                Type = x.Type,
                Quantity = x.Quantity,
                Unit = x.Unit,
                Length = x.Length,
                Width = x.Width,
                Height = x.Height,
                Material = x.Material,
                Remark = x.Remark,
                CalcRule = x.CalcRule,
                NoLabel = x.NoLabel,
                OneLabel = x.OneLabel,
                Pallet = false,
                Order=x.Order
            })
            .ToList();

        //向清单页面回传PackingItems
        if (!DialogHost.IsDialogOpen(DialogHostName)) return;
        DialogParameters param = new() { { "Value", packItems } };
        //保存时传递参数param
        DialogHost.Close(DialogHostName, new DialogResult(ButtonResult.OK, param));
    }


    private async Task GetDataAsync()
    {
        var response = await _mtlItemService.GetAllAsync();
        if (!response.Status) return;
        MaterialItemDtos.Clear();
        MaterialItemDtos.AddRange(response.Result);
    }


    public async void OnDialogOpen(IDialogParameters parameters)
    {
        if (!parameters.ContainsKey("Value")) Cancel();
        PackingList = parameters.GetValue<PackingListDto>("Value");
        if (PackingList.Id == null || packingList.Id == Guid.Empty) Cancel();
        Products = Enum.GetNames(typeof(Product_e));
        SelectedProduct = (int)PackingList.Product;
        await GetDataAsync();
        Query();
    }
}