using Compass.Wasm.Server.ProjectService;
using Compass.Wasm.Shared.ProjectService;
using Compass.Wasm.Shared;
using System.ComponentModel.DataAnnotations;
using Compass.Wasm.Shared.Parameter;

namespace Compass.Wasm.Server.Controllers.ProjectService;

[Route("api/[controller]")]
[ApiController]
[UnitOfWork(typeof(ProjectDbContext))]
public class CutListController : ControllerBase
{
    private readonly ICutListService _service;
    public CutListController(ICutListService service)
    {
        _service = service;
    }

    #region 标准增删改查
    [HttpGet("All")]
    public async Task<ApiResponse<List<CutListDto>>> GetAll() => await _service.GetAllAsync();

    [HttpGet("{id}")]
    public async Task<ApiResponse<CutListDto>> GetSingle([RequiredGuid] Guid id) => await _service.GetSingleAsync(id);

    [HttpPost("Add")]
    public async Task<ApiResponse<CutListDto>> Add(CutListDto dto) => await _service.AddAsync(dto);

    [HttpPut("{id}")]
    public async Task<ApiResponse<CutListDto>> Update([RequiredGuid] Guid id, CutListDto dto) => await _service.UpdateAsync(id, dto);

    [HttpDelete("{id}")]
    public async Task<ApiResponse<CutListDto>> Delete([RequiredGuid] Guid id) => await _service.DeleteAsync(id);
    #endregion


    #region 扩展的查询功能,WPF

    #endregion

    #region 扩展的查询功能,Blazor
    [HttpGet("Module")]
    public async Task<ApiResponse<List<CutListDto>>> GetAllByModuleId(CutListParameter parameter) => await _service.GetAllByModuleIdAsync(parameter);


    #endregion


}