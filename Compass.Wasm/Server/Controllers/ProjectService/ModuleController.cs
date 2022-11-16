using AutoMapper;
using Compass.Wasm.Client.ProjectService;
using Compass.Wasm.Server.ProjectService;
using Compass.Wasm.Shared.ProjectService;
using System.ComponentModel.DataAnnotations;

namespace Compass.Wasm.Server.Controllers.ProjectService;

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
    private readonly IEventBus _eventBus;

    public ModuleController(PMDomainService domainService, PMDbContext dbContext, IPMRepository repository, IMapper mapper, IEventBus eventBus)
    {
        _domainService = domainService;
        _dbContext = dbContext;
        _repository = repository;
        _mapper = mapper;
        _eventBus = eventBus;
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
        var module = new Compass.ProjectService.Domain.Entities.Module(Guid.NewGuid(), request.DrawingId, request.ModelId, request.Name.ToUpper(), request.SpecialNotes);
        await _dbContext.Modules.AddAsync(module);
        return module.Id;
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update([RequiredGuid] Guid id, ModuleResponse request)
    {
        var module = await _repository.GetModuleByIdAsync(id);
        if (module == null) return NotFound($"没有Id={id}的Module");
        module.ChangeModelId(request.ModelId).ChangeName(request.Name.ToUpper()).ChangeSpecialNotes(request.SpecialNotes);
        return Ok();
    }
    [HttpPut("Release/{id}")]
    public async Task<ActionResult> ReleaseModule([RequiredGuid] Guid id, bool isReleased)
    {
        var module = await _repository.GetModuleByIdAsync(id);
        if (module == null) return NotFound($"没有Id={id}的Module");
        module.ChangeIsReleased(isReleased);
        return Ok();
    }
    [HttpPost("Release/{projectId}")]
    public async Task<ActionResult> ReleaseModuleEvent([RequiredGuid] Guid projectId)
    {
        var eventData = new ModuleReleasedEvent(projectId);
        //发布集成事件
        _eventBus.Publish("ProjectService.Module.Released", eventData);
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