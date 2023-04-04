using Compass.Wasm.Shared.ProjectService;

namespace Compass.Wpf.ApiService;

public class ModuleService: BaseService<ModuleDto>, IModuleService
{
    public ModuleService(HttpRestClient client) : base(client, "Module")
    {

    }
}