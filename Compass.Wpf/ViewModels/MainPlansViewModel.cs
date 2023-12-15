using System.Collections.Generic;
using System.IO;
using Compass.Wasm.Shared.Extensions;
using Application = Microsoft.Office.Interop.Excel.Application;
using Compass.Wasm.Shared.Params;


namespace Compass.Wpf.ViewModels;

public class MainPlansViewModel : NavigationViewModel
{

    #region ctor
    private readonly IMainPlanService _mainPlanService;
    private readonly IProjectService _projectService;
    private readonly IPackingListService _packingListService;
    public MainPlansViewModel(IContainerProvider provider) : base(provider)
    {
        _mainPlanService = provider.Resolve<IMainPlanService>();
        _projectService = provider.Resolve<IProjectService>();
        _packingListService= provider.Resolve<IPackingListService>();

        MainPlanDtos = new ObservableCollection<MainPlanDto>();
        FilterMainPlanDtos = new ObservableCollection<MainPlanDto>();
        OpenPlanCommand = new DelegateCommand(OpenPlan);
        OpenCsvCommand = new DelegateCommand(OpenCsv);
        UpdateCommand = new DelegateCommand(UpdateMainplan);
        DetailCommand = new DelegateCommand<MainPlanDto>(Navigate);
        PackingInfoCommand = new DelegateCommand<MainPlanDto>(PackingInfo);
        ExecuteCommand = new DelegateCommand<string>(Execute);
    }
    public DelegateCommand OpenPlanCommand { get; }
    public DelegateCommand OpenCsvCommand { get; }
    public DelegateCommand UpdateCommand { get; }
    public DelegateCommand<string> ExecuteCommand { get; }
    public DelegateCommand<MainPlanDto> DetailCommand { get; }
    public DelegateCommand<MainPlanDto> PackingInfoCommand { get; }
    #endregion

    #region 角色控制属性
    private string updateRoles;
    public string UpdateRoles
    {
        get => updateRoles;
        set { updateRoles = value;RaisePropertyChanged(); }
    }
    #endregion

    #region 属性
    public ObservableCollection<MainPlanDto> MainPlanDtos { get; }
    public ObservableCollection<MainPlanDto> FilterMainPlanDtos { get; }

    private string search;
    /// <summary>
    /// 搜索条件属性
    /// </summary>
    public string Search
    {
        get => search;
        set => SetProperty(ref search,value);
    }
    #endregion

    #region 更新计划
    private void OpenPlan()
    {
        var path = $@"Y:\Production\Production Document\Production plan\Production Schedule for Hood Projects {DateTime.Today.Year}.xlsm";
        var excelApp = new Application();
        excelApp.Workbooks.Add(path);
        excelApp.Visible=true;
    }

    private void OpenCsv()
    {
        var path = Path.Combine(Environment.CurrentDirectory, "MainPlan.csv");
        var excelApp = new Application();
        excelApp.Workbooks.Open(path);
        excelApp.Visible=true;
    }

    private async void UpdateMainplan()
    {
        var path = Path.Combine(Environment.CurrentDirectory, "MainPlan.csv");
        List<MainPlanCsv> mainplans;
        try
        {
            mainplans= path.GetRecords<MainPlanCsv>();
        }
        //StreamReader的异常
        catch (Exception e)
        {
            Aggregator.SendMessage($"请关闭Excel后再执行更新，{e.Message}");
            return;
        }

        var dtos = (await _mainPlanService.GetAllAsync()).Result;
        foreach (var mainplan in mainplans)
        {
            //判断编号是否相同
            var dto = dtos.FirstOrDefault(x => x.Number.Equals(mainplan.Number)) ?? new MainPlanDto { Id = Guid.Empty };
            //解析mainplan
            dto.Batch = mainplan.Batch.ToInt();
            dto.CreateTime = mainplan.CreateTime.ToDateTime();
            dto.Number = mainplan.Number;
            dto.Name = mainplan.Name;
            dto.Quantity = mainplan.Quantity.ToInt();
            dto.ItemLine=mainplan.ItemLine.ToInt();
            dto.FinishTime=mainplan.FinishTime.ToDateTime();
            dto.DrwReleaseTarget=mainplan.DrwReleaseTarget.ToDateTime();
            dto.DrwReleaseTime = mainplan.DrwReleaseTime.ToDateTimeWithNull();
            dto.WarehousingTime = mainplan.PackingDate.ToDateTimeWithNull();
            if (dto.WarehousingTime != null && dto.ShippingTime == null)
            {
                dto.Status = MainPlanStatus_e.入库;
            }
            else if (dto.DrwReleaseTime != null && dto.WarehousingTime == null)
            {
                dto.Status = MainPlanStatus_e.生产;
            }
            else if (dto.DrwReleaseTime==null)
            {
                dto.Status = MainPlanStatus_e.制图;
            }
            dto.ModelSummary=mainplan.ModelSummary;
            dto.Workload=mainplan.Workload.ToDouble();
            dto.MonthOfInvoice=mainplan.MonthOfInvoice.ToMonth();
            dto.Value=mainplan.Value.ToDouble();
            dto.Remarks = mainplan.Purchase.ToRemarks(dto.Remarks);
            dto.Remarks = mainplan.Remark.ToRemarks(dto.Remarks);

            if (dto.Id==Guid.Empty)
            {
                dto.MainPlanType = MainPlanType_e.ETO;
                var result = await _mainPlanService.AddAsync(dto);
                var addDto = result.Result;
                await _mainPlanService.UpdateStatusesAsync(addDto.Id.Value, dto);
            }
            else
            {
                await _mainPlanService.UpdateAsync(dto.Id.Value, dto);
                await _mainPlanService.UpdateStatusesAsync(dto.Id.Value, dto);
            }
        }
        Aggregator.SendMessage("更新完毕！");
        GetDataAsync();
    } 
    #endregion

    #region 导航到详细页面和装箱信息页面
    /// <summary>
    /// 跳转到项目详细页面，传递dto
    /// </summary>
    private async void Navigate(MainPlanDto obj)
    {
        if(obj.ProjectId==null||obj.ProjectId.Equals(Guid.Empty))return;
        var response =await _projectService.GetFirstOrDefault(obj.ProjectId.Value);
        if (!response.Status) return;
        var project = response.Result;
        //将dto传递给要导航的页面
        NavigationParameters param = new NavigationParameters { { "Value", project } };
        RegionManager.Regions[PrismManager.MainViewRegionName].RequestNavigate("DetailView", back =>
        {
            Journal = back.Context.NavigationService.Journal;
        }, param);
    }

    /// <summary>
    /// 导航到装箱信息界面
    /// </summary>
    /// <param name="obj"></param>
    private async void PackingInfo(MainPlanDto obj)
    {
        if (obj.ProjectId==null||obj.ProjectId.Equals(Guid.Empty)) return;
        var packingListParam = new PackingListParam
        {
            ProjectId = obj.ProjectId,
            Batch = obj.Batch,
            ProjectName = $"{obj.Number}-{obj.Name}"
        };
        
        //先判断能不能导航：
        //根据项目ID和分批获取装箱信息表
        var response = await _packingListService.GetPackingInfoAsync(packingListParam);
        if (response.Status)
        {
            var param = new NavigationParameters { { "Value", packingListParam } };
            RegionManager.Regions[PrismManager.MainViewRegionName].RequestNavigate("PackingInfoView", back =>
            {
                Journal = back.Context.NavigationService.Journal;
            }, param);
        }
        else
        {
            Aggregator.SendMessage("技术部还没出装箱清单！");
        }
    }


    #endregion

    #region 筛选
    private void Execute(string obj)
    {
        switch (obj)
        {
            case "Query": Filter();break;

        }

        
    }
    private void Filter()
    {
        FilterMainPlanDtos.Clear();
        FilterMainPlanDtos.AddRange(
            MainPlanDtos.Where(x =>
                string.IsNullOrEmpty(Search)||
                x.Number.Contains(Search,StringComparison.OrdinalIgnoreCase)|| 
                x.Name.Contains(Search, StringComparison.OrdinalIgnoreCase)||
                ((!string.IsNullOrEmpty(x.ModelSummary))&& x.ModelSummary.Contains(Search, StringComparison.OrdinalIgnoreCase))||
                ((!string.IsNullOrEmpty(x.Remarks))&&x.Remarks!.Contains(Search, StringComparison.OrdinalIgnoreCase))));
        //有的时候ModelSummary或Remarks是null，导致调用Contains报错

    }
    #endregion

    #region 导航初始化
    private async void GetDataAsync()
    {
        var result = await _mainPlanService.GetAllAsync();
        if (!result.Status) return;
        MainPlanDtos.Clear();
        MainPlanDtos.AddRange(result.Result);
        Filter();
    }
    public override void OnNavigatedTo(NavigationContext navigationContext)
    {
        base.OnNavigatedTo(navigationContext);
        //SelectedProjectStatus = navigationContext.Parameters.ContainsKey("Value")
        //    ? navigationContext.Parameters.GetValue<int>("Value")
        //    : 0;
        GetDataAsync();
        UpdateRoles = "admin,pm,mgr,pmc";
    }
    #endregion


}