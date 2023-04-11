using Compass.Wasm.Shared.Parameter;
using Compass.Wasm.Shared;
using Compass.Wasm.Shared.ProjectService;

namespace Compass.Wasm.Server.ProjectService;

public interface IProjectService : IBaseService<ProjectDto>
{
    //扩展的查询功能,WPF
    Task<ApiResponse<List<ProjectDto>>> GetAllFilterAsync(ProjectParameter parameter);
    Task<ApiResponse<ProjectSummaryDto>> GetSummaryAsync();
    Task<ApiResponse<List<DrawingDto>>> GetModuleTreeAsync(ProjectParameter parameter);//用于树结构
    Task<ApiResponse<List<ModuleDto>>> GetModuleListAsync(ProjectParameter parameter);//用于自动作图

    //扩展查询功能，Blazor
    Task<ApiResponse<List<ProjectDto>>> GetAllFilterAsync(string? search);
    //UploadFiles
    Task<ApiResponse<ProjectDto>> UploadFilesAsync(Guid id, ProjectDto dto);

}