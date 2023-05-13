using Compass.Wpf.ApiService;

namespace Compass.Wpf.ApiServices.Projects;
public interface IModuleService : IBaseService<ModuleDto>
{

}
public class ModuleService : BaseService<ModuleDto>, IModuleService
{
    public ModuleService(HttpRestClient client) : base(client, "Module")
    {

    }
}