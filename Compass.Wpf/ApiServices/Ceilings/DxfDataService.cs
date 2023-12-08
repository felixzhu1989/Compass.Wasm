namespace Compass.Wpf.ApiServices.Ceilings;

public interface IDxfDataService : IBaseDataService<DxfData>
{

}

public class DxfDataService:BaseDataService<DxfData>,IDxfDataService
{
    public DxfDataService(HttpRestClient client, IModuleService moduleService) : base(client, "DxfData", moduleService)
    {
    }
}