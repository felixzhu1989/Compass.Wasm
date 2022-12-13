using Compass.Wasm.Shared.ProjectService;

namespace Compass.Wasm.Client.ProjectServices;
/// <summary>
/// Index页面数据显示模型
/// </summary>
public class TrackingModel
{
    public Guid Id { get; set; }
    public ProjectStatus ProjectStatus { get; set; }
    public DateTime SortDate { get; init; }

    #region Project
    public string? OdpNumber { get; set; }
    public string? ProjectName { get; set; }
    #endregion

    #region ProductionPlan

    public bool ProductionPlanOk { get; set; } = false;//是否已经绑定生产计划
    public DateTime OdpReleaseTime { get; set; }
    public DateTime ProductionFinishTime { get; set; }
    public DateTime DrawingReleaseTarget { get; set; }
    public DateTime? DrawingReleaseActual { get; set; }
    #endregion

    public DateTime? WarehousingTime { get; set; }//生产完工入库的时间->进入库存状态
    public DateTime? ShippingStartTime { get; set; }//项目真实发货的时间->进入结束状态，减去WarehousingTime，用户计算成品库存时间
    public DateTime? ShippingEndTime { get; set; }//所有产品都发货了的时间->进入结束状态（是否需要售后状态？）

    public bool ProblemNotResolved { get; set; }
    #region Problem
    public List<ProblemResponse>? Problems { get; set; } = new();
    #endregion

}