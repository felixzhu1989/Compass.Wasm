using Compass.DataService.Infrastructure;
using Compass.Wasm.Shared.Projects.Notifs;

namespace Compass.Wasm.Server.Events.NotifHandlers;

//删除Module时同时删除ModuleData
public class ModuleDeleteNotifHandler : INotificationHandler<ModuleDeleteNotif>
{
    private readonly DataDbContext _dataDbContext;
    public ModuleDeleteNotifHandler(DataDbContext dataDbContext)
    {
        _dataDbContext = dataDbContext;
    }

    public async Task Handle(ModuleDeleteNotif notification, CancellationToken cancellationToken)
    {
        var result = await _dataDbContext.ModulesData.SingleOrDefaultAsync(x => x.Id.Equals(notification.Id), cancellationToken: cancellationToken);
        if (result != null)
        {
            result.SoftDelete();
            await _dataDbContext.SaveChangesAsync(cancellationToken);
        }
    }
}