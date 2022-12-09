using AutoMapper;
using Compass.PlanService.Domain;
using Compass.PlanService.Infrastructure;
using Compass.Wasm.Shared.PlanService;
using System.ComponentModel.DataAnnotations;
using Compass.PlanService.Domain.Entities;
using Compass.Wasm.Shared.ProjectService;

namespace Compass.Wasm.Server.Controllers.PlanService;

[Route("api/[controller]")]
[ApiController]
[UnitOfWork(typeof(PSDbContext))]
//[Authorize(Roles = "admin,pmc")]
public class ProductionPlanController : ControllerBase
{
    private readonly PSDomainService _domainService;
    private readonly PSDbContext _dbContext;
    private readonly IPSRepository _repository;
    private readonly IMapper _mapper;
    private readonly IEventBus _eventBus;
    private readonly IPMRepository _pmRepository;

    public ProductionPlanController(PSDomainService domainService, PSDbContext dbContext, IPSRepository repository, IMapper mapper, IEventBus eventBus, IPMRepository pmRepository)
    {
        _domainService = domainService;
        _dbContext = dbContext;
        _repository = repository;
        _mapper = mapper;
        _eventBus = eventBus;
        _pmRepository = pmRepository;
    }
    [HttpGet("{year}/{month}/{planType}")]
    public async Task<List<ProductionPlanResponse>> FindByMonthAndClass(int year, int month, ProductionPlanType planType)
    {
        return await _mapper.ProjectTo<ProductionPlanResponse>(await _repository.GetProductionPlansAsync(year, month, planType)).ToListAsync();
    }
    [HttpGet("Unbind")]
    public async Task<List<ProductionPlanResponse>> FindByUnbindStatus()
    {
        return await _mapper.ProjectTo<ProductionPlanResponse>(await _repository.GetUnbindProductionPlansAsync()).ToListAsync();
    }
    [HttpGet("UnbindProjects")]
    public async Task<List<ProjectResponse>> FindUnbindProjects()
    {
        var projectIds = await _repository.GetBoundProductionPlansAsync();
        var unbindProjects = await _pmRepository.GetUnbindProjectsAsync(projectIds);
        return await _mapper.ProjectTo<ProjectResponse>(unbindProjects).ToListAsync();
    }
    [HttpGet("{id}")]
    public async Task<ActionResult<ProductionPlanResponse?>> FindById([RequiredGuid] Guid id)
    {
        //返回ValueTask的需要await的一下
        var productionPlan = await _repository.GetProductionPlanByIdAsync(id);
        if (productionPlan == null) return NotFound($"没有Id={id}的ProductionPlan");
        return _mapper.Map<ProductionPlanResponse>(productionPlan);
    }
    [HttpGet("ProjectId/{projectId}")]
    public async Task<ActionResult<ProductionPlanResponse?>> FindByProjectId([RequiredGuid] Guid projectId)
    {
        //返回ValueTask的需要await的一下
        var productionPlan = await _repository.GetProductionPlanByProjectIdAsync(projectId);
        if (productionPlan == null) return NotFound($"没有ProjectId={projectId}的ProductionPlan");
        return _mapper.Map<ProductionPlanResponse>(productionPlan);
    }

    [HttpPost("Add")]
    public async Task<ActionResult<Guid>> Add(AddProductionPlanRequest request)
    {
        var plan = new ProductionPlan(Guid.NewGuid(), request.OdpReleaseTime, request.SqNumber, request.Name, request.Quantity, request.ModelSummary, request.ProductionFinishTime, request.DrawingReleaseTarget, request.MonthOfInvoice, request.ProductionPlanType, request.Remarks);
        await _dbContext.ProductionPlans.AddAsync(plan);
        //Todo:是否需要
        //var eventData = new ProjectCreatedEvent(plan.Id, plan.DeliveryDate);
        ////发布集成事件
        //_eventBus.Publish("ProjectService.Project.Created", eventData);

        return plan.Id;
    }



    [HttpPut("BindProject")]
    public async Task<ActionResult> BindProject(ProductionPlanResponse request)
    {
        var prodPlan = await _repository.GetProductionPlanByIdAsync(request.Id);
        if (prodPlan == null) return NotFound($"没有Id={request.Id}的ProductionPlan");
        prodPlan.ChangeProjectId(request.ProjectId);
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete([RequiredGuid] Guid id)
    {
        var plan = await _repository.GetProductionPlanByIdAsync(id);
        //这样做仍然是幂等的，因为“调用N次，确保服务器处于与第一次调用相同的状态。”与响应无关
        if (plan == null) return NotFound($"没有Id={id}的ProductionPlan");
        plan.SoftDelete();//软删除
        //todo:是否需要发布集成事件
        //var eventData = new ProjectDeletedEvent(plan.Id);
        ////发布集成事件
        //_eventBus.Publish("ProjectService.Project.Deleted", eventData);
        return Ok();
    }
}