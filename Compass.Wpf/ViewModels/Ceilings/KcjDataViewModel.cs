﻿using Compass.Wasm.Shared.Data;

namespace Compass.Wpf.ViewModels.Ceilings;

public class KcjDataViewModel : NavigationViewModel
{
    #region ctor-Kcj参数页面
    private readonly IKcjDataService _service;
    public KcjDataViewModel(IContainerProvider provider) : base(provider)
    {
        _service = provider.Resolve<IKcjDataService>();
        SaveDataCommand = new DelegateCommand(async () =>
        {
            var result = await _service.UpdateAsync(DataDto.Id.Value, DataDto);
            Aggregator.SendMessage(result.Status ? $"{Title} 参数保存成功！" : $"{Title}参数保存失败，{result.Message}");
        });
        OpenHttpLinkCommand = new DelegateCommand(OpenHttpLink);
        UpdateRoles = "admin,pm,mgr,dsr";
    }
    public DelegateCommand SaveDataCommand { get; }
    public DelegateCommand OpenHttpLinkCommand { get; }
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
    private KcjData dataDto = null!;
    public KcjData DataDto
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
    public string[] ExhaustSpigotNumbers { get; set; } = { "1", "2" };
    private string[] filterTypes = null!;
    public string[] FilterTypes
    {
        get => filterTypes;
        set { filterTypes = value; RaisePropertyChanged(); }
    }
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
    private string[] lightCables = null!;
    public string[] LightCables
    {
        get => lightCables;
        set { lightCables = value; RaisePropertyChanged(); }
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
    private string[] ansulDetectorEnds = null!;
    public string[] AnsulDetectorEnds
    {
        get => ansulDetectorEnds;
        set { ansulDetectorEnds = value; RaisePropertyChanged(); }
    }
    #endregion
    
    #region 导航初始化
    private void GetEnumNames()
    {
        //初始化一些枚举值
        SidePanels = Enum.GetNames(typeof(SidePanel_e));
        FilterTypes =Enum.GetNames(typeof(FilterType_e));
        FilterSides = Enum.GetNames(typeof(FilterSide_e));
        CeilingLightTypes = Enum.GetNames(typeof(CeilingLightType_e));
        LightCables = Enum.GetNames(typeof(LightCable_e));
        HclSides = Enum.GetNames(typeof(HclSide_e));
        AnsulSides= Enum.GetNames(typeof(AnsulSide_e));
        AnsulDetectors = Enum.GetNames(typeof(AnsulDetector_e));
        AnsulDetectorEnds = Enum.GetNames(typeof(AnsulDetectorEnd_e));
    }
    private async void GetDataAsync()
    {
        DataDto = new KcjData();
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
    }

    #endregion
}