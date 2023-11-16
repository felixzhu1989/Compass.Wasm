namespace Compass.Wpf.ApiServices.Ceilings;

public interface ISspDataService : IBaseDataService<SspData>
{

}


public class SspDataService:BaseDataService<SspData>,ISspDataService
{
    public SspDataService(HttpRestClient client, IModuleService moduleService) : base(client, "SspData", moduleService)
    {
    }
}