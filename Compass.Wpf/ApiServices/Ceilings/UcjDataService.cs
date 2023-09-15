using Compass.Wasm.Shared.Data.Ceilings;
using Compass.Wpf.ApiService;
using Compass.Wpf.ApiServices.Projects;

namespace Compass.Wpf.ApiServices.Ceilings;

public interface IUcjDataService : IBaseDataService<UcjData>
{

}
public class UcjDataService:BaseDataService<UcjData>,IUcjDataService
{
    public UcjDataService(HttpRestClient client, IModuleService moduleService) : base(client, "UcjData", moduleService)
    {
    }
}