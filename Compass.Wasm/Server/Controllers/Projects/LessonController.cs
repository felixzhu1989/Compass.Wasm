using Compass.Wasm.Server.Services.Projects;
using Compass.Wasm.Shared.Projects;
using Compass.Wasm.Shared;
using System.ComponentModel.DataAnnotations;

namespace Compass.Wasm.Server.Controllers.Projects;

[Route("api/[controller]")]
[ApiController]
[UnitOfWork(typeof(ProjectDbContext))]
public class LessonController : ControllerBase
{
    private readonly ILessonService _service;
    public LessonController(ILessonService service)
    {
        _service = service;
    }
    #region 标准增删改查
    [HttpGet("All")]
    public async Task<ApiResponse<List<LessonDto>>> GetAll() => await _service.GetAllAsync();

    [HttpGet("{id}")]
    public async Task<ApiResponse<LessonDto>> GetSingle([RequiredGuid] Guid id) => await _service.GetSingleAsync(id);

    [HttpPost("Add")]
    public async Task<ApiResponse<LessonDto>> Add(LessonDto dto) => await _service.AddAsync(dto);

    [HttpPut("{id}")]
    public async Task<ApiResponse<LessonDto>> Update([RequiredGuid] Guid id, LessonDto dto) => await _service.UpdateAsync(id, dto);

    [HttpDelete("{id}")]
    public async Task<ApiResponse<LessonDto>> Delete([RequiredGuid] Guid id) => await _service.DeleteAsync(id);
    #endregion

    #region 扩展的查询功能,Blazor
    [HttpGet("Project/{projectId}")]
    public async Task<ApiResponse<List<LessonDto>>> GetAllByProjectId([RequiredGuid] Guid projectId) => await _service.GetAllByProjectIdAsync(projectId);

    #endregion
}