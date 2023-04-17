using Compass.Wasm.Shared.DataService.Hoods;

namespace Compass.Wasm.Server.HoodService;

public interface IUviDataService : IBaseDataGetService<UviData>, IBaseDataUpdateService<UviData>
{
}