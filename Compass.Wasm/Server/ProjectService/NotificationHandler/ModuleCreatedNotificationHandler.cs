using Compass.DataService.Domain;
using Compass.DataService.Infrastructure;
using Compass.Wasm.Shared.ProjectService.Notification;

namespace Compass.Wasm.Server.ProjectService.NotificationHandler;

//处理ModuleController发出的领域事件,创建分段后接着创建分段的参数
public class ModuleCreatedNotificationHandler : INotificationHandler<ModuleCreatedNotification>
{
    private readonly DataDbContext _dataDbContext;
    private readonly ICategoryRepository _categoryRepository;

    public ModuleCreatedNotificationHandler(DataDbContext dataDbContext, ICategoryRepository categoryRepository)
    {
        _dataDbContext = dataDbContext;
        _categoryRepository = categoryRepository;
    }

    public async Task Handle(ModuleCreatedNotification notification, CancellationToken cancellationToken)
    {
        //添加Module的同时添加ModuleData
        var moduleData = ModuleDataFactory.GetModuleData(notification.ModelName);
        //添加Module时添加长宽高
        if (moduleData != null)
        {
            moduleData.ChangeId(notification.Id);
            //查询标准长宽高尺寸
            var modelType = await _categoryRepository.GetModelTypeByIdAsync(notification.ModelTypeId);
            moduleData.Length = notification.Length == 0 ? modelType.Length : notification.Length;
            moduleData.Width = notification.Width == 0 ? modelType.Width : notification.Width;
            moduleData.Height = notification.Height == 0 ? modelType.Height : notification.Height;
            moduleData.SidePanel=notification.SidePanel;
           await _dataDbContext.ModulesData.AddAsync(moduleData, cancellationToken);
           await _dataDbContext.SaveChangesAsync(cancellationToken);
        }
    }
}