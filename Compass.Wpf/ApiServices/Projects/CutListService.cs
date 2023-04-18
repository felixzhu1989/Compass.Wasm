﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Compass.Wasm.Shared;
using Compass.Wasm.Shared.Parameters;
using Compass.Wasm.Shared.Projects;
using Compass.Wpf.ApiService;

namespace Compass.Wpf.ApiServices.Projects;

public class CutListService : BaseService<CutListDto>, ICutListService
{
    private readonly HttpRestClient _client;
    public CutListService(HttpRestClient client) : base(client, "CutList")
    {
        _client = client;
    }


    public async Task<ApiResponse<List<CutListDto>>> GetAllByModuleIdAsync(CutListParameter parameter)
    {
        BaseRequest request = new()
        {
            Method = RestSharp.Method.Get,
            Route = "api/CutList/Module",
            Parameter = parameter
        };
        return await _client.ExecuteAsync<List<CutListDto>>(request);
    }
}