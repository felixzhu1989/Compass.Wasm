namespace Compass.Wpf.ApiServices.Ceilings;
public interface IDpDataService : IBaseDataService<DpData>
{

}
public class DpDataService:BaseDataService<DpData>,IDpDataService
{
    public DpDataService(HttpRestClient client, IModuleService moduleService) : base(client, "DpData", moduleService)
    {
    }
}