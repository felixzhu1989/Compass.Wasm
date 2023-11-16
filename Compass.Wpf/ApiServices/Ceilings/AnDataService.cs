namespace Compass.Wpf.ApiServices.Ceilings;

public interface IAnDataService : IBaseDataService<AnData>
{

}

public class AnDataService:BaseDataService<AnData>,IAnDataService
{
    public AnDataService(HttpRestClient client, IModuleService moduleService) : base(client, "AnData", moduleService)
    {
    }
}