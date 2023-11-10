namespace Compass.Wpf.ViewModels.Ceilings;

public class UcwDataViewModel : NavigationViewModel
{
    #region ctor-Ucw参数页面

    private readonly IUcwDataService _service;
    public UcwDataViewModel(IContainerProvider provider) : base(provider)
    {
        _service = provider.Resolve<IUcwDataService>();
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
    private UcwData dataDto = null!;
    public UcwData DataDto
    {
        get => dataDto;
        set { dataDto = value; RaisePropertyChanged(); }
    }
    #endregion

    #region 详细参数相关枚举值属性
    public string[] ExhaustSpigotNumbers { get; set; } = { "1", "2" };

    private string[] filterSides = null!;
    public string[] FilterSides
    {
        get => filterSides;
        set { filterSides = value; RaisePropertyChanged(); }
    }

    private string[] ceilingLightTypes = null!;
    public string[] CeilingLightTypes
    {
        get => ceilingLightTypes;
        set { ceilingLightTypes = value; RaisePropertyChanged(); }
    }

    private string[] hclSides = null!;
    public string[] HclSides
    {
        get => hclSides;
        set { hclSides = value; RaisePropertyChanged(); }
    }
    private string[] ansulSides = null!;
    public string[] AnsulSides
    {
        get => ansulSides;
        set { ansulSides = value; RaisePropertyChanged(); }
    }
    private string[] ansulDetectors = null!;
    public string[] AnsulDetectors
    {
        get => ansulDetectors;
        set { ansulDetectors = value; RaisePropertyChanged(); }
    }

    private string[] sidePanels = null!;
    public string[] SidePanels
    {
        get => sidePanels;
        set { sidePanels = value; RaisePropertyChanged(); }
    }
    private string[] dpSides = null!;
    public string[] DpSides
    {
        get => dpSides;
        set { dpSides = value; RaisePropertyChanged(); }
    }

    private string[] ceilingWaterInlets = null!;
    public string[] CeilingWaterInlets
    {
        get => ceilingWaterInlets;
        set { ceilingWaterInlets = value; RaisePropertyChanged(); }
    }

    private string[] uvLightTypes = null!;
    public string[] UvLightTypes
    {
        get => uvLightTypes;
        set { uvLightTypes = value; RaisePropertyChanged(); }
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
        FilterSides = Enum.GetNames(typeof(FilterSide_e));
        CeilingLightTypes = Enum.GetNames(typeof(CeilingLightType_e));

        HclSides = Enum.GetNames(typeof(HclSide_e));
        AnsulSides= Enum.GetNames(typeof(AnsulSide_e));
        AnsulDetectors = Enum.GetNames(typeof(AnsulDetector_e));
        SidePanels = Enum.GetNames(typeof(SidePanel_e));
        DpSides=Enum.GetNames(typeof(DpSide_e));
        CeilingWaterInlets=Enum.GetNames(typeof(CeilingWaterInlet_e));
        UvLightTypes = Enum.GetNames(typeof(UvLightType_e));
    }
    private async void GetDataAsync()
    {
        DataDto = new UcwData();
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
        UpdateRoles = "admin,pm,mgr,dsr";
    }

    #endregion
}