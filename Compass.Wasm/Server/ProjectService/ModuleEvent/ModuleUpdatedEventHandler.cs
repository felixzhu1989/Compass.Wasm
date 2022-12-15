using Compass.DataService.Domain;
using Compass.DataService.Infrastructure;
using Compass.ProjectService.Infrastructure;
using Compass.Wasm.Shared.DataService.Entities;

namespace Compass.Wasm.Server.ProjectService.ModuleEvent;

[EventName("ProjectService.Module.Updated")]
public class ModuleUpdatedEventHandler : JsonIntegrationEventHandler<ModuleUpdatedEvent>
{
    private readonly DataDbContext _dataDbContext;
    private readonly CategoryDbContext _categoryDbContext;
    private readonly ProjectDbContext _projectDbContext;

    public ModuleUpdatedEventHandler(DataDbContext dataDbContext, CategoryDbContext categoryDbContext,ProjectDbContext projectDbContext)
    {
        _dataDbContext = dataDbContext;
        _categoryDbContext = categoryDbContext;
        _projectDbContext = projectDbContext;
    }
    public override async Task HandleJson(string eventName, ModuleUpdatedEvent? eventData)
    {
        var result = await _dataDbContext.ModulesData.SingleOrDefaultAsync(x => x.Id.Equals(eventData.Id));
        var module = await _projectDbContext.Modules.SingleOrDefaultAsync(x => x.Id.Equals(eventData.Id));
        if (result == null)
        {
            module.ChangeIsModuleDataOk(await AddModuleData(eventData));
        }
        else
        {
            //如果Id存在，则看Model是否发生变化，如果没发生变化则直接修改参数即可,如果发生变化则删除再添加，
            if (eventData.ModelTypeId.Equals(eventData.OldModelTypeId))
            {
                await ChangeDimensions(eventData, result);
                module.ChangeIsModuleDataOk(true);
            }
            else
            {
                //删除再添加
                _dataDbContext.ModulesData.Remove(result);
                module.ChangeIsModuleDataOk(await AddModuleData(eventData));
            }
        }
        await _dataDbContext.SaveChangesAsync();
        await _projectDbContext.SaveChangesAsync();
    }

    private async Task<bool> AddModuleData(ModuleUpdatedEvent? eventData)
    {
        //如果Id不存在则添加
        //添加Module的同时添加ModuleData
        var moduleData = ModuleDataFactory.GetModuleData(eventData.ModelName);
        //添加Module时添加长宽高
        if (moduleData == null) return false;//添加失败了
        moduleData.ChangeId(eventData.Id);
        await ChangeDimensions(eventData, moduleData);
        await _dataDbContext.ModulesData.AddAsync(moduleData);
        return true;
    }

    private async Task ChangeDimensions(ModuleUpdatedEvent eventData, ModuleData moduleData)
    {
        //查询标准长宽高尺寸
        var modelType = await _categoryDbContext.ModelTypes.SingleOrDefaultAsync(x => x.Id.Equals(eventData.ModelTypeId));
        moduleData.Length = eventData.Length == 0 ? modelType.Length : eventData.Length;
        moduleData.Width = eventData.Width == 0 ? modelType.Width : eventData.Width;
        moduleData.Height = eventData.Height == 0 ? modelType.Height : eventData.Height;
    }
}