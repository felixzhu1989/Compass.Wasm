using Compass.Wasm.Shared;
using Compass.Wpf.ApiService;
using System.Threading.Tasks;

namespace Compass.Wpf.ApiServices.Plans;

public interface IPackingItemService : IBaseService<PackingItemDto>
{
    Task<ApiResponse<PackingItemDto>> AddPalletAsync(PackingItemDto dto);
    Task<ApiResponse<PackingItemDto>> UpdatePalletAsync(Guid id, PackingItemDto dto);
    Task<ApiResponse<PackingItemDto>> UpdatePalletNumberAsync(Guid id);
    
}

public class PackingItemService : BaseService<PackingItemDto>, IPackingItemService
{
    private readonly HttpRestClient _client;
    public PackingItemService(HttpRestClient client) : base(client, "PackingItem")
    {
        _client = client;
    }

    public async Task<ApiResponse<PackingItemDto>> AddPalletAsync(PackingItemDto dto)
    {
        var request = new BaseRequest
        {
            Method = RestSharp.Method.Post,
            Route = "api/PackingItem/AddPallet",
            Param = dto
        };
        return await _client.ExecuteAsync<PackingItemDto>(request);
    }
    public async Task<ApiResponse<PackingItemDto>> UpdatePalletAsync(Guid id, PackingItemDto dto)
    {
        var request = new BaseRequest
        {
            Method = RestSharp.Method.Put,
            Route = $"api/PackingItem/Pallet/{id}",
            Param = dto
        };
        return await _client.ExecuteAsync<PackingItemDto>(request);

    }
    public async Task<ApiResponse<PackingItemDto>> UpdatePalletNumberAsync(Guid id)
    {
        var request = new BaseRequest
        {
            Method = RestSharp.Method.Put,
            Route = $"api/PackingItem/PalletNumber/{id}",
            Param = new object()
        };
        return await _client.ExecuteAsync<PackingItemDto>(request);

    }
    
}