using Compass.Wasm.Shared.Projects;

namespace Compass.Wasm.Shared.Plans;

public class MainPlanDto
{
    public Guid Id { get; set; }

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
    
    //状态属性
    public Guid? ProjectId { get; set; }//关联项目,可以多个主计划关联到一个订单
    public MainPlanStatus_e Status { get; set; }//计划,制图,生产,入库,结束

    public DateTime? DrwReleaseTime { get;set; }
    public DateTime? WarehousingTime { get; set; }//第一台生产完工入库的时间->进入库存状态
    public DateTime? ShippingTime { get; set; }//项目第一台真实发货的时间->进入发货状态，用减去WarehousingTime，用户计算成品库存时间

    //查询时附加Project的属性
    public string? OdpNumber { get; set; }
    public string? ProjectName { get; set; }
    public bool ProblemNotResolved { get; set; }
    public List<ProblemDto>? ProblemDtos { get; set; } = new();

}