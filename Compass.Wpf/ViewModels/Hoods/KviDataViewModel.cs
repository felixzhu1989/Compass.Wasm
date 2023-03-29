using Compass.Wasm.Shared.ProjectService;
using Compass.Wpf.Extensions;
using Compass.Wpf.Service.Hoods;
using Prism.Events;
using Prism.Ioc;
using Prism.Regions;
using System;
using Compass.Wasm.Shared.DataService;
using Prism.Commands;
using Compass.Wasm.Shared.DataService.Hoods;

namespace Compass.Wpf.ViewModels.Hoods;

public class KviDataViewModel : NavigationViewModel
{
    private readonly IEventAggregator _aggregator;
    private readonly IKviDataService _service;

    #region Module和ModuleData
    private ModuleDto currentModule;
    public ModuleDto CurrentModule
    {
        get => currentModule;
        set { currentModule = value; RaisePropertyChanged(); }
    }
    //抬头
    private string title;
    public string Title
    {
        get => title;
        set { title = value; RaisePropertyChanged(); }
    }
    private KviData dataDto;
    public KviData DataDto
    {
        get => dataDto;
        set { dataDto = value; RaisePropertyChanged(); }
    }
    #endregion

    #region 详细参数相关枚举值

    private string[] sidePanels;
    public string[] SidePanels
    {
        get => sidePanels;
        set { sidePanels = value;RaisePropertyChanged(); }
    }
    public string[] ExhaustSpigotNumbers { get; set; } = { "1", "2" };

    private string[] lightTypes;
    public string[] LightTypes
    {
        get => lightTypes;
        set { lightTypes = value; RaisePropertyChanged(); }
    }
    private string[] drainTypes;
    public string[] DrainTypes
    {
        get => drainTypes;
        set { drainTypes = value; RaisePropertyChanged(); }
    }
    private string[] ansulSides;
    public string[] AnsulSides
    {
        get => ansulSides;
        set { ansulSides = value; RaisePropertyChanged(); }
    }
    private string[] ansulDetectors;
    public string[] AnsulDetectors
    {
        get => ansulDetectors;
        set { ansulDetectors = value; RaisePropertyChanged(); }
    }


    #endregion

    #region Command
    public DelegateCommand SaveDataCommand { get; }
    #endregion

    public KviDataViewModel(IEventAggregator aggregator, IContainerProvider containerProvider,IKviDataService service) : base(containerProvider)
    {
        _aggregator = aggregator;
        _service = service;
        SaveDataCommand = new DelegateCommand( async () =>
        {
           var result=  await _service.UpdateAsync(DataDto.Id.Value, DataDto);
           if(result.Status) _aggregator.SendMessage($"{Title} 参数保存成功！");
           else _aggregator.SendMessage($"{Title}参数保存失败，{result.Message}");
        });
    }

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
            _aggregator.SendMessage(dataResult.Message??"查询失败了");
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