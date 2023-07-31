using Compass.Wasm.Shared.Data.UL;
using Compass.Wpf.ApiService;
using Compass.Wpf.ApiServices.Projects;

namespace Compass.Wpf.ApiServices.UL;

public interface IKveUvWwDataService : IBaseDataService<KveUvWwData>
{
}

public class KveUvWwDataService : BaseDataService<KveUvWwData>, IKveUvWwDataService
{
    public KveUvWwDataService(HttpRestClient client, IModuleService moduleService) : base(client, "KveUvWwData", moduleService)
    {
    }
}