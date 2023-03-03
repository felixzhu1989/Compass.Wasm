using Compass.Wasm.Shared.DataService;

namespace Compass.DataService.Domain;

public interface IDataRepository
{
    Task<List<ModuleData>> GetModulesDataAsync();
    Task<ModuleData?> GetModuleDataByIdAsync(Guid id);
}