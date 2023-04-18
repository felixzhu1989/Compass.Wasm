using Compass.Wasm.Shared;
using Compass.Wasm.Shared.Projects;

namespace Compass.Wasm.Server.Services.Projects;

public interface IModuleService : IBaseService<ModuleDto>
{
    //扩展查询
    Task<ApiResponse<List<ModuleDto>>> GetAllByDrawingIdAsync(Guid drawingId);

}