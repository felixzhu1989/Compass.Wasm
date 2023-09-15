using System.ComponentModel.DataAnnotations;
using Compass.DataService.Infrastructure;
using Compass.Dtos;
using Compass.Wasm.Server.Services.Data;
using Compass.Wasm.Shared;
using Compass.Wasm.Shared.Data;

namespace Compass.Wasm.Server.Controllers.Data;

[Route("api/[controller]")]
[ApiController]
[UnitOfWork(typeof(DataDbContext))]
public class ModuleDataController : ControllerBase
{
    private readonly IModuleDataService _service;
    public ModuleDataController(IModuleDataService service)
    {
        _service = service;
    }

    #region 标准增删改查
    [HttpGet("All")]
    public async Task<ApiResponse<List<ModuleData>>> GetAll() => await _service.GetAllAsync();

    [HttpGet("{id}")]
    public async Task<ApiResponse<ModuleData>> GetSingle([RequiredGuid] Guid id) => await _service.GetSingleAsync(id);

    [HttpPost("Add")]
    public async Task<ApiResponse<ModuleData>> Add(ModuleData dto) => await _service.AddAsync(dto);

    [HttpPut("{id}")]
    public async Task<ApiResponse<ModuleData>> Update([RequiredGuid] Guid id, ModuleData dto) => await _service.UpdateAsync(id, dto);

    [HttpDelete("{id}")]
    public async Task<ApiResponse<ModuleData>> Delete([RequiredGuid] Guid id) => await _service.DeleteAsync(id);
    #endregion




}