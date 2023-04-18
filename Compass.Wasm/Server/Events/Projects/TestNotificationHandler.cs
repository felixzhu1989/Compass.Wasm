using Compass.Wasm.Shared.Projects.Notifications;

namespace Compass.Wasm.Server.Events.Projects;

public class TestNotificationHandler : NotificationHandler<TestNotification>
{
    protected override void Handle(TestNotification notification)
    {
        Console.WriteLine($@"测试{notification.Name}");
    }
}