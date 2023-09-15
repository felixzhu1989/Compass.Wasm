using Compass.Wasm.Shared.Data.Ceilings;
using Compass.Wpf.ApiService;
using Compass.Wpf.ApiServices.Projects;

namespace Compass.Wpf.ApiServices.Ceilings;

public interface IKcjDataService:IBaseDataService<KcjData>
{

}
public class KcjDataService:BaseDataService<KcjData>,IKcjDataService
{
    public KcjDataService(HttpRestClient client, IModuleService moduleService) : base(client, "KcjData", moduleService)
    {
    }
}