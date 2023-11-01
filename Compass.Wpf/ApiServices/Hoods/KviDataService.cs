using Compass.Wasm.Shared.Data.Hoods;

namespace Compass.Wpf.ApiServices.Hoods;

public interface IKviDataService : IBaseDataService<KviData>
{
}

public class KviDataService : BaseDataService<KviData>, IKviDataService
{
    public KviDataService(HttpRestClient client, IModuleService moduleService) : base(client, "KviData", moduleService)
    {
    }
}