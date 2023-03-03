using AutoMapper;
using Compass.Wasm.Shared.ProjectService;
using System.ComponentModel.DataAnnotations;
using Compass.Wasm.Shared;

namespace Compass.Wasm.Server.Controllers.ProjectService;

[Route("api/[controller]")]
[ApiController]
[UnitOfWork(typeof(ProjectDbContext))]
//[Authorize(Roles = "admin,pm")]
public class DrawingPlanController : ControllerBase
{
    private readonly ProjectDomainService _domainService;
    private readonly ProjectDbContext _dbContext;
    private readonly IProjectRepository _repository;
    private readonly IMapper _mapper;
    private readonly IEventBus _eventBus;

    public DrawingPlanController(ProjectDomainService domainService, ProjectDbContext dbContext, IProjectRepository repository, IMapper mapper, IEventBus eventBus)
    {
        _domainService = domainService;
        _dbContext = dbContext;
        _repository = repository;
        _mapper = mapper;
        _eventBus = eventBus;
    }

    [HttpGet("All/{page}")]
    public async Task<ApiPaginationResponse<List<DrawingPlanResponse>>> FindAll(int page)
    {
        var result = await _repository.GetDrawingPlansAsync(page);
        return new ApiPaginationResponse<List<DrawingPlanResponse>>
        {
            Result = await _mapper.ProjectTo<DrawingPlanResponse>(result.Result).ToListAsync(),
            CurrentPage = result.CurrentPage,
            Pages = result.Pages
        };
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<DrawingPlanResponse?>> FindById([RequiredGuid] Guid id)
    {
        var drawingPlan = await _repository.GetDrawingPlanByIdAsync(id);
        if (drawingPlan == null) return NotFound($"没有Id={id}的DrawingPlan");
        return _mapper.Map<DrawingPlanResponse>(drawingPlan);
    }
        

    [HttpGet("ProjectsNotPlanned")]
    public async Task<ProjectDto[]> FindProjectsNotPlanned()
    {
        //使用AutoMapper将Project转换成ProjectResponse（Dto）
        var projects = await _repository.GetProjectsNotDrawingPlannedAsync();
        var response = new List<ProjectDto>();
        foreach (var project in projects)
        {
            response.Add(new ProjectDto{Id = project.Id,OdpNumber = project.OdpNumber,Name = project.Name});
        }
        return response.ToArray();

        //return await _mapper.ProjectTo<ProjectResponse>(projects.AsQueryable()).ToArrayAsync();
    }

    //[HttpGet("IsDrawingsNotAssigned/{projectId}")]
    //public Task<bool> IsDrawingsNotAssigned([RequiredGuid] Guid projectId)
    //{
    //    return _repository.IsDrawingsNotAssignedByProjectIdAsync(projectId);
    //}

    [HttpGet("DrawingsNotAssigned/{projectId}")]
    public async Task<List<DrawingDto>> FindDrawingsNotAssigned([RequiredGuid] Guid projectId)
    {
        var drawings = await _repository.GetDrawingsNotAssignedByProjectIdAsync(projectId);
        var response = new List<DrawingDto>();
        foreach (var drawing in drawings)
        {
            response.Add(new DrawingDto{Id = drawing.Id,ItemNumber = drawing.ItemNumber,DrawingUrl = drawing.DrawingUrl});
        }
        return response;

        //return await _mapper.ProjectTo<DrawingResponse>(await _repository.GetDrawingsNotAssignedByProjectIdAsync(projectId)).ToArrayAsync();
    }

    [HttpGet("DrawingsAssigned/{projectId}")]
    public async  Task<Dictionary<Guid, DrawingDto[]>> FindDrawingsAssigned([RequiredGuid] Guid projectId)
    {
        var dic = await _repository.GetDrawingsAssignedByProjectIdAsync(projectId);
        var responses = new Dictionary<Guid, DrawingDto[]>();
        foreach (var d in dic)
        {
            //将Drawing类型使用AutoMapper转换成DrawingResponse
            responses.Add(d.Key,await _mapper.ProjectTo<DrawingDto>(d.Value).ToArrayAsync());
        }
        return responses;
    }
        
    [HttpPost("Add")]
    public async Task<ActionResult<Guid>> Add(AddDrawingPlanRequest request)
    {
        var drawingPlan = new DrawingPlan( request.ProjectId);
        await _dbContext.DrawingsPlan.AddAsync(drawingPlan);
        return drawingPlan.Id;
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update([RequiredGuid] Guid id, DrawingPlanResponse request)
    {
        var drawingPlan = await _repository.GetDrawingPlanByIdAsync(id);
        if (drawingPlan == null) return NotFound($"没有Id={id}的DrawingPlan");
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