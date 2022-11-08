using Zack.DomainCommons.Models;

namespace Compass.ProjectService.Domain.Entities;

public record DrawingPlan : AggregateRootEntity, IAggregateRoot, IHasCreationTime, ISoftDelete
{
    public Guid ProjectId { get; private set; }
    public DateTime ReleaseTime { get; private set; }

    private DrawingPlan() { }
    public DrawingPlan(Guid id, Guid projectId, DateTime releaseTime)
    {
        Id=id;
        ProjectId = projectId;
        ReleaseTime = releaseTime;
    }
    public DrawingPlan ChangeReleaseTime(DateTime releaseTime)
    {
        ReleaseTime = releaseTime;
        return this;
    }
}