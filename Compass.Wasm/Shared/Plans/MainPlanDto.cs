namespace Compass.Wasm.Shared.Plans;

public class MainPlanDto:BaseDto
{
    public DateTime CreateTime { get; set; }//创建时间
    public string Number { get; set; }//SQ/FSO号码
    public string Name { get; set; }
    public int Quantity { get; set; }
    public string? ModelSummary { get; set; }
    public DateTime FinishTime { get; set; }
    public DateTime DrwReleaseTarget { get; set; }
    public DateTime MonthOfInvoice { get; set; }//开票月份，input type=month
    public MainPlanType_e MainPlanType { get; set; }//海工, ETO, KFC
    public string? Remarks { get; set; }
    public int Batch { get; set; }

    //记录但是不显示的属性
    public int ItemLine { get; set; }//订单行
    public double Workload { get;set; }//工作量
    public double Value { get; set; }//税后价格

    //状态属性
    public Guid? ProjectId { get; set; }//关联项目,可以多个主计划关联到一个订单
    public MainPlanStatus_e Status { get; set; }//计划,制图,生产,入库,结束

    public DateTime? DrwReleaseTime { get;set; }
    public DateTime? WarehousingTime { get; set; }//第一台生产完工入库的时间->进入库存状态
    public DateTime? ShippingTime { get; set; }//项目第一台真实发货的时间->进入发货状态，用减去WarehousingTime，用户计算成品库存时间
    public DateTime? ClosedTime { get; set; }//项目总结经验后结束的时间



    #region 查询时附加属性
    public bool AllIssueClosed { get; set; }
    public List<IssueDto> IssueDtos { get; set; } = new();




    #endregion

}