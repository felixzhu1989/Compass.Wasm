using Compass.Wasm.Shared.Data.Hoods;

namespace Compass.Wpf.ApiServices.Hoods;

public interface IKwiDataService : IBaseDataService<KwiData>
{
}

public class KwiDataService : BaseDataService<KwiData>, IKwiDataService
{
    public KwiDataService(HttpRestClient client, IModuleService moduleService) : base(client, "KwiData", moduleService)
    {
    }
}