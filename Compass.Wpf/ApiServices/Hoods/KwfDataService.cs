using Compass.Wasm.Shared.Data.Hoods;
using Compass.Wpf.ApiService;
using Compass.Wpf.ApiServices.Projects;

namespace Compass.Wpf.ApiServices.Hoods;

public interface IKwfDataService : IBaseDataService<KwfData>
{
}

public class KwfDataService : BaseDataService<KwfData>, IKwfDataService
{
    public KwfDataService(HttpRestClient client, IModuleService moduleService) : base(client, "KwfData", moduleService)
    {
    }
}