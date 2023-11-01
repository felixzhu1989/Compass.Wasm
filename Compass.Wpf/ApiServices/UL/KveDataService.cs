using Compass.Wasm.Shared.Data.UL;

namespace Compass.Wpf.ApiServices.UL;

public interface IKveDataService : IBaseDataService<KveData>
{
}

public class KveDataService : BaseDataService<KveData>, IKveDataService
{
    public KveDataService(HttpRestClient client, IModuleService moduleService) : base(client, "KveData", moduleService)
    {
    }
}