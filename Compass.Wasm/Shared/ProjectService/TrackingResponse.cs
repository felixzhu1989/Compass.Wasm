namespace Compass.Wasm.Shared.ProjectService;

public record TrackingResponse
{
    public Guid Id { get; set; }
    public bool ProblemNotResolved { get; set; }//有没有待解决得问题，如果有则另起一行显示异常详细信息

    //对象初始化时-> 项目进入计划状态（产生该跟踪记录时必填）
    //实际发生时间，进度相关参数
    //制定制图计划的时间->进入制图状态
    //发出生产图纸的时间->进入生产状态
    public DateTime? WarehousingTime { get; set; }//生产完工入库的时间->进入库存状态
    public DateTime? ShippingStartTime { get; set; }//项目真实发货的时间->进入结束状态，减去WarehousingTime，用户计算成品库存时间
    public DateTime? ShippingEndTime { get; set; }//所有产品都发货了的时间->进入结束状态（是否需要售后状态？）

    //附加Project中DeliveryDate的属性，用于排序
    public DateTime SortDate { get; init; }
}