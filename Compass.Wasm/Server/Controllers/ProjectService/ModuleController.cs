using AutoMapper;
using Compass.Wasm.Shared.ProjectService;
using System.ComponentModel.DataAnnotations;
using Compass.DataService.Domain;
using Compass.Wasm.Server.ProjectService.ModuleEvent;

namespace Compass.Wasm.Server.Controllers.ProjectService;

[Route("api/[controller]")]
[ApiController]
[UnitOfWork(typeof(ProjectDbContext))]
//[Authorize(Roles = "admin,pm,designer")]
public class ModuleController : ControllerBase
{
    private readonly ProjectDomainService _domainService;
    private readonly ProjectDbContext _dbContext;
    private readonly IProjectRepository _repository;
    private readonly IMapper _mapper;
    private readonly IEventBus _eventBus;
    private readonly IdentityUserManager _userManager;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IDataRepository _dataRepository;

    public ModuleController(ProjectDomainService domainService, ProjectDbContext dbContext, IProjectRepository repository, IMapper mapper, IEventBus eventBus,IdentityUserManager userManager,ICategoryRepository categoryRepository,IDataRepository dataRepository)
    {
        _domainService = domainService;
        _dbContext = dbContext;
        _repository = repository;
        _mapper = mapper;
        _eventBus = eventBus;
        _userManager = userManager;
        _categoryRepository = categoryRepository;
        _dataRepository = dataRepository;
    }

    [HttpGet("All/{drawingId}")]
    public async Task<ModuleResponse[]> FindAll([RequiredGuid] Guid drawingId)
    {
        return await _mapper.ProjectTo<ModuleResponse>(await _repository.GetModulesByDrawingIdAsync(drawingId)).ToArrayAsync();
    }
    [HttpGet("Project/{projectId}")]
    public async Task<List<ModuleResponse>> FindAllByProject([RequiredGuid] Guid projectId)
    {
        List<ModuleResponse> modules = new List<ModuleResponse>();
        var drawings = await _repository.GetDrawingsByProjectIdAsync(projectId);
        foreach (var drawing in drawings)
        {
            modules.AddRange(_mapper.ProjectTo<ModuleResponse>(await _repository.GetModulesByDrawingIdAsync(drawing.Id)));
        }
        return modules;
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<ModuleResponse?>> FindById([RequiredGuid] Guid id)
    {
        var module = await _repository.GetModuleByIdAsync(id);
        if (module == null) return NotFound($"没有Id={id}的Module");
        ModuleResponse response = _mapper.Map<ModuleResponse>(module);
        //同时查询ModuleData
        var moduleData = await _dataRepository.GetModuleDataByIdAsync(id);
        if (moduleData != null)
        {
            response.Length = moduleData.Length;
            response.Width = moduleData.Width;
            response.Height = moduleData.Height;
        }
        return response;
    }

    [HttpGet("Exists/{drawingId}")]
    public async Task<ActionResult<bool>> ModuleExists([RequiredGuid] Guid drawingId)
    {
        return await _repository.ModuleExistsInDrawing(drawingId);
    }

    [HttpGet("DrawingUrl/{id}")]
    public async Task<string?> GetDrawingUrl([Required] Guid id)
    {
        return await _repository.GetDrawingUrlByModuleIdAsync(id);
    }

    [HttpPost("Add")]
    public async Task<ActionResult<Guid>> Add(AddModuleRequest request)
    {
        var module = new Compass.ProjectService.Domain.Entities.Module(Guid.NewGuid(), request.DrawingId, request.ModelTypeId, request.Name.ToUpper(), request.SpecialNotes);
        await _dbContext.Modules.AddAsync(module);

        //todo:发出集成事件，创建Module的参数
       var modelName=await _categoryRepository.GetModelNameByModelTypeIdAsync(request.ModelTypeId);
       var eventData = new ModuleCreatedEvent(module.Id, modelName, request.ModelTypeId, request.Length, request.Width,
           request.Height);
        _eventBus.Publish("ProjectService.Module.Created", eventData);

        return module.Id;
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update([RequiredGuid] Guid id, ModuleResponse request)
    {
        var module = await _repository.GetModuleByIdAsync(id);
        if (module == null) return NotFound($"没有Id={id}的Module");
        module.ChangeModelTypeId(request.ModelTypeId).ChangeName(request.Name.ToUpper()).ChangeSpecialNotes(request.SpecialNotes);
        //todo:发出集成事件，修改Module的参数
        var modelName = await _categoryRepository.GetModelNameByModelTypeIdAsync(request.ModelTypeId);
        var eventData = new ModuleUpdatedEvent(module.Id, modelName, request.ModelTypeId,request.OldModelTypeId, request.Length, request.Width, request.Height);
        _eventBus.Publish("ProjectService.Module.Updated", eventData);
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
    public Task<ActionResult> ReleaseModuleEvent([RequiredGuid] Guid projectId)
    {
        var eventData = new ModuleReleasedEvent(projectId);
        //发布集成事件
        _eventBus.Publish("ProjectService.Module.Released", eventData);
        return Task.FromResult<ActionResult>(Ok());
    }


    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete([RequiredGuid] Guid id)
    {
        var module = await _repository.GetModuleByIdAsync(id);
        if (module == null) return NotFound($"没有Id={id}的Module");
        //这样做仍然是幂等的，因为“调用N次，确保服务器处于与第一次调用相同的状态。”与响应无关
        module.SoftDelete();//软删除
        //todo:发出集成事件，删除Module的参数



        return Ok();
    }
}