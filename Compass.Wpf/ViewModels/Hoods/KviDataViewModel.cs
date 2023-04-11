using Compass.Wasm.Shared.DataService;
using Compass.Wasm.Shared.DataService.Hoods;
using Compass.Wasm.Shared.ProjectService;
using Compass.Wpf.Extensions;
using Prism.Commands;
using Prism.Ioc;
using Prism.Regions;
using System;
using Compass.Wpf.ApiService.Hoods;

namespace Compass.Wpf.ViewModels.Hoods;

public class KviDataViewModel : NavigationViewModel
{
    #region ctor-KVI参数页面
    private readonly IKviDataService _service;
    public KviDataViewModel(IContainerProvider provider) : base(provider)
    {
        _service = provider.Resolve<IKviDataService>();
        SaveDataCommand = new DelegateCommand(async () =>
        {
            var result = await _service.UpdateAsync(DataDto.Id.Value, DataDto);
            Aggregator.SendMessage(result.Status ? $"{Title} 参数保存成功！" : $"{Title}参数保存失败，{result.Message}");
        });
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
    private KviData dataDto = null!;
    public KviData DataDto
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


    #endregion

    #region Command
    public DelegateCommand SaveDataCommand { get; }

    #endregion

    #region 导航初始化
    private void GetEnumNames()
    {
        //初始化一些枚举值
        SidePanels=Enum.GetNames(typeof(SidePanel_e));
        LightTypes=Enum.GetNames(typeof(LightType_e));
        DrainTypes = Enum.GetNames(typeof(DrainType_e));
        AnsulSides= Enum.GetNames(typeof(AnsulSide_e));
        AnsulDetectors = Enum.GetNames(typeof(AnsulDetector_e));
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
            DataDto = new KviData();
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