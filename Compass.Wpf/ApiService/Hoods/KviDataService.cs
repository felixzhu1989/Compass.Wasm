using Compass.Wasm.Shared.DataService.Hoods;

namespace Compass.Wpf.ApiService.Hoods;

public class KviDataService : BaseDataService<KviData>,IKviDataService
{
    public KviDataService(HttpRestClient client,IModuleService moduleService) : base(client, "KviData", moduleService)
    {
    }
}