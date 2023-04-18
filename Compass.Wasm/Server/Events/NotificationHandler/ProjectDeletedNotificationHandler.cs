using Compass.PlanService.Infrastructure;
using Compass.Wasm.Shared.Projects.Notifications;

namespace Compass.Wasm.Server.Events.NotificationHandler;

public class ProjectDeletedNotificationHandler : NotificationHandler<ProjectDeletedNotification>
{
    private readonly ProjectDbContext _dbContext;
    private readonly PlanDbContext _psDbContext;

    public ProjectDeletedNotificationHandler(ProjectDbContext dbContext, PlanDbContext psDbContext)
    {
        _dbContext = dbContext;
        _psDbContext = psDbContext;
    }
    protected override void Handle(ProjectDeletedNotification notification)
    {
        //删除项目跟踪数据
        var tracking = _dbContext.Trackings.SingleOrDefault(x => x.Id.Equals(notification.Id));
        if (tracking != null) tracking.SoftDelete();
        //删除项目制图计划数据
        var drawingPlan = _dbContext.DrawingsPlan.SingleOrDefault(x => x.Id.Equals(notification.Id));
        if (drawingPlan != null) drawingPlan.SoftDelete();

        //同时删除计划绑定的项目
        var prodPlan = _psDbContext.ProductionPlans.FirstOrDefault(x => x.ProjectId.Equals(notification.Id));
        if (prodPlan != null)
        {
            prodPlan.ChangeProjectId(null);
            _psDbContext.SaveChangesAsync();
        }
    }
}