using Compass.Wasm.Shared.Data.Hoods;

namespace Compass.Wpf.ApiServices.Hoods;

public interface ICmodmDataService : IBaseDataService<CmodmData>
{
}

public class CmodmDataService : BaseDataService<CmodmData>, ICmodmDataService
{
    public CmodmDataService(HttpRestClient client, IModuleService moduleService) : base(client, "CmodmData", moduleService)
    {
    }
}