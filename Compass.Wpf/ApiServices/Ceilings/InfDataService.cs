namespace Compass.Wpf.ApiServices.Ceilings;
public interface IInfDataService : IBaseDataService<InfData>
{

}

public class InfDataService : BaseDataService<InfData>, IInfDataService
{
    public InfDataService(HttpRestClient client, IModuleService moduleService) : base(client, "InfData", moduleService)
    {
    }
}