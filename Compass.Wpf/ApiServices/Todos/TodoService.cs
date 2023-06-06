﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Compass.Wasm.Shared;
using Compass.Wasm.Shared.Parameters;
using Compass.Wpf.ApiService;

namespace Compass.Wpf.ApiServices.Todos;

public interface ITodoService : IBaseService<TodoDto>
{
    //扩展的请求
    Task<ApiResponse<TodoDto>> UserAddAsync(TodoDto dto);
    Task<ApiResponse<List<TodoDto>>> GetAllFilterAsync(TodoParameter parameter);
    Task<ApiResponse<TodoSummaryDto>> GetSummaryAsync();
}

public class TodoService : BaseService<TodoDto>, ITodoService
{
    private readonly HttpRestClient _client;

    public TodoService(HttpRestClient client) : base(client, "Todo")
    {
        _client = client;
    }

    public async Task<ApiResponse<TodoDto>> UserAddAsync(TodoDto dto)
    {
        BaseRequest request = new BaseRequest
        {
            Method = RestSharp.Method.Post,
            Route = "api/Todo/User/Add",
            Parameter = dto
        };
        return await _client.ExecuteAsync<TodoDto>(request);
    }

    public async Task<ApiResponse<List<TodoDto>>> GetAllFilterAsync(TodoParameter parameter)
    {
        BaseRequest request = new()
        {
            Method = RestSharp.Method.Get,
            Route = "api/Todo/Filter",
            Parameter = parameter
        };
        return await _client.ExecuteAsync<List<TodoDto>>(request);
    }

    public async Task<ApiResponse<TodoSummaryDto>> GetSummaryAsync()
    {
        BaseRequest request = new()
        {
            Method = RestSharp.Method.Get,
            Route = "api/Todo/Summary"
        };
        return await _client.ExecuteAsync<TodoSummaryDto>(request);
    }
}