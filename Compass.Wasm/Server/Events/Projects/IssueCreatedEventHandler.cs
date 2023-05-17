using Compass.Wasm.Shared.Identities;
using System.Text;

namespace Compass.Wasm.Server.Events.Projects;

public record IssueCreatedEvent(List<EmailAddress> Emails, Guid MainPlanId, string Number, string Name, string Reporter, string Content, string Url);
//处理ProblemController发出的集成事件，生成项目异常后，将项目跟踪的异常为解决修改未true
[EventName("ProjectService.Issue.Created")]
public class IssueCreatedEventHandler : JsonIntegrationEventHandler<IssueCreatedEvent>
{
    private readonly ProjectDbContext _dbContext;
    private readonly IEmailSender _emailSender;

    public IssueCreatedEventHandler(ProjectDbContext dbContext, IEmailSender emailSender)
    {
        _dbContext = dbContext;
        _emailSender = emailSender;
    }
    public override async Task HandleJson(string eventName, IssueCreatedEvent? eventData)
    {
        //todo:群发邮件给能指派异常负责人的相关人员admin，manager，pm
        //自定义消息
        StringBuilder message = new StringBuilder();
        message.Append($"<span>项目：{eventData.Number} {eventData.Name}</span><br>");
        message.Append($"<span>发生异常：{eventData.Content}</span><br>");
        message.Append($"<span>上报人：{eventData.Reporter}</span><br>");
        message.Append($"<span>链接：<a href=\"{eventData.Url}\">{eventData.Url}</a></span><br>");
        message.Append("<span>请及时指派负责人</span><br>");
        //群发邮件
        await _emailSender.MassSendAsync(eventData.Emails, $"{eventData.Number}发生异常", message.ToString());
    }
}