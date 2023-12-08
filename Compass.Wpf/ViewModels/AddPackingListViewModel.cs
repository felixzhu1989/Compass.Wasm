using Azure;
using Compass.Wasm.Shared.Params;
using Microsoft.Win32;
using System.IO;

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
        GetDropPathCommand = new DelegateCommand<DragEventArgs>(GetDropPath);
        BrowseFileCommand = new DelegateCommand(BrowseFile);
    }
    public DelegateCommand SaveCommand { get; set; }
    public DelegateCommand CancelCommand { get; set; }
    public DelegateCommand<DragEventArgs> GetDropPathCommand { get; }
    public DelegateCommand BrowseFileCommand { get; }
    #endregion
    #region 角色控制属性
    private string updateRoles;
    public string UpdateRoles
    {
        get => updateRoles;
        set { updateRoles = value; RaisePropertyChanged(); }
    }
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
        //返回
        Journal.GoBack();
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

        bool status;
        if (PackingList.Id != null && PackingList.Id.Value != Guid.Empty) //编辑ToDo
        {
            var response = await _packingListService.UpdateAsync(PackingList.Id.Value, PackingList);
            status = response.Status;
        }
        else
        {
            //执行添加到数据库
            var response = await _packingListService.AddByProjectIdAndBathAsync(PackingList);
            if (!response.Status)
            {
                throw new Exception("添加装箱清单信息失败，请检查是否有重复计划行");
            }
            //跳转回PackingListView
            status = response.Status;
        }
        if (status)
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
        PackingTypes = new[]
        {
            "BasePallet-WoodenSupport(基本托盘-木条加固)", 
            "FullCrate(全箱包装)", 
            "Other(其它)"
        };
        DeliveryTypes = new[]
        {
            "ByClient-Ex-Work(顾客亲自提货)", 
            "ByHalton-DeliveryCharged(本司提供货运)", 
            "FOBShanghaiPort(FOB上海港)"
        };
    }
    #region 获取文件地址
    //拖拽
    private void GetDropPath(DragEventArgs e)
    {
        PackingList.AssyPath = ((Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString();
        e.Handled=true;
    }
    //浏览
    private void BrowseFile()
    {
        var dialog = new OpenFileDialog
        {
            Filter = "装配体|*.sldasm"
        };
        var odpPath = Path.Combine(@"D:\MyProjects", PackingList.ProjectName.Split('-').First());

        if (Directory.Exists(odpPath))
        {
            dialog.InitialDirectory = odpPath;
        }
        else
        {
            dialog.InitialDirectory = @"D:\MyProjects";
        }

        if (dialog.ShowDialog() == true)
        {
            PackingList.AssyPath=dialog.FileName;
        }
    }
    #endregion
    public override void OnNavigatedTo(NavigationContext navigationContext)
    {
        base.OnNavigatedTo(navigationContext);
        GetEnumNames();
        if (!navigationContext.Parameters.ContainsKey("Value")) Cancel();
        PklParam = navigationContext.Parameters.GetValue<PackingListParam>("Value");
        if (navigationContext.Parameters.ContainsKey("Dto"))
        {
            PackingList=navigationContext.Parameters.GetValue<PackingListDto>("Dto");
        }
        else
        {
            PackingList = new PackingListDto
            {
                ProjectId = PklParam.ProjectId,
                ProjectName = PklParam.ProjectName,
                Batch = PklParam.Batch,
                Product = Product_e.Hood,
                PackingType = PackingTypes[0],
                DeliveryType = DeliveryTypes[0]
            };
        }
        UpdateRoles = "admin,pm,mgr,dsr";
    }
}