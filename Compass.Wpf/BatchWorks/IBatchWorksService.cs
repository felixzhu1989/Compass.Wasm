using System.Collections.Generic;
using System.Threading.Tasks;
using Compass.Wasm.Shared;
using Compass.Wasm.Shared.ProjectService;

namespace Compass.Wpf.BatchWorks;
public interface IBatchWorksService
{
    //作图
    Task<ApiResponse<bool>>  BatchDrawingAsync(List<ModuleDto> moduleDtos);
    //todo:导出dxf

    //todo：JobCard


}