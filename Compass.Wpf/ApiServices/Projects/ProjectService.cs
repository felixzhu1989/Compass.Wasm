﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Compass.Wasm.Shared;
using Compass.Wasm.Shared.Categories;
using Compass.Wasm.Shared.Parameters;
using Compass.Wasm.Shared.Projects;
using Compass.Wpf.ApiService;

namespace Compass.Wpf.ApiServices.Projects;

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

    public async Task<ApiResponse<ProjectSummaryDto>> GetSummaryAsync()
    {
        BaseRequest request = new()
        {
            Method = RestSharp.Method.Get,
            Route = "api/Project/Summary"
        };
        return await _client.ExecuteAsync<ProjectSummaryDto>(request);
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