using System.Net.Http.Json;
using Compass.Dtos;
using Compass.Wasm.Shared;
using Compass.Wasm.Shared.Categories;

namespace Compass.Wasm.Client.Services.Categories;

public interface IProductService : IBaseService<ProductDto>
{
    Task<ApiResponse<List<ProductDto>>> GetModelTypeTreeAsync();
}

public class ProductService:BaseService<ProductDto>,IProductService
{
    private readonly HttpClient _http;
    public ProductService(HttpClient http) : base(http, "Product")
    {
        _http = http;
    }

    //ModelTypeTree
    public Task<ApiResponse<List<ProductDto>>> GetModelTypeTreeAsync()
    {
        return _http.GetFromJsonAsync<ApiResponse<List<ProductDto>>>("api/Product/ModelTypeTree")!;
    }
}