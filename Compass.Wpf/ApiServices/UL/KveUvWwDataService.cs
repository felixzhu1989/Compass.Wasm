using Compass.Wasm.Shared.Data.UL;

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