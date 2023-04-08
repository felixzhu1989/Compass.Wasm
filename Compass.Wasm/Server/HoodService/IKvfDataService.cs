using Compass.Wasm.Shared.DataService.Hoods;

namespace Compass.Wasm.Server.HoodService;

public interface IKvfDataService : IBaseDataGetService<KvfData>, IBaseDataUpdateService<KvfData>
{
}