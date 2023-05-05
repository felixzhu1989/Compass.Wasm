using Compass.PlanService.Infrastructure;
using Compass.Wasm.Server.Services.Plans;
using Compass.Wasm.Shared;
using Compass.Wasm.Shared.Plans;
using Compass.Wasm.Shared.Projects;
using System.ComponentModel.DataAnnotations;

namespace Compass.Wasm.Server.Controllers.Plans;

[Route("api/[controller]")]
[ApiController]
[UnitOfWork(typeof(PlanDbContext))]
//[Authorize(Roles = "admin,pmc")]
public class MainPlanController : ControllerBase
{
    private readonly IMainPlanService _service;
    public MainPlanController(IMainPlanService service)
    {
        _service = service;
    }

    #region 标准增删改查
    [HttpGet("All")]
    public async Task<ApiResponse<List<MainPlanDto>>> GetAll() => await _service.GetAllAsync();

    [HttpGet("{id}")]
    public async Task<ApiResponse<MainPlanDto>> GetSingle([RequiredGuid] Guid id) => await _service.GetSingleAsync(id);

    [HttpPost("Add")]
    public async Task<ApiResponse<MainPlanDto>> Add(MainPlanDto dto) => await _service.AddAsync(dto);

    [HttpPut("{id}")]
    public async Task<ApiResponse<MainPlanDto>> Update([RequiredGuid] Guid id, MainPlanDto dto) => await _service.UpdateAsync(id, dto);

    [HttpDelete("{id}")]
    public async Task<ApiResponse<MainPlanDto>> Delete([RequiredGuid] Guid id) => await _service.DeleteAsync(id);
    #endregion

    #region Blazor扩展
    //更新主计划状态
    [HttpPut("UpdateStatuses/{id}")]
    public async Task<ApiResponse<MainPlanDto>> UpdateStatuses([RequiredGuid] Guid id, MainPlanDto dto) => await _service.UpdateStatusesAsync(id, dto);

    //获取主页信息
    [HttpGet("IndexData")]
    public async Task<ApiResponse<List<MainPlanDto>>> GetIndexData() => await _service.GetIndexDataAsync();

    #endregion

}