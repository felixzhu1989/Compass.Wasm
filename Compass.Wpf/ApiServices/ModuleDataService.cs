using Compass.Wasm.Shared.Data;
using Compass.Wpf.ApiService;

namespace Compass.Wpf.ApiServices;

public class ModuleDataService : BaseService<ModuleData>, IModuleDataService
{
    public ModuleDataService(HttpRestClient client) : base(client, "ModuleData")
    {
    }
}