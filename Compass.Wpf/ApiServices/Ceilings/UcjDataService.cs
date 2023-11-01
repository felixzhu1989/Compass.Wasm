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