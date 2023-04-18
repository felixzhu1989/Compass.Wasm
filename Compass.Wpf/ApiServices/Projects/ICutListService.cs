using System.Collections.Generic;
using System.Threading.Tasks;
using Compass.Wasm.Shared;
using Compass.Wasm.Shared.Parameters;
using Compass.Wasm.Shared.Projects;
using Compass.Wpf.ApiService;

namespace Compass.Wpf.ApiServices.Projects;

public interface ICutListService : IBaseService<CutListDto>
{
    //扩展查询
    Task<ApiResponse<List<CutListDto>>> GetAllByModuleIdAsync(CutListParameter parameter);
}