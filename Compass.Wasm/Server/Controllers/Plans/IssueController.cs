using Compass.Wasm.Shared.Projects;
using Compass.Wasm.Shared;
using System.ComponentModel.DataAnnotations;
using Compass.PlanService.Infrastructure;
using Compass.Wasm.Shared.Plans;
using Compass.Wasm.Server.Services.Plans;

namespace Compass.Wasm.Server.Controllers.Plans;

[Route("api/[controller]")]
[ApiController]
[UnitOfWork(typeof(PlanDbContext))]
public class IssueController : ControllerBase
{
    private readonly IIssueService _service;
    public IssueController(IIssueService service)
    {
        _service = service;
    }
    #region 标准增删改查
    [HttpGet("All")]
    public async Task<ApiResponse<List<IssueDto>>> GetAll() => await _service.GetAllAsync();

    [HttpGet("{id}")]
    public async Task<ApiResponse<IssueDto>> GetSingle([RequiredGuid] Guid id) => await _service.GetSingleAsync(id);

    [HttpPost("Add")]
    public async Task<ApiResponse<IssueDto>> Add(IssueDto dto) => await _service.AddAsync(dto);

    [HttpPut("{id}")]
    public async Task<ApiResponse<IssueDto>> Update([RequiredGuid] Guid id, IssueDto dto) => await _service.UpdateAsync(id, dto);

    [HttpDelete("{id}")]
    public async Task<ApiResponse<IssueDto>> Delete([RequiredGuid] Guid id) => await _service.DeleteAsync(id);
    #endregion

    #region 扩展的查询功能,Blazor
    [HttpGet("MainPlan/{mainPlanId}")]
    public async Task<ApiResponse<List<IssueDto>>> GetAllByMainPlanId([RequiredGuid] Guid mainPlanId) => await _service.GetAllByMainPlanIdAsync(mainPlanId);
    /// <summary>
    /// 更新主计划状态
    /// </summary>
    [HttpPut("UpdateStatuses/{id}")]
    public async Task<ApiResponse<IssueDto>> UpdateStatuses([RequiredGuid] Guid id, IssueDto dto) => await _service.UpdateStatusesAsync(id, dto);
    #endregion



}