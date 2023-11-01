using Compass.Wasm.Shared.Data.Hoods;

namespace Compass.Wpf.ApiServices.Hoods;

public interface IUvimDataService : IBaseDataService<UvimData>
{
}
public class UvimDataService:BaseDataService<UvimData>,IUvimDataService
{
    public UvimDataService(HttpRestClient client, IModuleService moduleService) : base(client, "UvimData", moduleService)
    {
    }
}