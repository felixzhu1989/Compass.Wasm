using Compass.Wasm.Shared.Data.Ceilings;
using Compass.Wpf.ApiService;
using Compass.Wpf.ApiServices.Projects;

namespace Compass.Wpf.ApiServices.Ceilings;

public interface IUcwDataService : IBaseDataService<UcwData>
{

}
public class UcwDataService : BaseDataService<UcwData>, IUcwDataService
{
    public UcwDataService(HttpRestClient client, IModuleService moduleService) : base(client, "UcwData", moduleService)
    {
    }
}
