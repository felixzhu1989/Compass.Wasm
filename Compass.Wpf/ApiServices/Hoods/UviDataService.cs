using Compass.Wasm.Shared.Data.Hoods;

namespace Compass.Wpf.ApiServices.Hoods;

public interface IUviDataService : IBaseDataService<UviData>
{
}

public class UviDataService : BaseDataService<UviData>, IUviDataService
{
    public UviDataService(HttpRestClient client, IModuleService moduleService) : base(client, "UviData", moduleService)
    {
    }
}