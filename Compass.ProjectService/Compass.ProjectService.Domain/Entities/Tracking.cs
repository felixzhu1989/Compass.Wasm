using Compass.Wasm.Shared.ProjectService;
using Zack.DomainCommons.Models;

namespace Compass.ProjectService.Domain.Entities;
/// <summary>
/// 记录项目进度跟踪信息，实际发生的时间
/// </summary>
public record Tracking : AggregateRootEntity, IAggregateRoot, IHasCreationTime, ISoftDelete
{
    public Guid ProjectId { get; private set; }
    //状态表示当前，根据EventBus接收到事件，自动修改
    public ProjectStatus ProjectStatus { get; set; }//计划,制图,生产,检验,完工,结束
    public DateTime ReceiveTime { get; private set; }//收到ODP的时间（合同右上角）-> 项目进入计划状态（产生该跟踪记录时必填）
    public DateTime DrawingPlanedTime { get; private set; }//制定制图计划的时间->进入制图状态
    public DateTime DrawingReleaseTime { get; private set; }//发出生产图纸的时间->进入生产状态
    public DateTime InspectionTime { get; private set; }//生产报检的时间->进入检验状态
    public DateTime ProdCompletedTime { get; private set; }//生产完工的时间->进入完工状态
    public DateTime CloseTime { get; private set; }//项目真实发货的时间->进入结束状态，用减去合同DeliveryDate，用户计算成品库存时间
}