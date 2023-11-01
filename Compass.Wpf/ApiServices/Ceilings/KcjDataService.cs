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