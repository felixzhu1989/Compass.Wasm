using Compass.DataService.Domain;
using Compass.DataService.Infrastructure;
using Compass.Wasm.Shared.Data;
using Compass.Wasm.Shared.Projects.Notifs;

namespace Compass.Wasm.Server.Events.NotifHandlers;

public class ModuleUpdatedNotifHandler : INotificationHandler<ModuleUpdatedNotif>
{
    private readonly DataDbContext _dataDbContext;
    private readonly ICategoryRepository _categoryRepository;
    public ModuleUpdatedNotifHandler(DataDbContext dataDbContext, ICategoryRepository categoryRepository)
    {
        _dataDbContext = dataDbContext;
        _categoryRepository = categoryRepository;
    }
    private async Task AddModuleDataAsync(ModuleUpdatedNotif? notification)
    {
        //如果Id不存在则添加
        //添加Module的同时添加ModuleData
        var moduleData = ModuleDataFactory.GetModuleData(notification.ModelName);
        if (moduleData != null)
        {
            //添加Module时添加长宽高
            moduleData.ChangeId(notification.Id);
            await ChangeDimensionsAsync(notification, moduleData);
            await _dataDbContext.ModulesData.AddAsync(moduleData);
        }
    }

    private async Task ChangeDimensionsAsync(ModuleUpdatedNotif notification, ModuleData moduleData)
    {
        //查询标准长宽高尺寸
        var modelType = await _categoryRepository.GetModelTypeByIdAsync(notification.ModelTypeId);
        moduleData.Length = notification.Length == 0 ? modelType.Length : notification.Length;
        moduleData.Width = notification.Width == 0 ? modelType.Width : notification.Width;
        moduleData.Height = notification.Height == 0 ? modelType.Height : notification.Height;
        moduleData.SidePanel = notification.SidePanel;
    }

    public async Task Handle(ModuleUpdatedNotif notification, CancellationToken cancellationToken)
    {
        var result = await _dataDbContext.ModulesData.SingleOrDefaultAsync(x => x.Id.Equals(notification.Id), cancellationToken: cancellationToken);
        if (result == null)
        {
            //参数不存在就增加
            await AddModuleDataAsync(notification);
        }
        else
        {
            //如果Id存在，则修改参数
            await ChangeDimensionsAsync(notification, result);
        }
        await _dataDbContext.SaveChangesAsync(cancellationToken);
    }
}