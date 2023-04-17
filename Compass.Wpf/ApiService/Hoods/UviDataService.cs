using Compass.Wasm.Shared.DataService.Hoods;

namespace Compass.Wpf.ApiService.Hoods;

public class UviDataService : BaseDataService<UviData>, IUviDataService
{
    public UviDataService(HttpRestClient client, IModuleService moduleService) : base(client, "UviData", moduleService)
    {
    }
}