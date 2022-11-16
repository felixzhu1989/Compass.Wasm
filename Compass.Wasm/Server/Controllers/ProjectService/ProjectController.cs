using Compass.Wasm.Client.ProjectService;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Compass.Wasm.Server.ProjectService;
using Compass.Wasm.Shared.ProjectService;
using Zack.EventBus;

namespace Compass.Wasm.Server.Controllers.ProjectService;

[Route("api/[controller]")]
[ApiController]
[UnitOfWork(typeof(PMDbContext))]
//[Authorize(Roles = "admin,pm,designer")]
public class ProjectController : ControllerBase
{
    private readonly PMDomainService _domainService;
    private readonly PMDbContext _dbContext;
    private readonly IPMRepository _repository;
    private readonly IMapper _mapper;
    private readonly IEventBus _eventBus;

    public ProjectController(PMDomainService domainService, PMDbContext dbContext, IPMRepository repository, IMapper mapper,IEventBus eventBus)
    {
        _domainService = domainService;
        _dbContext = dbContext;
        _repository = repository;
        _mapper = mapper;
        _eventBus = eventBus;
    }
    [HttpGet("All")]
    public async Task<ProjectResponse[]> FindAll()
    {
        //使用AutoMapper将Project转换成ProjectResponse（Dto）
        return await _mapper.ProjectTo<ProjectResponse>(await _repository.GetProjectsAsync()).ToArrayAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProjectResponse?>> FindById([RequiredGuid] Guid id)
    {
        //返回ValueTask的需要await的一下
        var project = await _repository.GetProjectByIdAsync(id);
        if (project == null) return NotFound($"没有Id={id}的Project");
        return _mapper.Map<ProjectResponse>(project);
    }

    [HttpGet("Odp/{odpNumber}")]
    public async Task<ActionResult<ProjectResponse?>> FindByOdp(string odpNumber)
    {
        var project = await _repository.GetProjectByOdpAsync(odpNumber.ToUpper());
        if (project == null) return NotFound($"没有Id={odpNumber.ToUpper()}的Project");
        return _mapper.Map<ProjectResponse>(project);
    }

    [HttpPost("Add")]
    public async Task<ActionResult<Guid>> Add(AddProjectRequest request)
    {
        var project = new Project(Guid.NewGuid(), request.OdpNumber.ToUpper(), request.Name,request.ReceiveDate,request.DeliveryDate,request.ProjectType, request.RiskLevel, request.SpecialNotes);
        //包括合同地址
        project.ChangeContractUrl(request.ContractUrl);
        await _dbContext.Projects.AddAsync(project);
        var eventData =new ProjectCreatedEvent(project.Id);
        //发布集成事件
        _eventBus.Publish("ProjectService.Project.Created", eventData);

        return project.Id;
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update([RequiredGuid] Guid id, ProjectResponse request)
    {
        var project = await _repository.GetProjectByIdAsync(id);
        if (project == null) return NotFound($"没有Id={id}的Project");
        //包括合同地址和Bom地址
        project.ChangeOdpNumber(request.OdpNumber.ToUpper()).ChangeName(request.Name)
            .ChangeReceiveDate(request.ReceiveDate).ChangeDeliveryDate(request.DeliveryDate)
            .ChangeProjectType(request.ProjectType).ChangeRiskLevel(request.RiskLevel)
            .ChangeContractUrl(request.ContractUrl).ChangeBomUrl(request.BomUrl)
            .ChangeAttachmentsUrl(request.AttachmentsUrl)
            .ChangeSpecialNotes(request.SpecialNotes);
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete([RequiredGuid] Guid id)
    {
        var project = await _repository.GetProjectByIdAsync(id);
        //这样做仍然是幂等的，因为“调用N次，确保服务器处于与第一次调用相同的状态。”与响应无关
        if (project == null) return NotFound($"没有Id={id}的Project");
        project.SoftDelete();//软删除
        return Ok();
    }
}