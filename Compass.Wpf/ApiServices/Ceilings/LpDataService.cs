namespace Compass.Wpf.ApiServices.Ceilings;
public interface ILpDataService : IBaseDataService<LpData>
{

}


public class LpDataService : BaseDataService<LpData>, ILpDataService
{
    public LpDataService(HttpRestClient client, IModuleService moduleService) : base(client, "LpData", moduleService)
    {
    }
}