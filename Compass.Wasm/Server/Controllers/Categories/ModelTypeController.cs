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
public class ModelTypeController : ControllerBase
{
    private readonly IModelTypeService _service;

    public ModelTypeController(IModelTypeService service)
    {
        _service = service;
    }

    #region 基本增删改查
    [HttpGet("All")]
    public async Task<ApiResponse<List<ModelTypeDto>>> GetAll() => await _service.GetAllAsync();

    [HttpGet("{id}")]
    public async Task<ApiResponse<ModelTypeDto>> GetSingle([RequiredGuid] Guid id) => await _service.GetSingleAsync(id);

    [HttpPost("Add")]
    public async Task<ApiResponse<ModelTypeDto>> Add(ModelTypeDto dto) => await _service.AddAsync(dto);

    [HttpPut("{id}")]
    public async Task<ApiResponse<ModelTypeDto>> Update([RequiredGuid] Guid id, ModelTypeDto dto) => await _service.UpdateAsync(id, dto);

    [HttpDelete("{id}")]
    public async Task<ApiResponse<ModelTypeDto>> Delete([RequiredGuid] Guid id) => await _service.DeleteAsync(id);

    #endregion

}