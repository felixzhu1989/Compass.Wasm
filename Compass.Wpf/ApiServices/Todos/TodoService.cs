using System.Collections.Generic;
using Compass.Wasm.Shared;
using Compass.Wasm.Shared.Params;

namespace Compass.Wpf.ApiServices.Todos;

public interface ITodoService : IBaseService<TodoDto>
{
    //扩展的请求
    Task<ApiResponse<TodoDto>> UserAddAsync(TodoDto dto);
    Task<ApiResponse<List<TodoDto>>> GetAllFilterAsync(TodoParam param);
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
        var request = new BaseRequest
        {
            Method = RestSharp.Method.Post,
            Route = "api/Todo/User/Add",
            Param = dto
        };
        return await _client.ExecuteAsync<TodoDto>(request);
    }

    public async Task<ApiResponse<List<TodoDto>>> GetAllFilterAsync(TodoParam param)
    {
        var request = new BaseRequest
        {
            Method = RestSharp.Method.Get,
            Route = "api/Todo/Filter",
            Param = param
        };
        return await _client.ExecuteAsync<List<TodoDto>>(request);
    }

    public async Task<ApiResponse<TodoSummaryDto>> GetSummaryAsync()
    {
        var request = new BaseRequest
        {
            Method = RestSharp.Method.Get,
            Route = "api/Todo/Summary"
        };
        return await _client.ExecuteAsync<TodoSummaryDto>(request);
    }
}