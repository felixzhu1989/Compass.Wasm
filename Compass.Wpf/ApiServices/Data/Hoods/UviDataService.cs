using Compass.Wasm.Shared.Data.Hoods;
using Compass.Wpf.ApiService;
using Compass.Wpf.ApiServices.Projects;

namespace Compass.Wpf.ApiServices.Data.Hoods;

public interface IUviDataService : IBaseDataService<UviData>
{
}

public class UviDataService : BaseDataService<UviData>, IUviDataService
{
    public UviDataService(HttpRestClient client, IModuleService moduleService) : base(client, "UviData", moduleService)
    {
    }
}