using Compass.Wasm.Shared;
using Compass.Wasm.Shared.ProjectService;
using System.Collections.Generic;
using System.Threading.Tasks;
using Compass.Wasm.Shared.CategoryService;
using Compass.Wasm.Shared.Parameter;

namespace Compass.Wpf.Service;

public interface IProjectService : IBaseService<ProjectDto>
{
    Task<ApiResponse<List<ProjectDto>>> GetAllFilterAsync(ProjectParameter parameter);
    Task<ApiResponse<ProjectSummaryDto>> GetSummaryAsync();
    Task<ApiResponse<List<DrawingDto>>> GetModuleTreeAsync(ProjectParameter parameter);
    Task<ApiResponse<List<ModuleDto>>> GetModuleListAsync(ProjectParameter parameter);

    //获取产品模型树
    Task<ApiResponse<List<ProductDto>>> GetModelTypeTreeAsync();

}