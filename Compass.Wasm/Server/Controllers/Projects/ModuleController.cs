using System.ComponentModel.DataAnnotations;
using Compass.Wasm.Server.Services.Projects;
using Compass.Wasm.Shared;
using Compass.Wasm.Shared.Projects;

namespace Compass.Wasm.Server.Controllers.Projects;

[Route("api/[controller]")]
[ApiController]
[UnitOfWork(typeof(ProjectDbContext))]
//[Authorize(Roles = "admin,pm,designer")]
public class ModuleController : ControllerBase
{
    private readonly IModuleService _service;
    public ModuleController(IModuleService service)
    {
        _service = service;
    }
    #region 标准增删改查
    [HttpGet("All")]
    public async Task<ApiResponse<List<ModuleDto>>> GetAll() => await _service.GetAllAsync();

    [HttpGet("{id}")]
    public async Task<ApiResponse<ModuleDto>> GetSingle([RequiredGuid] Guid id) => await _service.GetSingleAsync(id);

    [HttpPost("Add")]
    public async Task<ApiResponse<ModuleDto>> Add(ModuleDto dto) => await _service.AddAsync(dto);

    [HttpPut("{id}")]
    public async Task<ApiResponse<ModuleDto>> Update([RequiredGuid] Guid id, ModuleDto dto) => await _service.UpdateAsync(id, dto);

    [HttpDelete("{id}")]
    public async Task<ApiResponse<ModuleDto>> Delete([RequiredGuid] Guid id) => await _service.DeleteAsync(id);
    #endregion


    #region 扩展的查询功能,WPF

    #endregion

    #region 扩展的查询功能,Blazor
    [HttpGet("Drawing/{drawingId}")]
    public async Task<ApiResponse<List<ModuleDto>>> GetAllByDrawingId([RequiredGuid] Guid drawingId) => await _service.GetAllByDrawingIdAsync(drawingId);





    #endregion


}