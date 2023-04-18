using Compass.Wasm.Shared.Data.Hoods;
using Compass.Wpf.ApiService;
using Compass.Wpf.ApiServices.Projects;

namespace Compass.Wpf.ApiServices.Data.Hoods;

public interface IKwiDataService : IBaseDataService<KwiData>
{
}

public class KwiDataService : BaseDataService<KwiData>, IKwiDataService
{
    public KwiDataService(HttpRestClient client, IModuleService moduleService) : base(client, "KwiData", moduleService)
    {
    }
}