using Compass.Wasm.Shared.Projects;

namespace Compass.Wasm.Server.Events.Projects;
//处理DrawingPlanController发出的集成事件，创建制图计划后，将项目跟踪得项目状态修改成制图
[EventName("ProjectService.Tracking.Warehousing")]
public class WarehousingEventHandler : JsonIntegrationEventHandler<WarehousingEvent>
{
    private readonly ProjectDbContext _dbContext;

    public WarehousingEventHandler(ProjectDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public override async Task HandleJson(string eventName, WarehousingEvent? eventData)
    {
        var project = await _dbContext.Projects.SingleAsync(x => x.Id.Equals(eventData!.ProjectId));
        project.ChangeProjectStatus(ProjectStatus_e.入库);
        await _dbContext.SaveChangesAsync();
    }
}