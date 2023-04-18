using System.Collections.Generic;
using System.Threading.Tasks;
using Compass.Wasm.Shared;
using Compass.Wasm.Shared.Categories;
using Compass.Wasm.Shared.Parameters;
using Compass.Wasm.Shared.Projects;
using Compass.Wpf.ApiService;

namespace Compass.Wpf.ApiServices.Projects;

public interface IProjectService : IBaseService<ProjectDto>
{
    Task<ApiResponse<List<ProjectDto>>> GetAllFilterAsync(ProjectParameter parameter);
    Task<ApiResponse<ProjectSummaryDto>> GetSummaryAsync();
    Task<ApiResponse<List<DrawingDto>>> GetModuleTreeAsync(ProjectParameter parameter);
    Task<ApiResponse<List<ModuleDto>>> GetModuleListAsync(ProjectParameter parameter);

    //获取产品模型树
    Task<ApiResponse<List<ProductDto>>> GetModelTypeTreeAsync();

}