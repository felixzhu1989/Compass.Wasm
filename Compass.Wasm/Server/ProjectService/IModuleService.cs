using Compass.Wasm.Shared;
using Compass.Wasm.Shared.ProjectService;

namespace Compass.Wasm.Server.ProjectService;

public interface IModuleService:IBaseService<ModuleDto>
{
    //扩展查询
    Task<ApiResponse<List<ModuleDto>>> GetAllByDrawingIdAsync(Guid drawingId);

}