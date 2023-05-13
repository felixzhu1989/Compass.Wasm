using Compass.PlanService.Infrastructure;
using Compass.Wasm.Shared.Projects.Notifications;

namespace Compass.Wasm.Server.Events.NotificationHandler;

public class ProjectDeletedNotificationHandler : NotificationHandler<ProjectDeletedNotification>
{
    private readonly ProjectDbContext _dbContext;
    private readonly PlanDbContext _planDbContext;

    public ProjectDeletedNotificationHandler(ProjectDbContext dbContext, PlanDbContext planDbContext)
    {
        _dbContext = dbContext;
        _planDbContext = planDbContext;
    }
    protected override void Handle(ProjectDeletedNotification notification)
    {
        //同时删除计划绑定的项目
        var prodPlans = _planDbContext.MainPlans.Where(x => x.ProjectId.Equals(notification.Id));
        foreach (var plan in prodPlans) 
        {
            plan.ChangeProjectId(null);
            _planDbContext.SaveChangesAsync();
        }
    }
}