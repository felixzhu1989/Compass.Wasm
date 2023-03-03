using Compass.DataService.Infrastructure;
using Compass.Wasm.Shared.ProjectService.Notification;

namespace Compass.Wasm.Server.ProjectService.NotificationHandler;

public class ModuleDeleteNotificationHandler: INotificationHandler<ModuleDeleteNotification>
{
    private readonly DataDbContext _dataDbContext;
    public ModuleDeleteNotificationHandler(DataDbContext dataDbContext)
    {
        _dataDbContext = dataDbContext;
    }

    public async Task Handle(ModuleDeleteNotification notification, CancellationToken cancellationToken)
    {
        var result =await _dataDbContext.ModulesData.SingleOrDefaultAsync(x => x.Id.Equals(notification.Id), cancellationToken: cancellationToken);
        if (result != null)
        {
            result.SoftDelete();
            await _dataDbContext.SaveChangesAsync(cancellationToken);
        }
    }
}