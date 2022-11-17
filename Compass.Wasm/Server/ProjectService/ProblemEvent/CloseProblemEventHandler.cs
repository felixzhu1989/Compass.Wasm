namespace Compass.Wasm.Server.ProjectService.ProblemEvent;

//处理ProblemController发出的集成事件，关闭项目异常，搜索项目下所有的Problem，看是否全部关闭，将项目跟踪的项目的项目未解决修改未false
[EventName("ProjectService.Problem.Closed")]
public class CloseProblemEventHandler : JsonIntegrationEventHandler<CloseProblemEvent>
{
    private readonly PMDbContext _dbContext;
    public CloseProblemEventHandler(PMDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public override async Task HandleJson(string eventName, CloseProblemEvent? eventData)
    {
        //延迟5s钟
        await Task.Delay(5000);
        //项目下所有的异常
        var problems = _dbContext.Problems.Where(x => x.ProjectId.Equals(eventData!.ProjectId));
        //如果有一个是非关闭状态则退出，什么也不做
        if (problems.Any(x => !x.IsClosed)) return;
        //如果所有的异常已经关闭，将Tracking中的ProblemNotResolved修改未false
        var tracking = await _dbContext.Trackings.SingleAsync(x => x.Id.Equals(eventData!.ProjectId));
        //将tracking中异常状态修改为false
        tracking.ChangeProblemNotResolved(false);
        await _dbContext.SaveChangesAsync();
    }
}