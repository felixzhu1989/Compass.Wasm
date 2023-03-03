using Compass.Wasm.Shared;
using Compass.Wasm.Shared.ProjectService;

namespace Compass.Wasm.Server.ProjectService;

public interface IDrawingService:IBaseService<DrawingDto>
{
    //扩展查询
    Task<ApiResponse<List<DrawingDto>>> GetAllByProjectIdAsync(Guid projectId);


}