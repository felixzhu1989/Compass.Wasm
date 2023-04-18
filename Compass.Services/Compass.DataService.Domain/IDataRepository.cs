using Compass.Wasm.Shared.Data;

namespace Compass.DataService.Domain;

public interface IDataRepository
{
    Task<List<ModuleData>> GetModulesDataAsync();
    Task<ModuleData?> GetModuleDataByIdAsync(Guid id);
}