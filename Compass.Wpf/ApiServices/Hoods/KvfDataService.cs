using Compass.Wasm.Shared.Data.Hoods;
using Compass.Wpf.ApiService;
using Compass.Wpf.ApiServices.Projects;

namespace Compass.Wpf.ApiServices.Hoods;

public interface IKvfDataService : IBaseDataService<KvfData>
{
}

public class KvfDataService : BaseDataService<KvfData>, IKvfDataService
{
    public KvfDataService(HttpRestClient client, IModuleService moduleService) : base(client, "KvfData", moduleService)
    {
    }
}