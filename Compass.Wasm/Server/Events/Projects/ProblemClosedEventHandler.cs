namespace Compass.Wasm.Server.Events.Projects;

//处理ProblemController发出的集成事件，关闭项目异常，搜索项目下所有的Problem，看是否全部关闭，将项目跟踪的项目的项目未解决修改未false
[EventName("ProjectService.Problem.Closed")]
public class ProblemClosedEventHandler : JsonIntegrationEventHandler<ProblemClosedEvent>
{
    private readonly ProjectDbContext _dbContext;
    public ProblemClosedEventHandler(ProjectDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public override async Task HandleJson(string eventName, ProblemClosedEvent? eventData)
    {
        //延迟1s钟
        await Task.Delay(1000);
        //项目下所有的异常
        var problems = _dbContext.Problems.Where(x => x.ProjectId.Equals(eventData!.ProjectId));
        //如果所有的异常已经关闭，将Tracking中的ProblemNotResolved修改未false
        var tracking = await _dbContext.Trackings.SingleAsync(x => x.Id.Equals(eventData!.ProjectId));
        //如果有一个是非关闭状态则退出，什么也不做
        //将tracking中异常状态修改为判断结果，如果有则为true，没有则为false
        tracking.ChangeProblemNotResolved(problems.Any(x => !x.IsClosed));
        await _dbContext.SaveChangesAsync();
    }
}