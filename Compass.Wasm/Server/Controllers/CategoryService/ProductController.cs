using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Compass.Wasm.Server.CategoryService;
using Compass.Wasm.Shared;
using Compass.Wasm.Shared.CategoryService;
using Compass.Wasm.Shared.Parameter;
using Compass.Wasm.Shared.ProjectService;

namespace Compass.Wasm.Server.Controllers.CategoryService;

[Route("api/[controller]")]
[ApiController]
[UnitOfWork(typeof(CategoryDbContext))]
//[Authorize(Roles = "admin,pm,designer")]
public class ProductController : ControllerBase
{
    private readonly IProductService _service;
    public ProductController(IProductService service)
    {
        _service = service;
    }
    #region 基本增删改查
    [HttpGet("All")]
    public async Task<ApiResponse<List<ProductDto>>> GetAll() => await _service.GetAllAsync();

    [HttpGet("{id}")]
    public async Task<ApiResponse<ProductDto>> GetSingle([RequiredGuid] Guid id) => await _service.GetSingleAsync(id);

    [HttpPost("Add")]
    public async Task<ApiResponse<ProductDto>> Add(ProductDto dto) => await _service.AddAsync(dto);

    [HttpPut("{id}")]
    public async Task<ApiResponse<ProductDto>> Update([RequiredGuid] Guid id, ProductDto dto) => await _service.UpdateAsync(id, dto);

    [HttpDelete("{id}")]
    public async Task<ApiResponse<ProductDto>> Delete([RequiredGuid] Guid id) => await _service.DeleteAsync(id);

    #endregion

    #region 扩展的查询功能,WPF
    /// <summary>
    /// 查询产品列表的树结构
    /// </summary>
    [HttpGet("ModelTypeTree")]
    public async Task<ApiResponse<List<ProductDto>>> GetModelTypeTree() => await _service.GetModelTypeTreeAsync();

    #endregion


}