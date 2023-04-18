using Compass.Wasm.Shared.Projects.Notifications;
using Zack.DomainCommons.Models;

namespace Compass.ProjectService.Domain.Entities;

public record DrawingPlan : AggregateRootEntity, IAggregateRoot, IHasCreationTime, ISoftDelete
{
    //默认Id为项目号
    public Guid UserId { get; private set; }//制图人，项目的主要制图人
    public DateTime AssignTime { get; private set; }//分配制图人的时间
    //发图时间可以由生产计划查询得到，如果没有查询得到，那么归类到未计划中

    private DrawingPlan() { }
    public DrawingPlan(Guid id)
    {
        Id=id;
        AssignTime = DateTime.Now;
        //添加领域事件，添加计划后，将项目跟踪修改成制图状态
        AddDomainEvent(new DrawingPlanCreatedNotification(id));
    }

    /// <summary>
    /// 修改制图人，重新分配项目制图人
    /// </summary>
    public DrawingPlan ChangeUserId(Guid userId)
    {
        UserId = userId;
        AssignTime = DateTime.Now;
        return this;
    }
}