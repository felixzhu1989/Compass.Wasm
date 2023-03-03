using Compass.Wasm.Shared.ProjectService;
using Compass.Wasm.Shared.ProjectService.Notification;
namespace Compass.Wasm.Server.ProjectService.TrackingEvent;

public class DrawingPlanCreatedNotificationHandler : NotificationHandler<DrawingPlanCreatedNotification>
{
    private readonly ProjectDbContext _dbContext;

    public DrawingPlanCreatedNotificationHandler(ProjectDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    protected override void Handle(DrawingPlanCreatedNotification notification)
    {
        var project = _dbContext.Projects.Single(x => x.Id.Equals(notification.ProjectId));
        project.ChangeProjectStatus(ProjectStatus_e.制图);
    }
}