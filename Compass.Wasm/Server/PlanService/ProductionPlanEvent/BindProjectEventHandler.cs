using Compass.PlanService.Infrastructure;

namespace Compass.Wasm.Server.PlanService.ProductionPlanEvent;
//处理ProductionPlanController发出的集成事件，创建新订单后，继续创建跟踪对象
[EventName("PlanService.ProductionPlan.BindProject")]
public class BindProjectEventHandler: JsonIntegrationEventHandler<BindProjectEvent>
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
                tracking.ChangeSortDate(eventData.ProductionFinishTime);
                await _pmDbContext.SaveChangesAsync();
            }
        }
    }
}