using Compass.Dtos;
using Compass.Wasm.Shared.Categories;
using System.Collections.Generic;
using Compass.Wasm.Shared.Params;

namespace Compass.Wpf.ApiServices.Categories;

public interface IAccCutListService : IBaseService<AccCutListDto>
{
    Task<ApiResponse<List<AccCutListDto>>> GetAllByAccTypeAsync(AccCutListParam param);
}

public class AccCutListService:BaseService<AccCutListDto>,IAccCutListService
{
    #region ctor
    private readonly HttpRestClient _client;
    public AccCutListService(HttpRestClient client) : base(client, "AccCutList")
    {
        _client = client;
    } 
    #endregion


    public async Task<ApiResponse<List<AccCutListDto>>> GetAllByAccTypeAsync(AccCutListParam param)
    {
        var request = new BaseRequest
        {
            Method = RestSharp.Method.Get,
            Route = "api/AccCutList/AccType",
            Param = param
        };
        return await _client.ExecuteAsync<List<AccCutListDto>>(request);
    }
}