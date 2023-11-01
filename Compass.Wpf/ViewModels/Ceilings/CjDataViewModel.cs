namespace Compass.Wpf.ViewModels.Ceilings;

public class CjDataViewModel : NavigationViewModel
{
    #region ctor-Cj参数页面
    private readonly ICjDataService _service;
    public CjDataViewModel(IContainerProvider provider) : base(provider)
    {
        _service = provider.Resolve<ICjDataService>();
        SaveDataCommand = new DelegateCommand(async () =>
        {
            var result = await _service.UpdateAsync(DataDto.Id.Value, DataDto);
            Aggregator.SendMessage(result.Status ? $"{Title} 参数保存成功！" : $"{Title}参数保存失败，{result.Message}");
        });
        OpenHttpLinkCommand = new DelegateCommand(OpenHttpLink);
    }
    public DelegateCommand SaveDataCommand { get; }
    public DelegateCommand OpenHttpLinkCommand { get; }
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
    private CjData dataDto = null!;
    public CjData DataDto
    {
        get => dataDto;
        set { dataDto = value; RaisePropertyChanged(); }
    }
    #endregion
    #region 详细参数相关枚举值属性
    private string[] sidePanels = null!;
    public string[] SidePanels
    {
        get => sidePanels;
        set { sidePanels = value; RaisePropertyChanged(); }
    }
    private string[] cjSpigotDirections = null!;
    public string[] CjSpigotDirections
    {
        get => cjSpigotDirections;
        set { cjSpigotDirections = value; RaisePropertyChanged(); }
    }
    private string[] beamTypes = null!;
    public string[] BeamTypes
    {
        get => beamTypes;
        set { beamTypes = value; RaisePropertyChanged(); }
    }
    private string[] bcjSides = null!;
    public string[] BcjSides
    {
        get => bcjSides;
        set { bcjSides = value; RaisePropertyChanged(); }
    }
    private string[] lksSides = null!;
    public string[] LksSides
    {
        get => lksSides;
        set { lksSides = value; RaisePropertyChanged(); }
    }
    private string[] gutterSides = null!;
    public string[] GutterSides
    {
        get => gutterSides;
        set { gutterSides = value; RaisePropertyChanged(); }
    }
    #endregion

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
    #region 导航初始化
    private void GetEnumNames()
    {
        //初始化一些枚举值
        SidePanels = Enum.GetNames(typeof(SidePanel_e));
        CjSpigotDirections=Enum.GetNames(typeof(CjSpigotDirection_e));
        BeamTypes = Enum.GetNames(typeof(BeamType_e));
        BcjSides = Enum.GetNames(typeof(BcjSide_e));
        LksSides = Enum.GetNames(typeof(LksSide_e));
        GutterSides = Enum.GetNames(typeof(GutterSide_e));
    }
    private async void GetDataAsync()
    {
        DataDto = new CjData();
        var dataResult = await _service.GetFirstOrDefault(CurrentModule.Id.Value);
        if (dataResult.Status)
        {
            DataDto = dataResult.Result;
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
        currentModule = new ModuleDto();
        CurrentModule= navigationContext.Parameters.ContainsKey("Value")
            ? navigationContext.Parameters.GetValue<ModuleDto>("Value")
            : new ModuleDto();
        var specialNotes = string.IsNullOrEmpty(CurrentModule.SpecialNotes) ? "" : $" ({CurrentModule.SpecialNotes})";
        Title = $"{CurrentModule.Name} {CurrentModule.ModelName}{specialNotes}";
        GetEnumNames();
        GetDataAsync();
        UpdateRoles = "admin,pm,manager,designer";
    }

    #endregion

}