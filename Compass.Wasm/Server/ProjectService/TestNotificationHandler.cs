using Compass.Wasm.Shared.ProjectService.Notification;

namespace Compass.Wasm.Server.ProjectService;

public class TestNotificationHandler : NotificationHandler<TestNotification>
{
    protected override void Handle(TestNotification notification)
    {
        Console.WriteLine($"测试{notification.Name}");
    }
}