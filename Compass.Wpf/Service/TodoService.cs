using System.Collections.Generic;
using System.Threading.Tasks;
using Compass.Wasm.Shared;
using Compass.Wasm.Shared.Parameter;
using Compass.Wasm.Shared.TodoService;

namespace Compass.Wpf.Service;

public class TodoService : BaseService<TodoDto>, ITodoService
{
    private readonly HttpRestClient _client;

    public TodoService(HttpRestClient client) : base(client, "Todo")
    {
        _client = client;
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