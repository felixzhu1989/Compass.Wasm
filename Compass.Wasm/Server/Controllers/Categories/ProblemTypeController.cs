using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Compass.Wasm.Shared.Categories;
using Compass.Wasm.Shared.Projects;

namespace Compass.Wasm.Server.Controllers.Categories;

[Route("api/[controller]")]
[ApiController]
[UnitOfWork(typeof(CategoryDbContext))]
//[Authorize(Roles = "admin,pm")]
public class ProblemTypeController : ControllerBase
{
    private readonly CategoryDomainService _domainService;
    private readonly CategoryDbContext _dbContext;
    private readonly ICategoryRepository _repository;
    private readonly IMapper _mapper;
    public ProblemTypeController(CategoryDomainService domainService, CategoryDbContext dbContext, ICategoryRepository repository, IMapper mapper)
    {
        _domainService = domainService;
        _dbContext = dbContext;
        _repository = repository;
        _mapper = mapper;
    }
    [HttpGet("All/{stakeholder}")]
    public async Task<List<ProblemTypeResponse>> FindAllByStakeholder([Required] Stakeholder_e stakeholder)
    {
        //使用AutoMapper将ProblemType转换成ProblemTypeResponse（Dto）
        return await _mapper.ProjectTo<ProblemTypeResponse>(await _repository.GetProblemTypesAsync(stakeholder)).ToListAsync();
    }
    [HttpGet("{id}")]
    public async Task<ActionResult<ProblemTypeResponse?>> FindById([RequiredGuid] Guid id)
    {
        var problemType = await _repository.GetProblemTypeByIdAsync(id);
        if (problemType == null) return NotFound($"没有Id={id}的ProblemType");
        return _mapper.Map<ProblemTypeResponse>(problemType);
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> Add(AddProblemTypeRequest request)
    {
        var problemType = new ProblemType(Guid.NewGuid(), request.Name, request.Stakeholder);
        await _dbContext.ProblemTypes.AddAsync(problemType);
        return problemType.Id;
    }
    [HttpPut("{id}")]
    public async Task<ActionResult> Update([RequiredGuid] Guid id, ProblemTypeResponse request)
    {
        var problemType = await _repository.GetProblemTypeByIdAsync(id);
        if (problemType == null) return NotFound($"没有Id={id}的ProblemType");
        problemType.ChangeName(request.Name).ChangeStakeholder(request.Stakeholder);
        return Ok();
    }
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete([RequiredGuid] Guid id)
    {
        var problemType = await _repository.GetProblemTypeByIdAsync(id);
        //这样做仍然是幂等的，因为“调用N次，确保服务器处于与第一次调用相同的状态。”与响应无关
        if (problemType == null) return NotFound($"没有Id={id}的ProblemType");
        problemType.SoftDelete();
        return Ok();
    }
}