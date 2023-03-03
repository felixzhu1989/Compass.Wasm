using Compass.Wasm.Shared;
using Compass.Wasm.Shared.CategoryService;

namespace Compass.Wasm.Server.CategoryService;

public interface IProductService:IBaseService<ProductDto>
{
    //扩展的查询功能,WPF
    Task<ApiResponse<List<ProductDto>>> GetModelTypeTreeAsync();
}