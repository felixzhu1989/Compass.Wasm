namespace Compass.Wpf.ApiServices.Ceilings;
public interface ILfuDataService:IBaseDataService<LfuData>{}

public class LfuDataService:BaseDataService<LfuData>,ILfuDataService
{
    public LfuDataService(HttpRestClient client, IModuleService moduleService) : base(client, "LfuData", moduleService)
    {
    }
}