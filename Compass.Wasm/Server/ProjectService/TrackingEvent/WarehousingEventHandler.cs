
using Compass.Wasm.Shared.ProjectService;
using Zack.EventBus;

namespace Compass.Wasm.Server.ProjectService.TrackingEvent;
//处理DrawingPlanController发出的集成事件，创建制图计划后，将项目跟踪得项目状态修改成制图
[EventName("ProjectService.Tracking.Warehousing")]
public class WarehousingEventHandler: JsonIntegrationEventHandler<WarehousingEvent>
{
    private readonly ProjectDbContext _dbContext;

    public WarehousingEventHandler(ProjectDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public override async Task HandleJson(string eventName, WarehousingEvent? eventData)
    {
        var tracking = await _dbContext.Trackings.SingleAsync(x => x.Id.Equals(eventData!.ProjectId));
        tracking.ChangeProjectStatus(ProjectStatus.入库);
        await _dbContext.SaveChangesAsync();
    }
}