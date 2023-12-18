using System.Collections.Generic;
using Compass.Wasm.Shared;
using Compass.Wasm.Shared.Categories;
using Compass.Wasm.Shared.Params;

namespace Compass.Wpf.ApiServices.Projects;
public interface IProjectService : IBaseService<ProjectDto>
{
    Task<ApiResponse<List<ProjectDto>>> GetAllFilterAsync(ProjectParam param);
    
    Task<ApiResponse<List<DrawingDto>>> GetModuleTreeAsync(ProjectParam param);
    Task<ApiResponse<List<ModuleDto>>> GetModuleListAsync(ProjectParam param);

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


    public async Task<ApiResponse<List<ProjectDto>>> GetAllFilterAsync(ProjectParam param)
    {
        var request = new BaseRequest
        {
            Method = RestSharp.Method.Get,
            Route = "api/Project/Filter",
            Param = param
        };
        return await _client.ExecuteAsync<List<ProjectDto>>(request);
    }

    

    public async Task<ApiResponse<List<DrawingDto>>> GetModuleTreeAsync(ProjectParam param)
    {
        var request = new BaseRequest
        {
            Method = RestSharp.Method.Get,
            Route = "api/Project/ModuleTree",
            Param = param
        };
        return await _client.ExecuteAsync<List<DrawingDto>>(request);
    }

    public async Task<ApiResponse<List<ModuleDto>>> GetModuleListAsync(ProjectParam param)
    {
        var request = new BaseRequest
        {
            Method = RestSharp.Method.Get,
            Route = "api/Project/ModuleList",
            Param = param
        };
        return await _client.ExecuteAsync<List<ModuleDto>>(request);
    }


    public async Task<ApiResponse<List<ProductDto>>> GetModelTypeTreeAsync()
    {
        var request = new BaseRequest
        {
            Method = RestSharp.Method.Get,
            Route = "api/Product/ModelTypeTree"
        };
        return await _client.ExecuteAsync<List<ProductDto>>(request);
    }
}