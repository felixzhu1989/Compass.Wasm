namespace Compass.Wasm.Server.Services.Identities;
//RabbitMQ集成事件，将收到的Json字符串序列化为字符串
//处理UserAdminController发出的集成事件，更改密码后发送邮件通知对方
[EventName("IdentityService.User.PasswordChange")]
public class ChangePasswordEventHandler : JsonIntegrationEventHandler<ChangePasswordEvent>
{
    private readonly ILogger<ChangePasswordEventHandler> _logger;
    private readonly IEmailSender _emailSender;
    public ChangePasswordEventHandler(ILogger<ChangePasswordEventHandler> logger, IEmailSender emailSender)
    {
        _logger = logger;
        _emailSender = emailSender;
    }
    public override Task HandleJson(string eventName, ChangePasswordEvent? eventData)
    {
        _logger.LogInformation($"发送密码给用户邮箱：{eventData!.Email}");
        //自定义消息
        var message = $"用户名：{eventData.UserName}<br>邮箱：{eventData.Email}<br>密码：<span style=\"color:red\">{eventData.Password}</span><br>";
        //发送密码给用户的邮箱
        return _emailSender.SendAsync(eventData.UserName, eventData.Email, "Compass登录密码修改", message);
    }
}