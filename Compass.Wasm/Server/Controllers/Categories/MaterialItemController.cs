using Compass.Wasm.Shared.Categories;
using Compass.Wasm.Shared;
using System.ComponentModel.DataAnnotations;
using Compass.Dtos;
using Compass.Wasm.Server.Services.Categories;

namespace Compass.Wasm.Server.Controllers.Categories;

[Route("api/[controller]")]
[ApiController]
[UnitOfWork(typeof(CategoryDbContext))]
public class MaterialItemController : ControllerBase
{
    #region ctor
    private readonly IMaterialItemService _service;
    public MaterialItemController(IMaterialItemService service)
    {
        _service = service;
    } 
    #endregion

    #region 基本增删改查
    [HttpGet("All")]
    public async Task<ApiResponse<List<MaterialItemDto>>> GetAll() => await _service.GetAllAsync();

    [HttpGet("{id}")]
    public async Task<ApiResponse<MaterialItemDto>> GetSingle([RequiredGuid] Guid id) => await _service.GetSingleAsync(id);

    [HttpPost("Add")]
    public async Task<ApiResponse<MaterialItemDto>> Add(MaterialItemDto dto) => await _service.AddAsync(dto);

    [HttpPut("{id}")]
    public async Task<ApiResponse<MaterialItemDto>> Update([RequiredGuid] Guid id, MaterialItemDto dto) => await _service.UpdateAsync(id, dto);

    [HttpDelete("{id}")]
    public async Task<ApiResponse<MaterialItemDto>> Delete([RequiredGuid] Guid id) => await _service.DeleteAsync(id);

    #endregion

    #region 扩展
    /// <summary>
    /// 更新库存
    /// </summary>
    [HttpPut("UpdateInventory/{id}")]
    public async Task<ApiResponse<MaterialItemDto>> UpdateInventory([RequiredGuid] Guid id, MaterialItemDto dto) => await _service.UpdateInventoryAsync(id, dto);
    /// <summary>
    /// 更新其他信息
    /// </summary>
    [HttpPut("UpdateOther/{id}")]
    public async Task<ApiResponse<MaterialItemDto>> UpdateOther([RequiredGuid] Guid id, MaterialItemDto dto) => await _service.UpdateOtherAsync(id, dto);

    [HttpGet("Top50")]
    public async Task<ApiResponse<List<MaterialItemDto>>> GetTop50() => await _service.GetTop50Async();
    #endregion
}