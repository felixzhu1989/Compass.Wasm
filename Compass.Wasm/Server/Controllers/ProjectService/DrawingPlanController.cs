using AutoMapper;
using Compass.Wasm.Client.ProjectService;
using Compass.Wasm.Shared.ProjectService;
using System.ComponentModel.DataAnnotations;
using Compass.Wasm.Server.ProjectService.TrackingEvent;

namespace Compass.Wasm.Server.Controllers.ProjectService;

[Route("api/[controller]")]
[ApiController]
[UnitOfWork(typeof(PMDbContext))]
//[Authorize(Roles = "admin,pm")]
public class DrawingPlanController : ControllerBase
{
    private readonly PMDomainService _domainService;
    private readonly PMDbContext _dbContext;
    private readonly IPMRepository _repository;
    private readonly IMapper _mapper;
    private readonly IEventBus _eventBus;

    public DrawingPlanController(PMDomainService domainService, PMDbContext dbContext, IPMRepository repository, IMapper mapper, IEventBus eventBus)
    {
        _domainService = domainService;
        _dbContext = dbContext;
        _repository = repository;
        _mapper = mapper;
        _eventBus = eventBus;
    }

    [HttpGet("All")]
    public async Task<DrawingPlanResponse[]> FindAll()
    {
        return await _mapper.ProjectTo<DrawingPlanResponse>(await _repository.GetDrawingPlansAsync()).ToArrayAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<DrawingPlanResponse?>> FindById([RequiredGuid] Guid id)
    {
        var drawingPlan = await _repository.GetDrawingPlanByIdAsync(id);
        if (drawingPlan == null) return NotFound($"没有Id={id}的DrawingPlan");
        return _mapper.Map<DrawingPlanResponse>(drawingPlan);
    }
        

    [HttpGet("ProjectsNotPlanned")]
    public async Task<ProjectResponse[]> FindProjectsNotPlanned()
    {
        //使用AutoMapper将Project转换成ProjectResponse（Dto）
        var projects = await _repository.GetProjectsNotDrawingPlannedAsync();
        var response = new List<ProjectResponse>();
        foreach (var project in projects)
        {
            response.Add(new ProjectResponse{Id = project.Id,OdpNumber = project.OdpNumber,Name = project.Name,DeliveryDate = project.DeliveryDate});
        }
        return response.ToArray();

        //return await _mapper.ProjectTo<ProjectResponse>(projects.AsQueryable()).ToArrayAsync();
    }

    [HttpGet("IsDrawingsNotAssigned/{projectId}")]
    public Task<bool> IsDrawingsNotAssigned([RequiredGuid] Guid projectId)
    {
        return _repository.IsDrawingsNotAssignedByProjectIdAsync(projectId);
    }

    [HttpGet("DrawingsNotAssigned/{projectId}")]
    public async Task<DrawingResponse[]> FindDrawingsNotAssigned([RequiredGuid] Guid projectId)
    {
        var drawings = await _repository.GetDrawingsNotAssignedByProjectIdAsync(projectId);
        var response = new List<DrawingResponse>();
        foreach (var drawing in drawings)
        {
            response.Add(new DrawingResponse{Id = drawing.Id,ItemNumber = drawing.ItemNumber,DrawingUrl = drawing.DrawingUrl});
        }
        return response.ToArray();

        //return await _mapper.ProjectTo<DrawingResponse>(await _repository.GetDrawingsNotAssignedByProjectIdAsync(projectId)).ToArrayAsync();
    }

    [HttpGet("DrawingsAssigned/{projectId}")]
    public async  Task<Dictionary<Guid, DrawingResponse[]>> FindDrawingsAssigned([RequiredGuid] Guid projectId)
    {
        var dic = await _repository.GetDrawingsAssignedByProjectIdAsync(projectId);
        var responses = new Dictionary<Guid, DrawingResponse[]>();
        foreach (var d in dic)
        {
            //将Drawing类型使用AutoMapper转换成DrawingResponse
            responses.Add(d.Key,await _mapper.ProjectTo<DrawingResponse>(d.Value).ToArrayAsync());
        }
        return responses;
    }
        
    [HttpPost("Add")]
    public async Task<ActionResult<Guid>> Add(AddDrawingPlanRequest request)
    {
        var drawingPlan = new DrawingPlan( request.ProjectId, request.ReleaseTime);
        await _dbContext.DrawingsPlan.AddAsync(drawingPlan);
            
        //添加计划后，将项目跟踪修改成制图状态
        var eventData = new DrawingPlanCreatedEvent(drawingPlan.Id);
        //发布集成事件
        _eventBus.Publish("ProjectService.DrawingPlan.Created", eventData);

        return drawingPlan.Id;
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update([RequiredGuid] Guid id, DrawingPlanResponse request)
    {
        var drawingPlan = await _repository.GetDrawingPlanByIdAsync(id);
        if (drawingPlan == null) return NotFound($"没有Id={id}的DrawingPlan");
        drawingPlan.ChangeReleaseTime(request.ReleaseTime);
        return Ok();
    }

    [HttpPut("AssignDrawingsToUser")]
    public async Task<ActionResult> AssignDrawingsToUser(AssignDrawingsToUserRequest request)
    {
        await  _repository.AssignDrawingsToUserAsync(request.DrawingIds, request.UserId);
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete([RequiredGuid] Guid id)
    {
        var drawingPlan = await _repository.GetDrawingPlanByIdAsync(id);
        //这样做仍然是幂等的，因为“调用N次，确保服务器处于与第一次调用相同的状态。”与响应无关
        if (drawingPlan == null) return NotFound($"没有Id={id}的DrawingPlan");
        drawingPlan.SoftDelete();//软删除
        return Ok();
    }






}