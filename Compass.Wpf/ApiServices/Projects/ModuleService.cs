using Compass.Wasm.Shared.Projects;
using Compass.Wpf.ApiService;

namespace Compass.Wpf.ApiServices.Projects;

public class ModuleService : BaseService<ModuleDto>, IModuleService
{
    public ModuleService(HttpRestClient client) : base(client, "Module")
    {

    }
}