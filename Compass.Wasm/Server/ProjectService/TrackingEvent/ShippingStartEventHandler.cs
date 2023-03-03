using Compass.Wasm.Shared.ProjectService;
using Zack.EventBus;

namespace Compass.Wasm.Server.ProjectService.TrackingEvent;
//处理DrawingPlanController发出的集成事件，创建制图计划后，将项目跟踪得项目状态修改成制图
[EventName("ProjectService.Tracking.ShippingStart")]
public class ShippingStartEventHandler: JsonIntegrationEventHandler<ShippingStartEvent>
{
    private readonly ProjectDbContext _dbContext;

    public ShippingStartEventHandler(ProjectDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public override async Task HandleJson(string eventName, ShippingStartEvent? eventData)
    {
        var project = await _dbContext.Projects.SingleAsync(x => x.Id.Equals(eventData!.ProjectId));
        project.ChangeProjectStatus(ProjectStatus_e.发货);
        await _dbContext.SaveChangesAsync();
    }
}