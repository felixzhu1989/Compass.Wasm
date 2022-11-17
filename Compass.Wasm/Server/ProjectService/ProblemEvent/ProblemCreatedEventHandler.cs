namespace Compass.Wasm.Server.ProjectService.ProblemEvent;

//处理ProblemController发出的集成事件，生成项目异常后，将项目跟踪的异常为解决修改未true
[EventName("ProjectService.Problem.Created")]
public class ProblemCreatedEventHandler : JsonIntegrationEventHandler<ProblemCreatedEvent>
{
    private readonly PMDbContext _dbContext;
    public ProblemCreatedEventHandler(PMDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public override async Task HandleJson(string eventName, ProblemCreatedEvent? eventData)
    {
        var tracking = await _dbContext.Trackings.SingleAsync(x => x.Id.Equals(eventData!.ProjectId));
        //将tracking中异常状态修改为true
        tracking.ChangeProblemNotResolved(true);
        await _dbContext.SaveChangesAsync();
    }
}