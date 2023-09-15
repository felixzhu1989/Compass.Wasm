using System.ComponentModel.DataAnnotations;
using Compass.Dtos;
using Compass.Wasm.Server.Services.Categories;
using Compass.Wasm.Shared;
using Compass.Wasm.Shared.Categories;

namespace Compass.Wasm.Server.Controllers.Categories;

[Route("api/[controller]")]
[ApiController]
[UnitOfWork(typeof(CategoryDbContext))]
//[Authorize(Roles = "admin,pm,designer")]
public class ModelController : ControllerBase
{
    private readonly IModelService _service;
    public ModelController(IModelService service)
    {
        _service = service;
    }
    #region 基本增删改查
    [HttpGet("All")]
    public async Task<ApiResponse<List<ModelDto>>> GetAll() => await _service.GetAllAsync();

    [HttpGet("{id}")]
    public async Task<ApiResponse<ModelDto>> GetSingle([RequiredGuid] Guid id) => await _service.GetSingleAsync(id);

    [HttpPost("Add")]
    public async Task<ApiResponse<ModelDto>> Add(ModelDto dto) => await _service.AddAsync(dto);

    [HttpPut("{id}")]
    public async Task<ApiResponse<ModelDto>> Update([RequiredGuid] Guid id, ModelDto dto) => await _service.UpdateAsync(id, dto);

    [HttpDelete("{id}")]
    public async Task<ApiResponse<ModelDto>> Delete([RequiredGuid] Guid id) => await _service.DeleteAsync(id);

    #endregion
}