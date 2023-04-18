using Compass.Wasm.Shared;
using Compass.Wasm.Shared.Projects;

namespace Compass.Wasm.Server.Services.Projects;

public interface IDrawingService : IBaseService<DrawingDto>
{
    //扩展查询
    Task<ApiResponse<List<DrawingDto>>> GetAllByProjectIdAsync(Guid projectId);


}