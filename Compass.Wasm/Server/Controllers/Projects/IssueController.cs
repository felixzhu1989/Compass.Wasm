using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Compass.Wasm.Shared.Projects;

namespace Compass.Wasm.Server.Controllers.Projects;

[Route("api/[controller]")]
[ApiController]
[UnitOfWork(typeof(ProjectDbContext))]
public class IssueController : ControllerBase
{
    private readonly ProjectDomainService _domainService;
    private readonly ProjectDbContext _dbContext;
    private readonly IProjectRepository _repository;
    private readonly IMapper _mapper;
    private readonly IEventBus _eventBus;
    private readonly IdentityUserManager _userManager;

    public IssueController(ProjectDomainService domainService, ProjectDbContext dbContext, IProjectRepository repository, IMapper mapper, IEventBus eventBus, IdentityUserManager userManager)
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
        var issue = new Issue(Guid.NewGuid(), request.ProjectId, request.ReportUserId, request.ProjectStatus,request.Stakeholder, request.Description, request.DescriptionUrl);
        await _dbContext.Issues.AddAsync(issue);
        return issue.Id;
    }
    [HttpPut("{id}")]
    public async Task<ActionResult> Update([RequiredGuid] Guid id, IssueResponse request)
    {
        var issue = await _repository.GetIssueByIdAsync(id);
        if (issue == null) return NotFound($"没有Id={id}的Issue");
        issue.ChangeStakeholder(request.Stakeholder).ChangeDescription(request.Description).ChangeDescriptionUrl(request.DescriptionUrl);
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