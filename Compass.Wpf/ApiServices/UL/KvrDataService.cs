using Compass.Wasm.Shared.Data.UL;
using Compass.Wpf.ApiService;
using Compass.Wpf.ApiServices.Projects;

namespace Compass.Wpf.ApiServices.UL;

public interface IKvrDataService : IBaseDataService<KvrData>
{
}

public class KvrDataService : BaseDataService<KvrData>, IKvrDataService
{
    public KvrDataService(HttpRestClient client, IModuleService moduleService) : base(client, "KvrData", moduleService)
    {
    }
}