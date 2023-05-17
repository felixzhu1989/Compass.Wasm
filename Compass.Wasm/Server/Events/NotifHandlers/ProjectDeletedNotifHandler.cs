using Compass.PlanService.Infrastructure;
using Compass.Wasm.Shared.Projects.Notifs;

namespace Compass.Wasm.Server.Events.NotifHandlers;

public class ProjectDeletedNotifHandler : NotificationHandler<ProjectDeletedNotif>
{
    private readonly ProjectDbContext _dbContext;
    private readonly PlanDbContext _planDbContext;

    public ProjectDeletedNotifHandler(ProjectDbContext dbContext, PlanDbContext planDbContext)
    {
        _dbContext = dbContext;
        _planDbContext = planDbContext;
    }
    protected override void Handle(ProjectDeletedNotif notification)
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