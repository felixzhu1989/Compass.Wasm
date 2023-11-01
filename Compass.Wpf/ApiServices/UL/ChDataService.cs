using Compass.Wasm.Shared.Data.UL;

namespace Compass.Wpf.ApiServices.UL;

public interface IChDataService : IBaseDataService<ChData>
{
}

public class ChDataService : BaseDataService<ChData>, IChDataService
{
    public ChDataService(HttpRestClient client, IModuleService moduleService) : base(client, "ChData", moduleService)
    {
    }
}