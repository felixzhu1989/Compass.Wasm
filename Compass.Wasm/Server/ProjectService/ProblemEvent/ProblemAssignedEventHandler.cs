using System.Text;

namespace Compass.Wasm.Server.ProjectService.ProblemEvent;

//处理ProblemController发出的集成事件，异常指派责任人后邮件通知责任人，
[EventName("ProjectService.Problem.Assigned")]
public class ProblemAssignedEventHandler:JsonIntegrationEventHandler<ProblemAssignedEvent>
{
    private readonly IEmailSender _emailSender;
    public ProblemAssignedEventHandler(IEmailSender emailSender)
    {
        _emailSender = emailSender;
    }
    public override Task HandleJson(string eventName, ProblemAssignedEvent? eventData)
    {
        //自定义消息
        StringBuilder message = new StringBuilder();
        message.Append($"<span>项目：{eventData.OdoNumber} {eventData.ProjectName}</span><br>");
        message.Append($"<span>发生异常：{eventData.ProblemDesc}</span><br>");
        message.Append($"<span>处理负责人：{eventData.Responder}</span><br>");
        message.Append($"<span>截止日期：{eventData.Deadline.ToShortDateString()}</span><br>");
        message.Append($"<span>链接：<a href=\"{eventData.Url}\">{eventData.Url}</a></span><br>");
        //单发邮件
        return _emailSender.SendAsync(eventData.Responder, eventData.Email, $"{eventData.OdoNumber}异常处理负责人{eventData.Responder}", message.ToString());
    }
}