namespace Compass.Wpf.ApiServices.Ceilings;

public interface ILkDataService : IBaseDataService<LkData>
{

}

public class LkDataService:BaseDataService<LkData>,ILkDataService
{
    public LkDataService(HttpRestClient client, IModuleService moduleService) : base(client, "LkData", moduleService)
    {
    }
}