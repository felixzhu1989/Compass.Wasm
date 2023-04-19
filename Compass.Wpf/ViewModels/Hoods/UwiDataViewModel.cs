using Compass.Wpf.Extensions;
using Prism.Commands;
using Prism.Ioc;
using Prism.Regions;
using System;
using System.Diagnostics;
using Compass.Wasm.Shared.Data;
using Compass.Wasm.Shared.Data.Hoods;
using Compass.Wasm.Shared.Projects;
using Compass.Wpf.ApiServices.Hoods;

namespace Compass.Wpf.ViewModels.Hoods;

public class UwiDataViewModel : NavigationViewModel
{
    #region ctor-Uwi参数页面
    private readonly IUwiDataService _service;
    public UwiDataViewModel(IContainerProvider provider) : base(provider)
    {
        _service = provider.Resolve<IUwiDataService>();
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
    private UwiData dataDto = null!;
    public UwiData DataDto
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

    private string[] lightTypes = null!;
    public string[] LightTypes
    {
        get => lightTypes;
        set { lightTypes = value; RaisePropertyChanged(); }
    }

    private string[] waterInlets = null!;
    public string[] WaterInlets
    {
        get => waterInlets;
        set { waterInlets = value; RaisePropertyChanged(); }
    }

    private string[] drainTypes = null!;
    public string[] DrainTypes
    {
        get => drainTypes;
        set { drainTypes = value; RaisePropertyChanged(); }
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
        SidePanels=Enum.GetNames(typeof(SidePanel_e));
        LightTypes=Enum.GetNames(typeof(LightType_e));
        WaterInlets = Enum.GetNames(typeof(WaterInlet_e));
        DrainTypes = Enum.GetNames(typeof(DrainType_e));
        AnsulSides= Enum.GetNames(typeof(AnsulSide_e));
        AnsulDetectors = Enum.GetNames(typeof(AnsulDetector_e));
        AnsulDetectorEnds = Enum.GetNames(typeof(AnsulDetectorEnd_e));
        UvLightTypes = Enum.GetNames(typeof(UvLightType_e));
    }
    private async void GetDataAsync()
    {
        var dataResult = await _service.GetFirstOrDefault(CurrentModule.Id.Value);
        if (dataResult.Status)
        {
            DataDto = dataResult.Result;
        }
        else
        {
            DataDto = new UwiData();
            //提示用户，查询失败了
            Aggregator.SendMessage(dataResult.Message??"查询失败了");
        }
    }
    public override void OnNavigatedTo(NavigationContext navigationContext)
    {
        base.OnNavigatedTo(navigationContext);
        //ModuleDto
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