using Compass.PlanService.Infrastructure;
using Compass.Wasm.Server.Services.Plans;
using Compass.Wasm.Shared.Plans;
using Compass.Wasm.Shared;
using System.ComponentModel.DataAnnotations;

namespace Compass.Wasm.Server.Controllers.Plans;

[Route("api/[controller]")]
[ApiController]
[UnitOfWork(typeof(PlanDbContext))]
public class PackingItemController : ControllerBase
{
    #region ctor
    private readonly IPackingItemService _service;
    public PackingItemController(IPackingItemService service)
    {
        _service = service;
    }
    #endregion


    #region 标准增删改查
    [HttpGet("All")]
    public async Task<ApiResponse<List<PackingItemDto>>> GetAll() => await _service.GetAllAsync();

    [HttpGet("{id}")]
    public async Task<ApiResponse<PackingItemDto>> GetSingle([RequiredGuid] Guid id) => await _service.GetSingleAsync(id);

    [HttpPost("Add")]
    public async Task<ApiResponse<PackingItemDto>> Add(PackingItemDto dto) => await _service.AddAsync(dto);

    [HttpPut("{id}")]
    public async Task<ApiResponse<PackingItemDto>> Update([RequiredGuid] Guid id, PackingItemDto dto) => await _service.UpdateAsync(id, dto);

    [HttpDelete("{id}")]
    public async Task<ApiResponse<PackingItemDto>> Delete([RequiredGuid] Guid id) => await _service.DeleteAsync(id);
    #endregion


}