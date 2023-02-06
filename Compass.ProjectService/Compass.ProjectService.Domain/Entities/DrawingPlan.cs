using Compass.Wasm.Shared.ProjectService.Notification;
using Zack.DomainCommons.Models;

namespace Compass.ProjectService.Domain.Entities;

public record DrawingPlan : AggregateRootEntity, IAggregateRoot, IHasCreationTime, ISoftDelete
{
    public DateTime ReleaseTime { get; private set; }

    private DrawingPlan() { }
    public DrawingPlan(Guid id,DateTime releaseTime)
    {
        Id=id;
        ReleaseTime = releaseTime;
        //添加领域事件，添加计划后，将项目跟踪修改成制图状态
        AddDomainEvent(new DrawingPlanCreatedNotification(id));
    }
    public DrawingPlan ChangeReleaseTime(DateTime releaseTime)
    {
        ReleaseTime = releaseTime;
        return this;
    }
}