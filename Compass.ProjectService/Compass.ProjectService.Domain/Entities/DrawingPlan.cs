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
    }
    public DrawingPlan ChangeReleaseTime(DateTime releaseTime)
    {
        ReleaseTime = releaseTime;
        return this;
    }
}