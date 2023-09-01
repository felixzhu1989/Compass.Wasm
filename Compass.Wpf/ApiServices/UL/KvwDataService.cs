using Compass.Wasm.Shared.Data.UL;
using Compass.Wpf.ApiService;
using Compass.Wpf.ApiServices.Projects;

namespace Compass.Wpf.ApiServices.UL;

public interface IKvwDataService : IBaseDataService<KvwData>
{
}

public class KvwDataService : BaseDataService<KvwData>, IKvwDataService
{
    public KvwDataService(HttpRestClient client, IModuleService moduleService) : base(client, "KvwData", moduleService)
    {
    }
}