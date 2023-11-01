namespace Compass.Wpf.ApiServices.Ceilings;

public interface IKcwDataService : IBaseDataService<KcwData>
{

}
public class KcwDataService : BaseDataService<KcwData>, IKcwDataService
{
    public KcwDataService(HttpRestClient client, IModuleService moduleService) : base(client, "KcwData", moduleService)
    {
    }
}