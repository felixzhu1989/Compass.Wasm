using Compass.Wasm.Shared.Data.UL;

namespace Compass.Wpf.ApiServices.UL;

public interface IKveWwDataService : IBaseDataService<KveWwData>
{
}

public class KveWwDataService : BaseDataService<KveWwData>, IKveWwDataService
{
    public KveWwDataService(HttpRestClient client, IModuleService moduleService) : base(client, "KveWwData", moduleService)
    {
    }
}