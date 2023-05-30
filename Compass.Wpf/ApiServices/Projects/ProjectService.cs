using System.Collections.Generic;
using System.Threading.Tasks;
using Compass.Wasm.Shared;
using Compass.Wasm.Shared.Categories;
using Compass.Wasm.Shared.Parameters;
using Compass.Wpf.ApiService;

namespace Compass.Wpf.ApiServices.Projects;
public interface IProjectService : IBaseService<ProjectDto>
{
    Task<ApiResponse<List<ProjectDto>>> GetAllFilterAsync(ProjectParameter parameter);
    
    Task<ApiResponse<List<DrawingDto>>> GetModuleTreeAsync(ProjectParameter parameter);
    Task<ApiResponse<List<ModuleDto>>> GetModuleListAsync(ProjectParameter parameter);

    //获取产品模型树
    Task<ApiResponse<List<ProductDto>>> GetModelTypeTreeAsync();

}

public class ProjectService : BaseService<ProjectDto>, IProjectService
{
    private readonly HttpRestClient _client;

    public ProjectService(HttpRestClient client) : base(client, "Project")
    {
        _client = client;
    }


    public async Task<ApiResponse<List<ProjectDto>>> GetAllFilterAsync(ProjectParameter parameter)
    {
        BaseRequest request = new()
        {
            Method = RestSharp.Method.Get,
            Route = "api/Project/Filter",
            Parameter = parameter
        };
        return await _client.ExecuteAsync<List<ProjectDto>>(request);
    }

    

    public async Task<ApiResponse<List<DrawingDto>>> GetModuleTreeAsync(ProjectParameter parameter)
    {
        BaseRequest request = new()
        {
            Method = RestSharp.Method.Get,
            Route = "api/Project/ModuleTree",
            Parameter = parameter
        };
        return await _client.ExecuteAsync<List<DrawingDto>>(request);
    }

    public async Task<ApiResponse<List<ModuleDto>>> GetModuleListAsync(ProjectParameter parameter)
    {
        BaseRequest request = new()
        {
            Method = RestSharp.Method.Get,
            Route = "api/Project/ModuleList",
            Parameter = parameter
        };
        return await _client.ExecuteAsync<List<ModuleDto>>(request);
    }


    public async Task<ApiResponse<List<ProductDto>>> GetModelTypeTreeAsync()
    {
        BaseRequest request = new()
        {
            Method = RestSharp.Method.Get,
            Route = "api/Product/ModelTypeTree"
        };
        return await _client.ExecuteAsync<List<ProductDto>>(request);
    }
}