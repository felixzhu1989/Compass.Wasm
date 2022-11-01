using Compass.IdentityService.Domain;
using Zack.EventBus;

namespace Compass.Wasm.Server.IdentityService;
//RabbitMQ集成事件，将收到的Json字符串序列化为字符串
//处理UserAdminController发出的集成事件，重置密码后发送邮件通知对方
[EventName("IdentityService.User.PasswordReset")]
public class ResetPasswordEventHandler : JsonIntegrationEventHandler<ResetPasswordEvent>
{
    private readonly ILogger<ResetPasswordEventHandler> _logger;
    private readonly IEmailSender _emailSender;
    public ResetPasswordEventHandler(ILogger<ResetPasswordEventHandler> logger, IEmailSender emailSender)
    {
        _logger = logger;
        _emailSender=emailSender;
    }
    public override Task HandleJson(string eventName, ResetPasswordEvent? eventData)
    {
        _logger.LogInformation($"发送密码给用户邮箱：{eventData.Email}");
        //自定义消息
        var message = $"用户名：{eventData.UserName}<br>邮箱：{eventData.Email}<br>密码：<span style=\"color:navy\">{eventData.Password}</span><br>";
        //发送密码给用户的邮箱
        return _emailSender.SendAsync(eventData.UserName,eventData.Email, "Compass登录密码重置", message);
    }
}