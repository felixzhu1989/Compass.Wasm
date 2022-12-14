namespace Compass.Wasm.Server.IdentityService;

//处理UserAdminController发出的集成事件，创建新用户后发送邮件通知对方
[EventName("IdentityService.User.Created")]
public class UserCreatedEventHandler : JsonIntegrationEventHandler<UserCreatedEvent>
{
    private readonly ILogger<UserCreatedEventHandler> _logger;
    private readonly IEmailSender _emailSender;
    public UserCreatedEventHandler(ILogger<UserCreatedEventHandler> logger, IEmailSender emailSender)
    {
        _logger = logger;
        _emailSender = emailSender;
    }
    public override Task HandleJson(string eventName, UserCreatedEvent? eventData)
    {
        _logger.LogInformation($"发送初始密码给被创建用户的邮箱：{eventData!.Email}");
        //自定义消息
        var message = $"用户名：{eventData.UserName}<br>邮箱：{eventData.Email}<br>密码：<span style=\"color:navy\">{eventData.Password}</span><br>";
        //发送初始密码给被创建用户的邮箱
        return _emailSender.SendAsync(eventData.UserName,eventData.Email, "Compass新用户初始密码", message);
    }
}