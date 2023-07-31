using Compass.Wasm.Shared.Data.UL;
using Compass.Wpf.ApiService;
using Compass.Wpf.ApiServices.Projects;

namespace Compass.Wpf.ApiServices.UL;

public interface IKvcWwDataService : IBaseDataService<KvcWwData>
{
}

public class KvcWwDataService : BaseDataService<KvcWwData>, IKvcWwDataService
{
    public KvcWwDataService(HttpRestClient client, IModuleService moduleService) : base(client, "KvcWwData", moduleService)
    {
    }
}