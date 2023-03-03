using Compass.Wasm.Shared.DataService;

namespace Compass.Wpf.Service;

public class ModuleDataService:BaseService<ModuleData>,IModuleDataService
{
    public ModuleDataService(HttpRestClient client) : base(client, "ModuleData")
    {
    }
}