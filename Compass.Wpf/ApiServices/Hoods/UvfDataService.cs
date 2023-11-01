using Compass.Wasm.Shared.Data.Hoods;

namespace Compass.Wpf.ApiServices.Hoods;

public interface IUvfDataService : IBaseDataService<UvfData>
{
}

public class UvfDataService : BaseDataService<UvfData>, IUvfDataService
{
    public UvfDataService(HttpRestClient client, IModuleService moduleService) : base(client, "UvfData", moduleService)
    {
    }
}