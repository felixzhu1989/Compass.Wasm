using Compass.Wasm.Shared.DataService.Hoods;

namespace Compass.Wpf.ApiService.Hoods;

internal class KvfDataService : BaseDataService<KvfData>, IKvfDataService
{
    public KvfDataService(HttpRestClient client, IModuleService moduleService) : base(client, "KvfData", moduleService)
    {
    }
}