namespace Compass.Wasm.Shared.PlanService;

public class ProductionPlanResponse
{
    public Guid Id { get; set; }
    public DateTime OdpReleaseTime { get; set; }//创建时间
    public string SqNumber { get; set; }//SQ号码
    public string Name { get; set; }
    public int Quantity { get; set; }
    public string? ModelSummary { get; set; }
    public DateTime ProductionFinishTime { get; set; }
    public DateTime DrawingReleaseTarget { get; set; }
    public DateTime MonthOfInvoice { get; set; }//开票月份，input type=month
    public ProductionPlanType_e ProductionPlanType { get; set; }//海工, ETO, KFC
    public string? Remarks { get; set; }

    public Guid? ProjectId { get; set; }//关联项目
    public DateTime? DrawingReleaseActual { get;set; }
}