using Compass.Wasm.Shared.Projects;
using Compass.Wasm.Shared.Projects.Notifications;

namespace Compass.Wasm.Server.Events.NotificationHandler;

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