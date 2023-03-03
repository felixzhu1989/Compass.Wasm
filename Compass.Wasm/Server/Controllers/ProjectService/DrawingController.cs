using AutoMapper;
using Compass.Wasm.Shared.ProjectService;
using System.ComponentModel.DataAnnotations;
using Compass.Wasm.Server.ProjectService;
using Compass.Wasm.Shared;

namespace Compass.Wasm.Server.Controllers.ProjectService;

[Route("api/[controller]")]
[ApiController]
[UnitOfWork(typeof(ProjectDbContext))]
//[Authorize(Roles = "admin,pm,designer")]
public class DrawingController : ControllerBase
{
    private readonly IDrawingService _service;
    public DrawingController(IDrawingService service)
    {
        _service = service;
    }

    #region 标准增删改查
    [HttpGet("All")]
    public async Task<ApiResponse<List<DrawingDto>>> GetAll() => await _service.GetAllAsync();

    [HttpGet("{id}")]
    public async Task<ApiResponse<DrawingDto>> GetSingle([RequiredGuid] Guid id) => await _service.GetSingleAsync(id);

    [HttpPost("Add")]
    public async Task<ApiResponse<DrawingDto>> Add(DrawingDto dto) => await _service.AddAsync(dto);

    [HttpPut("{id}")]
    public async Task<ApiResponse<DrawingDto>> Update([RequiredGuid] Guid id, DrawingDto dto) => await _service.UpdateAsync(id, dto);

    [HttpDelete("{id}")]
    public async Task<ApiResponse<DrawingDto>> Delete([RequiredGuid] Guid id) => await _service.DeleteAsync(id);
    #endregion

    #region 扩展的查询功能,WPF

    #endregion

    #region 扩展的查询功能,Blazor
    [HttpGet("Project/{projectId}")]
    public async Task<ApiResponse<List<DrawingDto>>> GetAllByProjectId([RequiredGuid] Guid projectId) => await _service.GetAllByProjectIdAsync(projectId);

    #endregion
}