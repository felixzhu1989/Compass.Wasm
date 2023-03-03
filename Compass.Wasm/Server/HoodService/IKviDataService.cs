using Compass.Wasm.Shared.DataService.Hoods;

namespace Compass.Wasm.Server.HoodService;

public interface IKviDataService:IBaseDataGetService<KviData>,IBaseDataUpdateService<KviData>
{
}