using Compass.Wasm.Shared.Parameter;
using Compass.Wasm.Shared;
using Compass.Wasm.Shared.TodoService;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Compass.Wpf.Service;

public class MemoService : BaseService<MemoDto>, IMemoService
{
    private readonly HttpRestClient _client;

    public MemoService(HttpRestClient client) : base(client, "Memo")
    {
        _client = client;
    }
    public async Task<ApiResponse<MemoDto>> UserAddAsync(MemoDto dto)
    {
        BaseRequest request = new BaseRequest
        {
            Method = RestSharp.Method.Post,
            Route = "api/Memo/User/Add",
            Parameter = dto
        };
        return await _client.ExecuteAsync<MemoDto>(request);
    }

    public async Task<ApiResponse<List<MemoDto>>> GetAllFilterAsync(QueryParameter parameter)
    {
        BaseRequest request = new()
        {
            Method = RestSharp.Method.Get,
            Route = "api/Memo/Filter",
            Parameter = parameter
        };
        return await _client.ExecuteAsync<List<MemoDto>>(request);
    }
}