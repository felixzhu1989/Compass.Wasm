using Compass.Wasm.Shared.Data.Hoods;
using Compass.Wpf.ApiService;
using Compass.Wpf.ApiServices.Projects;

namespace Compass.Wpf.ApiServices.Hoods;

public interface ICmodfDataService : IBaseDataService<CmodfData>
{
}

public class CmodfDataService : BaseDataService<CmodfData>, ICmodfDataService
{
    public CmodfDataService(HttpRestClient client, IModuleService moduleService) : base(client, "CmodfData", moduleService)
    {
    }
}