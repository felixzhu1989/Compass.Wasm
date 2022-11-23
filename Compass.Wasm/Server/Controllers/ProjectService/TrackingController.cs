using AutoMapper;
using Compass.Wasm.Shared.ProjectService;
using System.ComponentModel.DataAnnotations;

namespace Compass.Wasm.Server.Controllers.ProjectService;

[Route("api/[controller]")]
[ApiController]
[UnitOfWork(typeof(PMDbContext))]
//[Authorize(Roles = "admin,pm")]
public class TrackingController : ControllerBase
{

    private readonly PMDomainService _domainService;
    private readonly PMDbContext _dbContext;
    private readonly IPMRepository _repository;
    private readonly IMapper _mapper;
    private readonly IEventBus _eventBus;

    public TrackingController(PMDomainService domainService, PMDbContext dbContext, IPMRepository repository, IMapper mapper, IEventBus eventBus)
    {
        _domainService = domainService;
        _dbContext = dbContext;
        _repository = repository;
        _mapper = mapper;
        _eventBus = eventBus;
    }

    [HttpGet("All")]
    public async Task<TrackingResponse[]> FindAll()
    {
        //Todo:需要将项目信息和异常信息一起查询？还是再界面查询后组合？
        return await _mapper.ProjectTo<TrackingResponse>(await _repository.GetTrackingsAsync()).ToArrayAsync();
    }
    [HttpGet("{id}")]
    public async Task<ActionResult<TrackingResponse?>> FindById([RequiredGuid] Guid id)
    {
        var tracking = await _repository.GetTrackingByIdAsync(id);
        if (tracking == null) return NotFound($"没有Id={id}的Tracking");
        return _mapper.Map<TrackingResponse>(tracking);
    }
    
    //无需Add,由集成事件自动创建

    //项目状态和问题解决是由eventbus发出事件来维护，但是保留手动修改的接口
    [HttpPut("ProjectStatus/{id}")]
    public async Task<ActionResult> UpdateProjectStatus([RequiredGuid] Guid id, ProjectStatus projectStatus)
    {
        var tracking = await _repository.GetTrackingByIdAsync(id);
        if (tracking == null) return NotFound($"没有Id={id}的Tracking");
        tracking.ChangeProjectStatus(projectStatus);
        return Ok();
    }
    [HttpPut("ProblemNotResolved/{id}")]
    public async Task<ActionResult> UpdateProblemNotResolved([RequiredGuid] Guid id, bool problemNotResolved)
    {
        var tracking = await _repository.GetTrackingByIdAsync(id);
        if (tracking == null) return NotFound($"没有Id={id}的Tracking");
        tracking.ChangeProblemNotResolved(problemNotResolved);
        return Ok();
    }

    //可以添加一些修改api，用于修正错误的记录时间
    //DrawingPlanedTime,DrawingReleaseTime,WarehousingTime,CloseTime 
    [HttpPut("DrawingPlanedTime/{id}")]
    public async Task<ActionResult> UpdateDrawingPlanedTime([RequiredGuid] Guid id, DateTime drawingPlanedTime)
    {
        var tracking = await _repository.GetTrackingByIdAsync(id);
        if (tracking == null) return NotFound($"没有Id={id}的Tracking");
        tracking.ChangeDrawingPlanedTime(drawingPlanedTime);
        return Ok();
    }
    [HttpPut("DrawingReleaseTime/{id}")]
    public async Task<ActionResult> UpdateDrawingReleaseTime([RequiredGuid] Guid id, DateTime moduleReleaseTime)
    {
        var tracking = await _repository.GetTrackingByIdAsync(id);
        if (tracking == null) return NotFound($"没有Id={id}的Tracking");
        tracking.ChangeModuleReleaseTime(moduleReleaseTime);
        return Ok();
    }
    [HttpPut("WarehousingTime/{id}")]
    public async Task<ActionResult> UpdateWarehousingTime([RequiredGuid] Guid id, DateTime warehousingTime)
    {
        var tracking = await _repository.GetTrackingByIdAsync(id);
        if (tracking == null) return NotFound($"没有Id={id}的Tracking");
        tracking.ChangeWarehousingTime(warehousingTime);
        return Ok();
    }
    [HttpPut("ShippingTime/{id}")]
    public async Task<ActionResult> UpdateShippingTime([RequiredGuid] Guid id, DateTime shippingTime)
    {
        var tracking = await _repository.GetTrackingByIdAsync(id);
        if (tracking == null) return NotFound($"没有Id={id}的Tracking");
        tracking.ChangeShippingTime(shippingTime);
        return Ok();
    }
    [HttpPut("ClosedTime/{id}")]
    public async Task<ActionResult> UpdateClosedTime([RequiredGuid] Guid id, DateTime closedTime)
    {
        var tracking = await _repository.GetTrackingByIdAsync(id);
        if (tracking == null) return NotFound($"没有Id={id}的Tracking");
        tracking.ChangeClosedTime(closedTime);
        return Ok();
    }
    //由eventbus发出事件来维护，但是保留手动修改的接口
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete([RequiredGuid] Guid id)
    {
        var tracking = await _repository.GetTrackingByIdAsync(id);
        if (tracking == null) return NotFound($"没有Id={id}的Tracking");
        //这样做仍然是幂等的，因为“调用N次，确保服务器处于与第一次调用相同的状态。”与响应无关
        tracking.SoftDelete();//软删除
        return Ok();
    }
}