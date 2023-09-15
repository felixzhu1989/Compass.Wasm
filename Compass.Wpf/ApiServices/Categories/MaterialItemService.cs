using System.Collections.Generic;
using System.Threading.Tasks;
using Compass.Dtos;
using Compass.Wasm.Shared;
using Compass.Wasm.Shared.Categories;
using Compass.Wpf.ApiService;

namespace Compass.Wpf.ApiServices.Categories;

public interface IMaterialItemService : IBaseService<MaterialItemDto>
{
    Task<ApiResponse<MaterialItemDto>> UpdateInventoryAsync(Guid id, MaterialItemDto dto);
    Task<ApiResponse<MaterialItemDto>> UpdateOtherAsync(Guid id, MaterialItemDto dto);
    Task<ApiResponse<List<MaterialItemDto>>> GetTop50Async();

}

public class MaterialItemService:BaseService<MaterialItemDto>,IMaterialItemService
{
    #region ctor
    private readonly HttpRestClient _client;
    public MaterialItemService(HttpRestClient client) : base(client, "MaterialItem")
    {
        _client = client;
    }
    #endregion

    #region 扩展
    public async Task<ApiResponse<MaterialItemDto>> UpdateInventoryAsync(Guid id, MaterialItemDto dto)
    {
        var request = new BaseRequest
        {
            Method = RestSharp.Method.Put,
            Route = $"api/MaterialItem/UpdateInventory/{id}",
            Param = dto
        };
        return await _client.ExecuteAsync<MaterialItemDto>(request);
    }

    public async Task<ApiResponse<MaterialItemDto>> UpdateOtherAsync(Guid id, MaterialItemDto dto)
    {
        var request = new BaseRequest
        {
            Method = RestSharp.Method.Put,
            Route = $"api/MaterialItem/UpdateOther/{id}",
            Param = dto
        };
        return await _client.ExecuteAsync<MaterialItemDto>(request);
    }

    public async Task<ApiResponse<List<MaterialItemDto>>> GetTop50Async()
    {
        var request = new BaseRequest
        {
            Method = RestSharp.Method.Get,
            Route = "api/MaterialItem/Top50"
        };
        return await _client.ExecuteAsync<List<MaterialItemDto>>(request);
    } 
    #endregion
}