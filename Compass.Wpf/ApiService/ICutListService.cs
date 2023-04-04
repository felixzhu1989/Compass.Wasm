using System.Collections.Generic;
using System.Threading.Tasks;
using Compass.Wasm.Shared;
using Compass.Wasm.Shared.Parameter;
using Compass.Wasm.Shared.ProjectService;

namespace Compass.Wpf.ApiService;

public interface ICutListService : IBaseService<CutListDto>
{
    //扩展查询
    Task<ApiResponse<List<CutListDto>>> GetAllByModuleIdAsync(CutListParameter parameter);
}