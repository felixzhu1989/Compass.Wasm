using Compass.Wasm.Shared.Data.Hoods;

namespace Compass.Wpf.ApiServices.Hoods;

public interface IUwfDataService : IBaseDataService<UwfData>
{
}

public class UwfDataService : BaseDataService<UwfData>, IUwfDataService
{
    public UwfDataService(HttpRestClient client, IModuleService moduleService) : base(client, "UwfData", moduleService)
    {
    }
}