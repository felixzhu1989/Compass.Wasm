using Compass.Wasm.Shared.ProjectService;

namespace Compass.Wpf.Service;

public class ModuleService: BaseService<ModuleDto>, IModuleService
{
    public ModuleService(HttpRestClient client) : base(client, "Module")
    {
    }
}