using Compass.PlanService.Infrastructure;

namespace Compass.Wasm.Server.Events.Plans;
//处理集成事件
[EventName("PlanService.MainPlan.BindProject")]
public class BindProjectEventHandler : JsonIntegrationEventHandler<BindProjectEvent>
{

    private readonly ProjectDbContext _pmDbContext;
    private readonly PlanDbContext _psDbContext;
    public BindProjectEventHandler(ProjectDbContext pmDbContext, PlanDbContext psDbContext)
    {
        _pmDbContext = pmDbContext;
        _psDbContext = psDbContext;
    }
    public override async Task HandleJson(string eventName, BindProjectEvent? eventData)
    {
        //Delay2s
        await Task.Delay(2000);
        if (eventData.Id != null)
        {
            var tracking = await _pmDbContext.Trackings.FirstOrDefaultAsync(x => x.Id.Equals(eventData.Id));
            if (tracking != null)
            {
                tracking.ChangeSortDate(eventData.FinishTime);
                await _pmDbContext.SaveChangesAsync();
            }
        }
    }
}