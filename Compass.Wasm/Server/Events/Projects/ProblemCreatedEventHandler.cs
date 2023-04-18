using System.Text;

namespace Compass.Wasm.Server.Events.Projects;

//处理ProblemController发出的集成事件，生成项目异常后，将项目跟踪的异常为解决修改未true
[EventName("ProjectService.Problem.Created")]
public class ProblemCreatedEventHandler : JsonIntegrationEventHandler<ProblemCreatedEvent>
{
    private readonly ProjectDbContext _dbContext;
    private readonly IEmailSender _emailSender;

    public ProblemCreatedEventHandler(ProjectDbContext dbContext, IEmailSender emailSender)
    {
        _dbContext = dbContext;
        _emailSender = emailSender;
    }
    public override async Task HandleJson(string eventName, ProblemCreatedEvent? eventData)
    {
        var tracking = await _dbContext.Trackings.SingleAsync(x => x.Id.Equals(eventData!.ProjectId));
        //将tracking中异常状态修改为true
        tracking.ChangeProblemNotResolved(true);
        await _dbContext.SaveChangesAsync();
        //todo:群发邮件给能指派异常负责人的相关人员admin，manager，pm
        //自定义消息
        StringBuilder message = new StringBuilder();
        message.Append($"<span>项目：{eventData.OdoNumber} {eventData.ProjectName}</span><br>");
        message.Append($"<span>发生异常：{eventData.ProblemDesc}</span><br>");
        message.Append($"<span>上报人：{eventData.Reporter}</span><br>");
        message.Append($"<span>链接：<a href=\"{eventData.Url}\">{eventData.Url}</a></span><br>");
        message.Append("<span>请及时指派负责人</span><br>");
        //群发邮件
        await _emailSender.MassSendAsync(eventData.Emails, $"{eventData.OdoNumber}发生异常", message.ToString());
    }
}