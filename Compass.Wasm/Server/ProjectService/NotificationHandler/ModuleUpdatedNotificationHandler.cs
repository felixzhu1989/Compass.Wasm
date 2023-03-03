using Compass.DataService.Domain;
using Compass.DataService.Infrastructure;
using Compass.Wasm.Shared.DataService;
using Compass.Wasm.Shared.ProjectService.Notification;

namespace Compass.Wasm.Server.ProjectService.NotificationHandler;

public class ModuleUpdatedNotificationHandler : INotificationHandler<ModuleUpdatedNotification>
{
    private readonly DataDbContext _dataDbContext;
    private readonly ICategoryRepository _categoryRepository;
    public ModuleUpdatedNotificationHandler(DataDbContext dataDbContext,ICategoryRepository categoryRepository)
    {
        _dataDbContext = dataDbContext;
        _categoryRepository = categoryRepository;
    }
    private async Task AddModuleDataAsync(ModuleUpdatedNotification? notification)
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

    private async Task ChangeDimensionsAsync(ModuleUpdatedNotification notification, ModuleData moduleData)
    {
        //查询标准长宽高尺寸
        var modelType =await _categoryRepository.GetModelTypeByIdAsync(notification.ModelTypeId);
        moduleData.Length = notification.Length == 0 ? modelType.Length : notification.Length;
        moduleData.Width = notification.Width == 0 ? modelType.Width : notification.Width;
        moduleData.Height = notification.Height == 0 ? modelType.Height : notification.Height;
    }

    public async Task Handle(ModuleUpdatedNotification notification, CancellationToken cancellationToken)
    {
        var result =await _dataDbContext.ModulesData.SingleOrDefaultAsync(x => x.Id.Equals(notification.Id), cancellationToken: cancellationToken);
        if (result == null)
        {
            //参数不存在就增加
            await  AddModuleDataAsync(notification);
        }
        else
        {
            //如果Id存在，则修改参数
            await ChangeDimensionsAsync(notification, result);
        }
        await _dataDbContext.SaveChangesAsync(cancellationToken);
    }
}