using Compass.Wasm.Shared.DataService.Hoods;

namespace Compass.Wasm.Server.HoodService;

public interface IUvfDataService : IBaseDataGetService<UvfData>, IBaseDataUpdateService<UvfData>
{
}