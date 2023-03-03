using Compass.Wasm.Shared.DataService.Hoods;

namespace Compass.Wpf.Service.Hoods;

public class KviDataService : BaseDataService<KviData>,IKviDataService
{
    public KviDataService(HttpRestClient client) : base(client, "KviData")
    {
    }
}