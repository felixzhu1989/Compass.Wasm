using System.Collections.Generic;
using System.Threading.Tasks;
using Compass.Wasm.Shared;
using Compass.Wasm.Shared.Params;
using Compass.Wpf.ApiService;

namespace Compass.Wpf.ApiServices.Projects;
public interface ICutListService : IBaseService<CutListDto>
{
    //扩展查询
    Task<ApiResponse<List<CutListDto>>> GetAllByModuleIdAsync(CutListParam param);
}
public class CutListService : BaseService<CutListDto>, ICutListService
{
    private readonly HttpRestClient _client;
    public CutListService(HttpRestClient client) : base(client, "CutList")
    {
        _client = client;
    }


    public async Task<ApiResponse<List<CutListDto>>> GetAllByModuleIdAsync(CutListParam param)
    {
        var request = new BaseRequest
        {
            Method = RestSharp.Method.Get,
            Route = "api/CutList/Module",
            Param = param
        };
        return await _client.ExecuteAsync<List<CutListDto>>(request);
    }
}