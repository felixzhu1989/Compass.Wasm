using Compass.Wpf.ApiServices.Plans;
using CsvHelper;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using Compass.Wasm.Shared.Extensions;
using Application = Microsoft.Office.Interop.Excel.Application;

namespace Compass.Wpf.ViewModels;

public class MainPlansViewModel : NavigationViewModel
{

    #region ctor
    private readonly IMainPlanService _mainPlanService;
    public MainPlansViewModel(IContainerProvider provider) : base(provider)
    {
        _mainPlanService = provider.Resolve<IMainPlanService>();

        MainPlanDtos = new ObservableCollection<MainPlanDto>();

        OpenPlanCommand = new DelegateCommand(OpenPlan);
        OpenCsvCommand = new DelegateCommand(OpenCsv);
        UpdateCommand = new DelegateCommand(UpdateMainplan);
    }
    public DelegateCommand OpenPlanCommand { get; }
    public DelegateCommand OpenCsvCommand { get; }
    public DelegateCommand UpdateCommand { get; }
    #endregion

    #region 属性
    private ObservableCollection<MainPlanDto> mainPlanDtos;
    public ObservableCollection<MainPlanDto> MainPlanDtos
    {
        get => mainPlanDtos;
        set { mainPlanDtos = value; RaisePropertyChanged(); }
    }
    private string search;
    /// <summary>
    /// 搜索条件属性
    /// </summary>
    public string Search
    {
        get => search;
        set { search = value; RaisePropertyChanged(); }
    }
    #endregion

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
            if(dto.WarehousingTime != null && dto.ShippingTime == null)
            {
                dto.Status = MainPlanStatus_e.入库;
            }
            else if (dto.DrwReleaseTime != null && dto.WarehousingTime == null)
            {
                dto.Status = MainPlanStatus_e.生产;
            }
            else if(dto.DrwReleaseTime==null)
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

    #region 导航初始化
    private async void GetDataAsync()
    {
        var result = await _mainPlanService.GetAllAsync();
        if (!result.Status) return;
        MainPlanDtos.Clear();
        MainPlanDtos.AddRange(result.Result);
    }
    public override void OnNavigatedTo(NavigationContext navigationContext)
    {
        base.OnNavigatedTo(navigationContext);
        //SelectedProjectStatus = navigationContext.Parameters.ContainsKey("Value")
        //    ? navigationContext.Parameters.GetValue<int>("Value")
        //    : 0;
        GetDataAsync();
    }
    #endregion


}