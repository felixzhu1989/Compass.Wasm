using Compass.Wasm.Shared.Data.Hoods;
using Compass.Wpf.ApiService;
using Compass.Wpf.ApiServices.Projects;

namespace Compass.Wpf.ApiServices.Hoods;

public interface ICmodiDataService : IBaseDataService<CmodiData>
{
}

public class CmodiDataService : BaseDataService<CmodiData>, ICmodiDataService
{
    public CmodiDataService(HttpRestClient client, IModuleService moduleService) : base(client, "CmodiData", moduleService)
    {
    }
}