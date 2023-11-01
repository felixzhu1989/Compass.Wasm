namespace Compass.Wpf.ApiServices.Ceilings;
public interface ICjDataService : IBaseDataService<CjData>
{

}
public class CjDataService : BaseDataService<CjData>, ICjDataService
{
    public CjDataService(HttpRestClient client, IModuleService moduleService) : base(client, "CjData", moduleService)
    {
    }
}