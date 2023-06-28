using Compass.PlanService.Infrastructure;
using Compass.Wasm.Server.Services.Plans;
using Compass.Wasm.Shared.Plans;
using Compass.Wasm.Shared;
using System.ComponentModel.DataAnnotations;
using Compass.Wasm.Shared.Params;

namespace Compass.Wasm.Server.Controllers.Plans;

[Route("api/[controller]")]
[ApiController]
[UnitOfWork(typeof(PlanDbContext))]
public class PackingListController : ControllerBase
{
    #region ctor
    private readonly IPackingListService _service;
    public PackingListController(IPackingListService service)
    {
        _service = service;
    }
    #endregion

    #region 标准增删改查
    [HttpGet("All")]
    public async Task<ApiResponse<List<PackingListDto>>> GetAll() => await _service.GetAllAsync();

    [HttpGet("{id}")]
    public async Task<ApiResponse<PackingListDto>> GetSingle([RequiredGuid] Guid id) => await _service.GetSingleAsync(id);

    [HttpPost("Add")]
    public async Task<ApiResponse<PackingListDto>> Add(PackingListDto dto) => await _service.AddAsync(dto);

    [HttpPut("{id}")]
    public async Task<ApiResponse<PackingListDto>> Update([RequiredGuid] Guid id, PackingListDto dto) => await _service.UpdateAsync(id, dto);

    [HttpDelete("{id}")]
    public async Task<ApiResponse<PackingListDto>> Delete([RequiredGuid] Guid id) => await _service.DeleteAsync(id);
    #endregion

    #region 扩展功能
    [HttpGet("ProjectIdAndBatch")]
    public async Task<ApiResponse<PackingListDto>> GetSingleByProjectIdAndBath(PackingListParam param) => await _service.GetSingleByProjectIdAndBathAsync(param);

    [HttpPost("Add/ProjectIdAndBatch")]
    public async Task<ApiResponse<PackingListDto>> AddByProjectIdAndBath(PackingListDto dto) => await _service.AddByProjectIdAndBathAsync(dto);
    #endregion
}