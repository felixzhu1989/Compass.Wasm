using Compass.Wasm.Shared.Data.Hoods;

namespace Compass.Wpf.ApiServices.Hoods;

public interface IUwiDataService : IBaseDataService<UwiData>
{
}

public class UwiDataService : BaseDataService<UwiData>, IUwiDataService
{
    public UwiDataService(HttpRestClient client, IModuleService moduleService) : base(client, "UwiData", moduleService)
    {
    }
}