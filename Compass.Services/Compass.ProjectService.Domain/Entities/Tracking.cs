using Zack.DomainCommons.Models;

namespace Compass.ProjectService.Domain.Entities;
/// <summary>
/// 记录项目进度跟踪信息，实际发生的时间
/// </summary>
public record Tracking : AggregateRootEntity, IAggregateRoot, IHasCreationTime, ISoftDelete
{
    public bool ProblemNotResolved { get;private set; }//有没有待解决得问题，如果有则另起一行显示异常详细信息

    //对象初始化时-> 项目进入计划状态（产生该跟踪记录时必填）
    //实际发生时间，进度相关参数
    //制定制图计划的时间->进入制图状态
    //第一台发出生产图纸的时间->进入生产状态
    public DateTime? WarehousingTime { get; private set; }//第一台生产完工入库的时间->进入库存状态
    public DateTime? ShippingStartTime { get; private set; }//项目第一台真实发货的时间->进入发货状态，用减去WarehousingTime，用户计算成品库存时间
    public DateTime? ShippingEndTime { get; private set; }//所有产品都发货了的时间->进入结束状态（是否需要售后状态？）

    public DateTime SortDate { get; private set; }
    //public DateTime DeliveryDate { get; set; }//特征5，不映射到数据库

    private Tracking() { }
    public Tracking(Guid id,DateTime sortDate)//使用Project的Id作为Tracking的Id
    {
        Id = id;
        SortDate = sortDate;
    }
    public Tracking ChangeSortDate(DateTime sortDate)
    {
        SortDate = sortDate;
        return this;
    }
    public Tracking ChangeProblemNotResolved(bool problemNotResolved)
    {
        ProblemNotResolved = problemNotResolved;
        return this;
    }
    public Tracking ChangeWarehousingTime(DateTime? warehousingTime)
    {
        WarehousingTime = warehousingTime;
        return this;
    }
    public Tracking ChangeShippingStartTime(DateTime? shippingStartTime)
    {
        ShippingStartTime = shippingStartTime;
        return this;
    }
    public Tracking ChangeShippingEndTime(DateTime? shippingEndTime)
    {
        ShippingEndTime = shippingEndTime;
        return this;
    }
}