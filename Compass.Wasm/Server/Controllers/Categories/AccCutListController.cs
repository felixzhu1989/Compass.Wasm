using Compass.Dtos;
using Compass.Wasm.Server.Services.Categories;
using Compass.Wasm.Shared.Categories;
using System.ComponentModel.DataAnnotations;
using Compass.Wasm.Shared.Params;

namespace Compass.Wasm.Server.Controllers.Categories;

[Route("api/[controller]")]
[ApiController]
[UnitOfWork(typeof(CategoryDbContext))]
public class AccCutListController : ControllerBase
{
    #region ctor
    private readonly IAccCutListService _service;
    public AccCutListController(IAccCutListService service)
    {
        _service = service;
    }
    #endregion


    #region 基本增删改查
    [HttpGet("All")]
    public async Task<ApiResponse<List<AccCutListDto>>> GetAll() => await _service.GetAllAsync();

    [HttpGet("{id}")]
    public async Task<ApiResponse<AccCutListDto>> GetSingle([RequiredGuid] Guid id) => await _service.GetSingleAsync(id);

    [HttpPost("Add")]
    public async Task<ApiResponse<AccCutListDto>> Add(AccCutListDto dto) => await _service.AddAsync(dto);

    [HttpPut("{id}")]
    public async Task<ApiResponse<AccCutListDto>> Update([RequiredGuid] Guid id, AccCutListDto dto) => await _service.UpdateAsync(id, dto);

    [HttpDelete("{id}")]
    public async Task<ApiResponse<AccCutListDto>> Delete([RequiredGuid] Guid id) => await _service.DeleteAsync(id);

    #endregion

    [HttpGet("AccType")]
    public async Task<ApiResponse<List<AccCutListDto>>> GetAllByAccType(AccCutListParam param) => await _service.GetAllByAccTypeAsync(param);
}