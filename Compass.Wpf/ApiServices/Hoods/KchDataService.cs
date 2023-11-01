using Compass.Wasm.Shared.Data.Hoods;

namespace Compass.Wpf.ApiServices.Hoods;

public interface IKchDataService : IBaseDataService<KchData>
{
}

public class KchDataService : BaseDataService<KchData>, IKchDataService
{
    public KchDataService(HttpRestClient client, IModuleService moduleService) : base(client, "KchData", moduleService)
    {
    }
}