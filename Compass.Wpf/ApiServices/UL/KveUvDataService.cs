using Compass.Wasm.Shared.Data.UL;
using Compass.Wpf.ApiService;
using Compass.Wpf.ApiServices.Projects;

namespace Compass.Wpf.ApiServices.UL;

public interface IKveUvDataService : IBaseDataService<KveUvData>
{
}

public class KveUvDataService : BaseDataService<KveUvData>, IKveUvDataService
{
    public KveUvDataService(HttpRestClient client, IModuleService moduleService) : base(client, "KveUvData", moduleService)
    {
    }
}