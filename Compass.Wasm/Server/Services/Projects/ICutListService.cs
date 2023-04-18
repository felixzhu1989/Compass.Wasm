using Compass.Wasm.Shared;
using Compass.Wasm.Shared.Parameters;
using Compass.Wasm.Shared.Projects;

namespace Compass.Wasm.Server.Services.Projects;

public interface ICutListService : IBaseService<CutListDto>
{
    //扩展查询
    Task<ApiResponse<List<CutListDto>>> GetAllByModuleIdAsync(CutListParameter parameter);
}