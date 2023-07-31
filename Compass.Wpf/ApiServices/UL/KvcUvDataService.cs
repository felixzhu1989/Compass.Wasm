using Compass.Wasm.Shared.Data.UL;
using Compass.Wpf.ApiService;
using Compass.Wpf.ApiServices.Projects;

namespace Compass.Wpf.ApiServices.UL;

public interface IKvcUvDataService : IBaseDataService<KvcUvData>
{
}

public class KvcUvDataService : BaseDataService<KvcUvData>, IKvcUvDataService
{
    public KvcUvDataService(HttpRestClient client, IModuleService moduleService) : base(client, "KvcUvData", moduleService)
    {
    }
}