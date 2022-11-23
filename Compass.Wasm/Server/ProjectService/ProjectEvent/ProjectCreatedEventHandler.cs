namespace Compass.Wasm.Server.ProjectService.ProjectEvent;

//处理ProjectController发出的集成事件，创建新订单后，继续创建跟踪对象
[EventName("ProjectService.Project.Created")]
public class ProjectCreatedEventHandler : JsonIntegrationEventHandler<ProjectCreatedEvent>
{
    //todo:是否需要发送邮件
    private readonly PMDbContext _dbContext;
    public ProjectCreatedEventHandler(PMDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public override Task HandleJson(string eventName, ProjectCreatedEvent? eventData)
    {
        //当项目创建时，同时创建跟踪对象
        var tracking = new Tracking(eventData!.Id);
        _dbContext.Trackings.Add(tracking);
        return _dbContext.SaveChangesAsync();
    }
}