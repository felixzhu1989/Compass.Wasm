using Compass.Wasm.Shared;
using Compass.Wasm.Shared.Params;

namespace Compass.Wpf.ApiServices.Plans;

public interface IPackingListService:IBaseService<PackingListDto>
{
    Task<ApiResponse<PackingListDto>> GetSingleByProjectIdAndBathAsync(PackingListParam param);
    Task<ApiResponse<PackingListDto>> GetPackingInfoAsync(PackingListParam param);
    Task<ApiResponse<PackingListDto>> AddByProjectIdAndBathAsync(PackingListDto dto);
}

public class PackingListService:BaseService<PackingListDto>, IPackingListService
{
    #region ctor
    private readonly HttpRestClient _client;
    public PackingListService(HttpRestClient client) : base(client, "PackingList")
    {
        _client = client;
    }
    #endregion

    #region 扩展
    public async Task<ApiResponse<PackingListDto>> GetSingleByProjectIdAndBathAsync(PackingListParam param)
    {
        var request = new BaseRequest
        {
            Method = RestSharp.Method.Get,
            Route = "api/PackingList/ProjectIdAndBatch",
            Param = param
        };
        return await _client.ExecuteAsync<PackingListDto>(request);
    }

    public async Task<ApiResponse<PackingListDto>> GetPackingInfoAsync(PackingListParam param)
    {
        var request = new BaseRequest
        {
            Method = RestSharp.Method.Get,
            Route = "api/PackingList/PackingInfo",
            Param = param
        };
        return await _client.ExecuteAsync<PackingListDto>(request);
    }

    public async Task<ApiResponse<PackingListDto>> AddByProjectIdAndBathAsync(PackingListDto dto)
    {
        var request = new BaseRequest
        {
            Method = RestSharp.Method.Post,
            Route = "api/PackingList/Add/ProjectIdAndBatch",
            Param = dto
        };
        return await _client.ExecuteAsync<PackingListDto>(request);
    }
    #endregion
}