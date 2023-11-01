using Compass.Wasm.Shared.Data.UL;

namespace Compass.Wpf.ApiServices.UL;

public interface IKvcUvWwDataService : IBaseDataService<KvcUvWwData>
{
}

public class KvcUvWwDataService : BaseDataService<KvcUvWwData>, IKvcUvWwDataService
{
    public KvcUvWwDataService(HttpRestClient client, IModuleService moduleService) : base(client, "KvcUvWwData", moduleService)
    {
    }
}