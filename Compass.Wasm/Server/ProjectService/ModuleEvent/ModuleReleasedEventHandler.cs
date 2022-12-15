using Compass.PlanService.Infrastructure;
using Compass.Wasm.Shared.ProjectService;

namespace Compass.Wasm.Server.ProjectService.ModuleEvent;
//处理ModuleController发出的集成事件，发出生产图后，将项目跟踪的项目状态修改成生产，记录发图时间
[EventName("ProjectService.Module.Released")]
public class ModuleReleasedEventHandler : JsonIntegrationEventHandler<ModuleReleasedEvent>
{
    private readonly ProjectDbContext _dbContext;
    private readonly PlanDbContext _planDbContext;

    public ModuleReleasedEventHandler(ProjectDbContext dbContext,PlanDbContext planDbContext)
    {
        _dbContext = dbContext;
        _planDbContext = planDbContext;
    }
    public override async Task HandleJson(string eventName, ModuleReleasedEvent? eventData)
    {
        var prodPlan =await _planDbContext.ProductionPlans.SingleOrDefaultAsync(x => x.ProjectId.Equals(eventData!.ProjectId));
        if (prodPlan!=null)
        {
            prodPlan.ChangeDrawingReleaseActual(DateTime.Now);
           await _planDbContext.SaveChangesAsync();
        }
        var tracking = await _dbContext.Trackings.SingleAsync(x => x.Id.Equals(eventData!.ProjectId));
        tracking.ChangeProjectStatus(ProjectStatus.生产);
        await _dbContext.SaveChangesAsync();
    }
}