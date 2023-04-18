using Compass.Wasm.Shared.Data.Hoods;
using Compass.Wpf.ApiService;
using Compass.Wpf.ApiServices.Projects;

namespace Compass.Wpf.ApiServices.Data.Hoods;

public interface IKviDataService : IBaseDataService<KviData>
{
}

public class KviDataService : BaseDataService<KviData>, IKviDataService
{
    public KviDataService(HttpRestClient client, IModuleService moduleService) : base(client, "KviData", moduleService)
    {
    }
}