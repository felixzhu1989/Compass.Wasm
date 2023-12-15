using System.IO;
using Compass.Wasm.Shared.Categories;
using Compass.Wasm.Shared.Extensions;
using Microsoft.Win32;

namespace Compass.Wpf.ViewModels.Ceilings;

public class DxfDataViewModel : NavigationViewModel
{
    #region ctor-Dxf参数页面

    private readonly IDxfDataService _service;
    public DxfDataViewModel(IContainerProvider provider) : base(provider)
    {
        _service = provider.Resolve<IDxfDataService>();
        SaveDataCommand = new DelegateCommand(async () =>
        {
            DataDto.AccNumber = Accs.AccsToString();
            var result = await _service.UpdateAsync(DataDto.Id.Value, DataDto);
            Aggregator.SendMessage(result.Status ? $"{Title} 参数保存成功！" : $"{Title}参数保存失败，{result.Message}");
        });
        OpenHttpLinkCommand = new DelegateCommand(OpenHttpLink);
        GetDropPathCommand = new DelegateCommand<DragEventArgs>(GetDropPath);
        BrowseFileCommand = new DelegateCommand(BrowseFile);
        Accs = new ObservableCollection<Accessories>();
        RefreshTextCommand = new DelegateCommand(() =>
        { DataDto.AccNumber = Accs.AccsToString(); });
        AddItemCommand = new DelegateCommand(() =>
        { Accs.Add(new Accessories()); });
        DeleteItemCommand = new DelegateCommand<Accessories>((obj) => { Accs.Remove(obj); });
        UpdateRoles = "admin,pm,mgr,dsr";
    }



    public DelegateCommand SaveDataCommand { get; }
    public DelegateCommand OpenHttpLinkCommand { get; }
    public DelegateCommand<DragEventArgs> GetDropPathCommand { get; }
    public DelegateCommand BrowseFileCommand { get; }
    public DelegateCommand RefreshTextCommand { get; }
    public DelegateCommand AddItemCommand { get; }
    public DelegateCommand<Accessories> DeleteItemCommand { get; }
    #region 打开网页链接
    private void OpenHttpLink()
    {
        foreach (var drwUrl in CurrentModule.DrawingUrl.Split('\n'))
        {
            var startInfo = new ProcessStartInfo(drwUrl)
            { UseShellExecute =true };
            Process.Start(startInfo);
        }
    }
    #endregion
    #endregion

    #region 角色控制属性
    private string updateRoles;
    public string UpdateRoles
    {
        get => updateRoles;
        set { updateRoles = value; RaisePropertyChanged(); }
    }
    #endregion

    #region Module和ModuleData属性
    private ModuleDto currentModule = null!;
    public ModuleDto CurrentModule
    {
        get => currentModule;
        set { currentModule = value; RaisePropertyChanged(); }
    }
    //抬头
    private string title = null!;
    public string Title
    {
        get => title;
        set { title = value; RaisePropertyChanged(); }
    }
    private DxfData dataDto = null!;
    public DxfData DataDto
    {
        get => dataDto;
        set { dataDto = value; RaisePropertyChanged(); }
    }
    public ObservableCollection<Accessories> Accs { get; }

    #endregion

    #region 详细参数相关枚举值属性
    private string[] sidePanels = null!;
    public string[] SidePanels
    {
        get => sidePanels;
        set { sidePanels = value; RaisePropertyChanged(); }
    }
    private string[] accTypes = null!;
    public string[] AccTypes
    {
        get => accTypes;
        set { accTypes = value; RaisePropertyChanged(); }
    }
    #endregion

    #region 获取文件地址
    //拖拽
    private void GetDropPath(DragEventArgs e)
    {
        DataDto.AssyPath = ((Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString();
        e.Handled=true;
    }
    //浏览
    private void BrowseFile()
    {
        var dialog = new OpenFileDialog
        {
            Filter = "装配体|*.sldasm"
        };
        var odpPath = Path.Combine(@"D:\MyProjects", CurrentModule.OdpNumber);
        var itemPath = Path.Combine(odpPath, CurrentModule.ItemNumber);
        if (Directory.Exists(itemPath))
        {
            dialog.InitialDirectory = itemPath;
        }
        else if (Directory.Exists(odpPath))
        {
            dialog.InitialDirectory = odpPath;
        }
        else
        {
            dialog.InitialDirectory = @"D:\MyProjects";
        }

        if (dialog.ShowDialog() == true)
        {
            DataDto.AssyPath=dialog.FileName;
        }
    }
    #endregion

    #region 导航初始化
    private void GetEnumNames()
    {
        //初始化一些枚举值
        SidePanels=Enum.GetNames(typeof(SidePanel_e));
        AccTypes=Enum.GetNames(typeof(AccType_e));
    }
    private async void GetDataAsync()
    {
        DataDto = new DxfData();
        var dataResult = await _service.GetFirstOrDefault(CurrentModule.Id.Value);
        if (dataResult.Status)
        {
            DataDto = dataResult.Result;
            Accs.AddRange(DataDto.AccNumber.StringToAccs());
        }
        else
        {
            //提示用户，查询失败了
            Aggregator.SendMessage(dataResult.Message??"查询失败了");
        }
    }
    public override void OnNavigatedTo(NavigationContext navigationContext)
    {
        base.OnNavigatedTo(navigationContext);
        //ModuleDto
        CurrentModule = new ModuleDto();
        CurrentModule = navigationContext.Parameters.ContainsKey("Value")
            ? navigationContext.Parameters.GetValue<ModuleDto>("Value")
            : new ModuleDto();
        var specialNotes = string.IsNullOrEmpty(CurrentModule.SpecialNotes) ? "" : $" ({CurrentModule.SpecialNotes})";
        Title = $"{CurrentModule.Name} {CurrentModule.ModelName}{specialNotes}";
        GetEnumNames();
        GetDataAsync();
    }
    #endregion
}