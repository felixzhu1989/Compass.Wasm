using Compass.Wasm.Shared.Data.Hoods;
using Compass.Wpf.ApiService;
using Compass.Wpf.ApiServices.Projects;

namespace Compass.Wpf.ApiServices.Data.Hoods;

public interface IUwiDataService : IBaseDataService<UwiData>
{
}

public class UwiDataService : BaseDataService<UwiData>, IUwiDataService
{
    public UwiDataService(HttpRestClient client, IModuleService moduleService) : base(client, "UwiData", moduleService)
    {
    }
}