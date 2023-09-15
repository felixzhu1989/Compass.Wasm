using Compass.PlanService.Infrastructure;
using Compass.Wasm.Server.Services.Plans;
using Compass.Wasm.Shared;
using Compass.Wasm.Shared.Plans;
using System.ComponentModel.DataAnnotations;
using Compass.Dtos;

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
    /// <summary>
    /// 更新主计划状态
    /// </summary>
    [HttpPut("UpdateStatuses/{id}")]
    public async Task<ApiResponse<MainPlanDto>> UpdateStatuses([RequiredGuid] Guid id, MainPlanDto dto) => await _service.UpdateStatusesAsync(id, dto);

    /// <summary>
    /// 获取主页信息
    /// </summary>
    [HttpGet("IndexData")]
    public async Task<ApiResponse<List<MainPlanDto>>> GetIndexData() => await _service.GetIndexDataAsync();

    /// <summary>
    /// 根据项目Id查询项目的主计划
    /// </summary>
    [HttpGet("Project/{projectId}")]
    public async Task<ApiResponse<List<MainPlanDto>>> GetAllByProjectId([RequiredGuid] Guid projectId) => await _service.GetAllByProjectIdAsync(projectId);



    /// <summary>
    /// 查询项目汇总结果
    /// </summary>
    [HttpGet("Count")]
    public async Task<ApiResponse<MainPlanCountDto>> GetCount() => await _service.GetCountAsync();


    #endregion

}