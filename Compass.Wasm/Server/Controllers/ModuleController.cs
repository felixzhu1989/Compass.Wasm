using AutoMapper;
using Compass.Wasm.Client.ProjectService;
using Compass.Wasm.Shared.ProjectService;
using System.ComponentModel.DataAnnotations;

namespace Compass.Wasm.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
[UnitOfWork(typeof(PMDbContext))]
//[Authorize(Roles = "admin,pm,designer")]
public class ModuleController : ControllerBase
{
    private readonly PMDomainService _domainService;
    private readonly PMDbContext _dbContext;
    private readonly IPMRepository _repository;
    private readonly IMapper _mapper;

    public ModuleController(PMDomainService domainService, PMDbContext dbContext, IPMRepository repository, IMapper mapper)
    {
        _domainService = domainService;
        _dbContext = dbContext;
        _repository = repository;
        _mapper = mapper;
    }

    [HttpGet("All/{drawingId}")]
    public async Task<ModuleResponse[]> FindAll([RequiredGuid] Guid drawingId)
    {
        return await _mapper.ProjectTo<ModuleResponse>(await _repository.GetModulesByDrawingIdAsync(drawingId)).ToArrayAsync();
    }
    [HttpGet("{id}")]
    public async Task<ActionResult<ModuleResponse?>> FindById([RequiredGuid] Guid id)
    {
        var module = await _repository.GetModuleByIdAsync(id);
        if (module == null) return NotFound($"没有Id={id}的Module");
        return _mapper.Map<ModuleResponse>(module);
    }
    [HttpPost("Add")]
    public async Task<ActionResult<Guid>> Add(AddModuleRequest request)
    {
        var module = new Compass.ProjectService.Domain.Entities.Module(Guid.NewGuid(), request.DrawingId, request.ModelId, request.Name,request.SpecialNotes);
        await _dbContext.Modules.AddAsync(module);
        return module.Id;
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update([RequiredGuid] Guid id, ModuleResponse request)
    {
        var module = await _repository.GetModuleByIdAsync(id);
        if (module == null) return NotFound($"没有Id={id}的Module");
        module.ChangeModelId(request.ModelId).ChangeName(request.Name).ChangeSpecialNotes(request.SpecialNotes);
        return Ok();
    }
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete([RequiredGuid] Guid id)
    {
        var module = await _repository.GetModuleByIdAsync(id);
        if (module == null) return NotFound($"没有Id={id}的Module");
        //这样做仍然是幂等的，因为“调用N次，确保服务器处于与第一次调用相同的状态。”与响应无关
        module.SoftDelete();//软删除
        return Ok();
    }
}