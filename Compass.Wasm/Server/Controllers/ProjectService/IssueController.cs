using AutoMapper;
using Compass.Wasm.Client.ProjectService;
using Compass.Wasm.Server.ProjectService.ProblemEvent;
using Compass.Wasm.Shared.ProjectService;
using System.ComponentModel.DataAnnotations;

namespace Compass.Wasm.Server.Controllers.ProjectService;

[Route("api/[controller]")]
[ApiController]
[UnitOfWork(typeof(PMDbContext))]
public class IssueController : ControllerBase
{
    private readonly PMDomainService _domainService;
    private readonly PMDbContext _dbContext;
    private readonly IPMRepository _repository;
    private readonly IMapper _mapper;
    private readonly IEventBus _eventBus;
    private readonly IdUserManager _userManager;

    public IssueController(PMDomainService domainService, PMDbContext dbContext, IPMRepository repository, IMapper mapper, IEventBus eventBus, IdUserManager userManager)
    {
        _domainService = domainService;
        _dbContext = dbContext;
        _repository = repository;
        _mapper = mapper;
        _eventBus = eventBus;
        _userManager = userManager;
    }
    [HttpGet("All")]
    public async Task<IssueResponse[]> FindAll()
    {
        return await _mapper.ProjectTo<IssueResponse>(await _repository.GetIssuesAsync()).ToArrayAsync();
    }
    [HttpGet("All/{projectId}")]
    public async Task<IssueResponse[]> FindIssuesByProject([RequiredGuid] Guid projectId)
    {
        return await _mapper.ProjectTo<IssueResponse>(await _repository.GetIssuesByProjectIdAsync(projectId)).ToArrayAsync();
    }
    [HttpGet("{id}")]
    public async Task<ActionResult<IssueResponse?>> FindById([RequiredGuid] Guid id)
    {
        var issue = await _repository.GetIssueByIdAsync(id);
        if (issue == null) return NotFound($"没有Id={id}的Issue");
        return _mapper.Map<IssueResponse>(issue);
    }
    [HttpPost("Add")]
    public async Task<ActionResult<Guid>> Add(AddIssueRequest request)
    {
        var issue = new Issue(Guid.NewGuid(), request.ProjectId, request.ReportUserId, request.ProjectStatus, request.Description, request.DescriptionUrl);
        await _dbContext.Issues.AddAsync(issue);
        return issue.Id;
    }
    [HttpPut("{id}")]
    public async Task<ActionResult> Update([RequiredGuid] Guid id, IssueResponse request)
    {
        var issue = await _repository.GetIssueByIdAsync(id);
        if (issue == null) return NotFound($"没有Id={id}的Issue");
        issue.ChangeDescription(request.Description).ChangeDescriptionUrl(request.DescriptionUrl);
        return Ok();
    }
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete([RequiredGuid] Guid id)
    {
        var issue = await _repository.GetIssueByIdAsync(id);
        //这样做仍然是幂等的，因为“调用N次，确保服务器处于与第一次调用相同的状态。”与响应无关
        if (issue == null) return NotFound($"没有Id={id}的Issue");
        issue.SoftDelete();//软删除
        return Ok();
    }
}