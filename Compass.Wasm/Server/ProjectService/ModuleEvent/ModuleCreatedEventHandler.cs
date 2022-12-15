using Compass.DataService.Domain;
using Compass.DataService.Infrastructure;

namespace Compass.Wasm.Server.ProjectService.ModuleEvent;

//处理ModuleController发出的集成事件，发出生产图后，将项目跟踪的项目状态修改成生产，记录发图时间
[EventName("ProjectService.Module.Created")]
public class ModuleCreatedEventHandler : JsonIntegrationEventHandler<ModuleCreatedEvent>
{
    private readonly DataDbContext _dataDbContext;
    private readonly CategoryDbContext _categoryDbContext;
    private readonly ProjectDbContext _projectDbContext;

    public ModuleCreatedEventHandler(DataDbContext dataDbContext, CategoryDbContext categoryDbContext,
        ProjectDbContext projectDbContext)
    {
        _dataDbContext = dataDbContext;
        _categoryDbContext = categoryDbContext;
        _projectDbContext = projectDbContext;
    }

    public override async Task HandleJson(string eventName, ModuleCreatedEvent? eventData)
    {
        //添加Module的同时添加ModuleData
        var moduleData = ModuleDataFactory.GetModuleData(eventData.ModelName);
        //添加Module时添加长宽高
        if (moduleData != null)
        {
            moduleData.ChangeId(eventData.Id);
            //查询标准长宽高尺寸
            var modelType =
                await _categoryDbContext.ModelTypes.SingleOrDefaultAsync(x => x.Id.Equals(eventData.ModelTypeId));
            moduleData.Length = eventData.Length == 0 ? modelType.Length : eventData.Length;
            moduleData.Width = eventData.Width == 0 ? modelType.Width : eventData.Width;
            moduleData.Height = eventData.Height == 0 ? modelType.Height : eventData.Height;
            await _dataDbContext.ModulesData.AddAsync(moduleData);
            await _dataDbContext.SaveChangesAsync();
            var module = await _projectDbContext.Modules.SingleOrDefaultAsync(x => x.Id.Equals(eventData.Id));
            module.ChangeIsModuleDataOk(true);
            await _projectDbContext.SaveChangesAsync();
        }
    }
}