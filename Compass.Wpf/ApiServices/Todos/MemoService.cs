using System.Collections.Generic;
using System.Threading.Tasks;
using Compass.Dtos;
using Compass.Wasm.Shared;
using Compass.Wasm.Shared.Params;
using Compass.Wpf.ApiService;

namespace Compass.Wpf.ApiServices.Todos;

public interface IMemoService : IBaseService<MemoDto>
{
    //扩展的请求
    Task<ApiResponse<MemoDto>> UserAddAsync(MemoDto dto);
    Task<ApiResponse<List<MemoDto>>> GetAllFilterAsync(QueryParam param);
}

public class MemoService : BaseService<MemoDto>, IMemoService
{
    private readonly HttpRestClient _client;

    public MemoService(HttpRestClient client) : base(client, "Memo")
    {
        _client = client;
    }
    public async Task<ApiResponse<MemoDto>> UserAddAsync(MemoDto dto)
    {
        var request = new BaseRequest
        {
            Method = RestSharp.Method.Post,
            Route = "api/Memo/User/Add",
            Param = dto
        };
        return await _client.ExecuteAsync<MemoDto>(request);
    }

    public async Task<ApiResponse<List<MemoDto>>> GetAllFilterAsync(QueryParam param)
    {
        var request = new BaseRequest
        {
            Method = RestSharp.Method.Get,
            Route = "api/Memo/Filter",
            Param = param
        };
        return await _client.ExecuteAsync<List<MemoDto>>(request);
    }
}