﻿using AutoMapper;
using Compass.Wasm.Client.ProjectService;
using Compass.Wasm.Shared.ProjectService;
using System.ComponentModel.DataAnnotations;
using Compass.Wasm.Server.ProjectService.ProblemEvent;

namespace Compass.Wasm.Server.Controllers.ProjectService;

[Route("api/[controller]")]
[ApiController]
[UnitOfWork(typeof(PMDbContext))]
public class ProblemController : ControllerBase
{
    private readonly PMDomainService _domainService;
    private readonly PMDbContext _dbContext;
    private readonly IPMRepository _repository;
    private readonly IMapper _mapper;
    private readonly IEventBus _eventBus;
    private readonly IdUserManager _userManager;

    public ProblemController(PMDomainService domainService, PMDbContext dbContext, IPMRepository repository, IMapper mapper, IEventBus eventBus,IdUserManager userManager)
    {
        _domainService = domainService;
        _dbContext = dbContext;
        _repository = repository;
        _mapper = mapper;
        _eventBus = eventBus;
        _userManager = userManager;
    }
    [HttpGet("All")]
    public async Task<ProblemResponse[]> FindAll()
    {
        return await _mapper.ProjectTo<ProblemResponse>(await _repository.GetProblemsAsync()).ToArrayAsync();
    }

    [HttpGet("All/{projectId}")]
    public async Task<ProblemResponse[]> FindProblemsByProject([RequiredGuid] Guid projectId)
    {
        return await _mapper.ProjectTo<ProblemResponse>(await _repository.GetProblemsByProjectIdAsync(projectId)).ToArrayAsync();
    }
    //NotResolved
    [HttpGet("NotResolved/{projectId}")]
    public async Task<ProblemResponse[]> FindNotResolvedProblemsByProject([RequiredGuid] Guid projectId)
    {
        return await _mapper.ProjectTo<ProblemResponse>(await _repository.GetNotResolvedProblemsByProjectIdAsync(projectId)).ToArrayAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProblemResponse?>> FindById([RequiredGuid] Guid id)
    {
        var problem = await _repository.GetProblemByIdAsync(id);
        if (problem == null) return NotFound($"没有Id={id}的Problem");
        return _mapper.Map<ProblemResponse>(problem);
    }

    [HttpPost("Add")]
    public async Task<ActionResult<Guid>> Add(AddProblemRequest request)
    {
        var problem = new Problem(Guid.NewGuid(), request.ProjectId, request.ReportUserId, request.ProblemTypeId,request.Description,request.DescriptionUrl);
        await _dbContext.Problems.AddAsync(problem);
        //todo:发出集成事件，修改项目跟踪状态，是否需要发邮件，"ProjectService.Problem.Created"
        var eventData = new ProblemCreatedEvent(problem.ProjectId);
        _eventBus.Publish("ProjectService.Problem.Created", eventData);

        return problem.Id;
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update([RequiredGuid] Guid id, ProblemResponse request)
    {
        var problem = await _repository.GetProblemByIdAsync(id);
        if (problem == null) return NotFound($"没有Id={id}的Problem");
        problem.ChangeProblemTypeId(request.ProblemTypeId)
            .ChangeDescription(request.Description).ChangeDescriptionUrl(request.DescriptionUrl);
        return Ok();
    }

    [HttpPut("Responder/{id}")]
    public async Task<ActionResult> AssignResponder([RequiredGuid] Guid id, ProblemResponse request)
    {
        var problem = await _repository.GetProblemByIdAsync(id);
        if (problem == null) return NotFound($"没有Id={id}的Problem");
        problem.ChangeResponseUserId(request.ResponseUserId)
            .ChangeDeadline(request.Deadline);
        //todo:发出集成事件，修改项目跟踪状态，是否需要发邮件，"ProjectService.Problem.Assigned"
        var user = await _userManager.FindByIdAsync(request.ResponseUserId.ToString());
        var project=await _repository.GetProjectByIdAsync(problem.ProjectId);
        var eventData = new ProblemAssignedEvent(user.UserName,user.Email,project.OdpNumber,project.Name,problem.Description,problem.Deadline.Value,$"http://10.9.18.31/reportproblem/{problem.ProjectId}");
        _eventBus.Publish("ProjectService.Problem.Assigned", eventData);
        return Ok();
    }

    [HttpPut("Solution/{id}")]
    public async Task<ActionResult> SolveProblem([RequiredGuid] Guid id, ProblemResponse request)
    {
        var problem = await _repository.GetProblemByIdAsync(id);
        if (problem == null) return NotFound($"没有Id={id}的Problem");
        problem.ChangeSolution(request.Solution).ChangeSolutionUrl(request.SolutionUrl);
        return Ok();
    }

    [HttpPut("Close/{id}")]
    public async Task<ActionResult> CloseProblem([RequiredGuid] Guid id, ProblemResponse request)
    {
        var problem = await _repository.GetProblemByIdAsync(id);
        if (problem == null) return NotFound($"没有Id={id}的Problem");
        problem.ChangeCloseTime(DateTime.Now).ChangeIsClosed(request.IsClosed);//支持重新打开
        //todo：发送集成事件，判断Tracking中项目异常是否可以关闭，是否需要发邮件，"ProjectService.Problem.Closed"
        var eventData = new ProblemClosedEvent(problem.ProjectId);
        _eventBus.Publish("ProjectService.Problem.Closed", eventData);
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete([RequiredGuid] Guid id)
    {
        var problem = await _repository.GetProblemByIdAsync(id);
        //这样做仍然是幂等的，因为“调用N次，确保服务器处于与第一次调用相同的状态。”与响应无关
        if (problem == null) return NotFound($"没有Id={id}的Problem");
        problem.SoftDelete();//软删除
        //todo：发送集成事件，判断Tracking中项目异常是否可以关闭，是否需要发邮件，"ProjectService.Problem.Closed"
        var eventData = new ProblemClosedEvent(problem.ProjectId);
        _eventBus.Publish("ProjectService.Problem.Closed", eventData);
        return Ok();
    }
}