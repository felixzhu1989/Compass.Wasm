﻿using Compass.Wasm.Shared.Data.Hoods;

namespace Compass.Wpf.ViewModels.Hoods;

public class KvfDataViewModel : NavigationViewModel
{
    #region ctor-Kvf参数页面
    private readonly IKvfDataService _service;
    public KvfDataViewModel(IContainerProvider provider) : base(provider)
    {
        _service = provider.Resolve<IKvfDataService>();
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
    private KvfData dataDto = null!;
    public KvfData DataDto
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
    public string[] SupplySpigotNumbers { get; set; } = { "1", "2","3" };

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
        DrainTypes = Enum.GetNames(typeof(DrainType_e));
        AnsulSides= Enum.GetNames(typeof(AnsulSide_e));
        AnsulDetectors = Enum.GetNames(typeof(AnsulDetector_e));
    }
    private async void GetDataAsync()
    {
        DataDto = new KvfData();
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
        CurrentModule = new ModuleDto();
        CurrentModule = navigationContext.Parameters.ContainsKey("Value")
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