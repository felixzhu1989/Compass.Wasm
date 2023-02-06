using Compass.Wasm.Shared.ProjectService;
using Compass.Wasm.Shared.ProjectService.Notification;
namespace Compass.Wasm.Server.ProjectService;

public class DrawingPlanCreatedNotificationHandler:NotificationHandler<DrawingPlanCreatedNotification>
{
    private readonly ProjectDbContext _dbContext;

    public DrawingPlanCreatedNotificationHandler(ProjectDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    protected override void Handle(DrawingPlanCreatedNotification notification)
    {
        var tracking = _dbContext.Trackings.Single(x => x.Id.Equals(notification.ProjectId));
        tracking.ChangeProjectStatus(ProjectStatus.制图);
    }
}