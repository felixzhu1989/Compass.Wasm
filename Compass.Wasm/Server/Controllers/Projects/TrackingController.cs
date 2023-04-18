using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Compass.Wasm.Server.Events.Projects;
using Compass.Wasm.Shared;
using Compass.Wasm.Shared.Projects;

namespace Compass.Wasm.Server.Controllers.Projects;

[Route("api/[controller]")]
[ApiController]
[UnitOfWork(typeof(ProjectDbContext))]
//[Authorize(Roles = "admin,pm")]
public class TrackingController : ControllerBase
{

    private readonly ProjectDomainService _domainService;
    private readonly ProjectDbContext _dbContext;
    private readonly IProjectRepository _repository;
    private readonly IMapper _mapper;
    private readonly IEventBus _eventBus;

    public TrackingController(ProjectDomainService domainService, ProjectDbContext dbContext, IProjectRepository repository, IMapper mapper, IEventBus eventBus)
    {
        _domainService = domainService;
        _dbContext = dbContext;
        _repository = repository;
        _mapper = mapper;
        _eventBus = eventBus;
    }

    [HttpGet("All/{page}")]
    public async Task<ApiPaginationResponse<List<TrackingResponse>>> FindAll(int page)
    {
        var result = await _repository.GetTrackingsAsync(page);
        var responses = await _mapper.ProjectTo<TrackingResponse>(result.Result).ToListAsync();
        return new ApiPaginationResponse<List<TrackingResponse>>
        {
            Result = responses,
            CurrentPage = result.CurrentPage,
            Pages = result.Pages
        };
    }
    [HttpGet("{id}")]
    public async Task<ActionResult<TrackingResponse?>> FindById([RequiredGuid] Guid id)
    {
        var tracking = await _repository.GetTrackingByIdAsync(id);
        if (tracking == null) return NotFound($"没有Id={id}的Tracking");
        return _mapper.Map<TrackingResponse>(tracking);
    }
    //搜索项目
    [HttpGet("search/{searchText}/{page}")]
    public async Task<ApiPaginationResponse<List<TrackingResponse>>> SearchTrackings(string searchText,int page)
    {
        var result=await _repository.SearchTrackingsAsync(searchText,page);
        return new ApiPaginationResponse<List<TrackingResponse>>
        {
            Result = await _mapper.ProjectTo<TrackingResponse>(result.Result).ToListAsync(),
            CurrentPage = result.CurrentPage,
            Pages = result.Pages
        };
    }
    [HttpGet("searchsuggestions/{searchText}")]
    public  Task<List<string>> GetProjectSearchSuggestions(string searchText)
    {
        return _repository.GetProjectSearchSuggestions(searchText);
    }


    //无需Add,由集成事件自动创建

    
    [HttpPut("ProblemNotResolved/{id}")]
    public async Task<ActionResult> UpdateProblemNotResolved([RequiredGuid] Guid id, bool problemNotResolved)
    {
        var tracking = await _repository.GetTrackingByIdAsync(id);
        if (tracking == null) return NotFound($"没有Id={id}的Tracking");
        tracking.ChangeProblemNotResolved(problemNotResolved);
        return Ok();
    }
    
    [HttpPut("UpdateDate")]
    public async Task<ActionResult> UpdateWarehousingTime(TrackingResponse request)
    {
        var tracking = await _repository.GetTrackingByIdAsync(request.Id);
        if (tracking == null) return NotFound($"没有Id={request.Id}的Tracking");
        tracking.ChangeWarehousingTime(request.WarehousingTime)
            .ChangeShippingStartTime(request.ShippingStartTime)
            .ChangeShippingEndTime(request.ShippingEndTime);
        if (request.WarehousingTime != null && request.ShippingStartTime == null && request.ShippingEndTime == null)
        {
            //todo:发出集成事件，状态修改为入库
            var eventData = new WarehousingEvent(request.Id);
            _eventBus.Publish("ProjectService.Tracking.Warehousing",eventData);
        }
        if (request.WarehousingTime != null && request.ShippingStartTime != null && request.ShippingEndTime == null)
        {
            //todo:发出集成事件，状态修改为发货
            var eventData = new ShippingStartEvent(request.Id);
            _eventBus.Publish("ProjectService.Tracking.ShippingStart", eventData);
        }
        if (request.ShippingEndTime != null)
        {
            //todo:发出集成事件，状态修改为结束
            var eventData = new ShippingEndEvent(request.Id);
            _eventBus.Publish("ProjectService.Tracking.ShippingEnd", eventData);
        }
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