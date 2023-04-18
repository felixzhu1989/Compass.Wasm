using Compass.Wasm.Shared.Data.Hoods;
using Compass.Wpf.ApiService;
using Compass.Wpf.ApiServices.Projects;

namespace Compass.Wpf.ApiServices.Data.Hoods;

public interface IUvfDataService : IBaseDataService<UvfData>
{
}

public class UvfDataService : BaseDataService<UvfData>, IUvfDataService
{
    public UvfDataService(HttpRestClient client, IModuleService moduleService) : base(client, "UvfData", moduleService)
    {
    }
}