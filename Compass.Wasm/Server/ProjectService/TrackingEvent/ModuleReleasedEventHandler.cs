using Compass.Wasm.Shared.ProjectService;

namespace Compass.Wasm.Server.ProjectService.TrackingEvent;
//处理ModuleController发出的集成事件，发出生产图后，将项目跟踪的项目状态修改成生产，记录发图时间
[EventName("ProjectService.Module.Released")]
public class ModuleReleasedEventHandler : JsonIntegrationEventHandler<ModuleReleasedEvent>
{
    private readonly PMDbContext _dbContext;
    public ModuleReleasedEventHandler(PMDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public override async Task HandleJson(string eventName, ModuleReleasedEvent? eventData)
    {
        var tracking = await _dbContext.Trackings.SingleAsync(x => x.Id.Equals(eventData!.ProjectId));
        tracking.ChangeModuleReleaseTime(DateTime.Now).ChangeProjectStatus(ProjectStatus.生产);
        await _dbContext.SaveChangesAsync();
    }
}