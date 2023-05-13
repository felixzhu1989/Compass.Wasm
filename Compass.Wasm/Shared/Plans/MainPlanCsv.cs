namespace Compass.Wasm.Shared.Plans;

public class MainPlanCsv
{
    public string? Batch { get; set; }//->DeliveryBatch_e
    public string? CreateTime { get; set; }//->DateTime,创建时间
    public string? Number { get; set; }//SQ/FSO号码
    public string? Name { get; set; } 
    public string? Quantity { get; set; } //->int
    public string? ItemLine { get; set; }//->int,订单行
    public string? FinishTime { get; set; }//->DateTime
    public string? DrwReleaseTarget { get; set; }//->DateTime
    public string? DrwReleaseTime { get; set; }//->double
    public string? ModelSummary { get; set; }
    public string? Workload { get; set; }//->double,工作量
    public string? MonthOfInvoice { get; set; }//->DateTime,开票月份，input type=month
    public string? Purchase { get; set; }
    public string? Value { get; set; }//->double,税后价格
    public string? Remark { get; set; }
}