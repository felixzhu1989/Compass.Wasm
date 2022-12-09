using Compass.PlanService.Infrastructure;

namespace Compass.Wasm.Server.ProjectService.ProjectEvent;

//处理ProjectController发出的集成事件，删除订单后，继续删除跟踪对象和制图计划（判断有没有）
[EventName("ProjectService.Project.Deleted")]
public class ProjectDeletedEventHandler : JsonIntegrationEventHandler<ProjectDeletedEvent>
{
    private readonly PMDbContext _dbContext;
    private readonly PSDbContext _psDbContext;

    public ProjectDeletedEventHandler(PMDbContext dbContext, PSDbContext psDbContext)
    {
        _dbContext = dbContext;
        _psDbContext = psDbContext;
    }
    public override async Task HandleJson(string eventName, ProjectDeletedEvent? eventData)
    {
        //删除项目跟踪数据
        var tracking = await _dbContext.Trackings.SingleOrDefaultAsync(x => x.Id.Equals(eventData.Id));
        if (tracking != null) tracking.SoftDelete();
        //删除项目制图计划数据
        var drawingPlan = await _dbContext.DrawingsPlan.SingleOrDefaultAsync(x => x.Id.Equals(eventData.Id));
        if (drawingPlan != null) drawingPlan.SoftDelete();

        await _dbContext.SaveChangesAsync();

        //同时删除计划绑定的项目                                                
        var prodPlan = await _psDbContext.ProductionPlans.FirstOrDefaultAsync(x => x.ProjectId.Equals(eventData.Id));
        if (prodPlan != null)
        {
            prodPlan.ChangeProjectId(null);
            await _psDbContext.SaveChangesAsync();
        }

        
    }
}