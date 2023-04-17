using Compass.Wasm.Shared.DataService.Hoods;

namespace Compass.Wpf.ApiService.Hoods;

public class UvfDataService : BaseDataService<UvfData>, IUvfDataService
{
    public UvfDataService(HttpRestClient client, IModuleService moduleService) : base(client, "UvfData", moduleService)
    {
    }
}