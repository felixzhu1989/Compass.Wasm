using Compass.Wasm.Shared.Projects.Notifs;

namespace Compass.Wasm.Server.Events.NotifHandlers;

public class TestNotificationHandler : NotificationHandler<TestNotification>
{
    protected override void Handle(TestNotification notification)
    {
        Console.WriteLine($@"测试{notification.Name}");
    }
}