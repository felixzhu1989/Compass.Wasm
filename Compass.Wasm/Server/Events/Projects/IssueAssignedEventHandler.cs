using System.Text;

namespace Compass.Wasm.Server.Events.Projects;
public record IssueAssignedEvent(string Responder, string Email, string Number, string Name, string Content, DateTime Deadline, string Url);
//处理ProblemController发出的集成事件，异常指派责任人后邮件通知责任人，
[EventName("ProjectService.Problem.Assigned")]
public class IssueAssignedEventHandler : JsonIntegrationEventHandler<IssueAssignedEvent>
{
    private readonly IEmailSender _emailSender;
    public IssueAssignedEventHandler(IEmailSender emailSender)
    {
        _emailSender = emailSender;
    }
    public override Task HandleJson(string eventName, IssueAssignedEvent? eventData)
    {
        //自定义消息
        StringBuilder message = new StringBuilder();
        message.Append($"<span>项目：{eventData.Number} {eventData.Name}</span><br>");
        message.Append($"<span>发生异常：{eventData.Content}</span><br>");
        message.Append($"<span>处理负责人：{eventData.Responder}</span><br>");
        message.Append($"<span>截止日期：{eventData.Deadline.ToShortDateString()}</span><br>");
        message.Append($"<span>链接：<a href=\"{eventData.Url}\">{eventData.Url}</a></span><br>");
        //单发邮件
        return _emailSender.SendAsync(eventData.Responder, eventData.Email, $"{eventData.Number}异常处理负责人{eventData.Responder}", message.ToString());
    }
}