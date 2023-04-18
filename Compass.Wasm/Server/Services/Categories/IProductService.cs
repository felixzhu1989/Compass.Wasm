using Compass.Wasm.Shared;
using Compass.Wasm.Shared.Categories;

namespace Compass.Wasm.Server.Services.Categories;

public interface IProductService : IBaseService<ProductDto>
{
    //扩展的查询功能,WPF
    Task<ApiResponse<List<ProductDto>>> GetModelTypeTreeAsync();
}