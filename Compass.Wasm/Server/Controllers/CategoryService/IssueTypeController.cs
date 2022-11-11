using AutoMapper;
using Compass.Wasm.Client.CategoryService;
using Compass.Wasm.Shared.CategoryService;
using Compass.Wasm.Shared.ProjectService;
using System.ComponentModel.DataAnnotations;

namespace Compass.Wasm.Server.Controllers.CategoryService;

[Route("api/[controller]")]
[ApiController]
[UnitOfWork(typeof(CSDbContext))]
//[Authorize(Roles = "admin,pm")]
public class IssueTypeController : ControllerBase
{
    private readonly CSDomainService _domainService;
    private readonly CSDbContext _dbContext;
    private readonly ICSRepository _repository;
    private readonly IMapper _mapper;
    public IssueTypeController(CSDomainService domainService, CSDbContext dbContext, ICSRepository repository, IMapper mapper)
    {
        _domainService = domainService;
        _dbContext = dbContext;
        _repository = repository;
        _mapper = mapper;
    }
    [HttpGet("All/{stakeholder}")]
    public async Task<IssueTypeResponse[]> FindAllByStakeholder([Required] Stakeholder stakeholder)
    {
        //使用AutoMapper将IssueType转换成IssueTypeResponse（Dto）
        return await _mapper.ProjectTo<IssueTypeResponse>(await _repository.GetIssueTypesAsync(stakeholder)).ToArrayAsync();
    }
    [HttpGet("{id}")]
    public async Task<ActionResult<IssueTypeResponse?>> FindById([RequiredGuid] Guid id)
    {
        var issueType = await _repository.GetIssueTypeByIdAsync(id);
        if (issueType == null) return NotFound($"没有Id={id}的IssueType");
        return _mapper.Map<IssueTypeResponse>(issueType);
    }
    [HttpPost]
    public async Task<ActionResult<Guid>> Add(AddIssueTypeRequest request)
    {
        var issueType=new IssueType(Guid.NewGuid(), request.Name, request.Stakeholder);
        await _dbContext.IssueTypes.AddAsync(issueType);
        return issueType.Id;
    }
    [HttpPut("{id}")]
    public async Task<ActionResult> Update([RequiredGuid] Guid id, IssueTypeResponse request)
    {
        var issueType = await _repository.GetIssueTypeByIdAsync(id);
        if (issueType == null) return NotFound($"没有Id={id}的IssueType");
        issueType.ChangeName(request.Name).ChangeStakeholder(request.Stakeholder);
        return Ok();
    }
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete([RequiredGuid] Guid id)
    {
        var issueType = await _repository.GetIssueTypeByIdAsync(id);
        //这样做仍然是幂等的，因为“调用N次，确保服务器处于与第一次调用相同的状态。”与响应无关
        if (issueType == null) return NotFound($"没有Id={id}的IssueType");
        issueType.SoftDelete();
        return Ok();
    }
}