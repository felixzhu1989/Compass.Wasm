using Compass.Wasm.Shared.Data.Hoods;

namespace Compass.Wpf.ApiServices.Hoods;

public interface ICmodiDataService : IBaseDataService<CmodiData>
{
}

public class CmodiDataService : BaseDataService<CmodiData>, ICmodiDataService
{
    public CmodiDataService(HttpRestClient client, IModuleService moduleService) : base(client, "CmodiData", moduleService)
    {
    }
}