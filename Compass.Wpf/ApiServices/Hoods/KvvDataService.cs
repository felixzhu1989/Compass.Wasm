using Compass.Wasm.Shared.Data.Hoods;
using Compass.Wpf.ApiService;
using Compass.Wpf.ApiServices.Projects;

namespace Compass.Wpf.ApiServices.Hoods;

public interface IKvvDataService:IBaseDataService<KvvData>
{

}

public class KvvDataService:BaseDataService<KvvData>,IKvvDataService
{
    public KvvDataService(HttpRestClient client, IModuleService moduleService) : base(client, "KvvData", moduleService)
    {
    }
}