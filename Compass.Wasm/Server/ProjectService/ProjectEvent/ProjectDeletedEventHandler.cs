namespace Compass.Wasm.Server.ProjectService.ProjectEvent;

//处理ProjectController发出的集成事件，删除订单后，继续删除跟踪对象和制图计划（判断有没有）
[EventName("ProjectService.Project.Deleted")]
public class ProjectDeletedEventHandler: JsonIntegrationEventHandler<ProjectDeletedEvent>
{
    private readonly PMDbContext _dbContext;
    public ProjectDeletedEventHandler(PMDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public override async Task HandleJson(string eventName, ProjectDeletedEvent? eventData)
    {
        var tracking = await _dbContext.Trackings.SingleOrDefaultAsync(x=>x.Id.Equals(eventData.Id));
        if (tracking != null) tracking.SoftDelete();//软删除
        var drawingPlan = await _dbContext.DrawingsPlan.SingleOrDefaultAsync(x => x.Id.Equals(eventData.Id));
        if (drawingPlan != null) drawingPlan.SoftDelete();//软删除

        await _dbContext.SaveChangesAsync();
    }
}