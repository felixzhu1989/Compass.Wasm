using AutoMapper;
using Compass.PlanService.Domain;
using Compass.PlanService.Infrastructure;
using Compass.Wasm.Shared.PlanService;
using System.ComponentModel.DataAnnotations;
using Compass.PlanService.Domain.Entities;
using Compass.Wasm.Shared.ProjectService;
using Compass.Wasm.Server.PlanService.ProductionPlanEvent;
using Compass.Wasm.Shared.PlanService.Notification;

namespace Compass.Wasm.Server.Controllers.PlanService;

[Route("api/[controller]")]
[ApiController]
[UnitOfWork(typeof(PlanDbContext))]
//[Authorize(Roles = "admin,pmc")]
public class ProductionPlanController : ControllerBase
{
    private readonly PlanDomainService _domainService;
    private readonly PlanDbContext _dbContext;
    private readonly IPlanRepository _repository;
    private readonly IMapper _mapper;
    private readonly IEventBus _eventBus;
    private readonly IProjectRepository _pmRepository;

    public ProductionPlanController(PlanDomainService domainService, PlanDbContext dbContext, IPlanRepository repository, IMapper mapper, IEventBus eventBus, IProjectRepository pmRepository)
    {
        _domainService = domainService;
        _dbContext = dbContext;
        _repository = repository;
        _mapper = mapper;
        _eventBus = eventBus;
        _pmRepository = pmRepository;
    }
    [HttpGet("{year}/{month}/{planType}")]
    public async Task<List<ProductionPlanResponse>> FindByMonthAndClass(int year, int month, ProductionPlanType_e planType)
    {
        return await _mapper.ProjectTo<ProductionPlanResponse>(await _repository.GetProductionPlansAsync(year, month, planType)).ToListAsync();
    }
    [HttpGet("{year}/{planType}")]
    public async Task<List<ProductionPlanResponse>> FindByYearAndClass(int year, ProductionPlanType_e planType)
    {
        return await _mapper.ProjectTo<ProductionPlanResponse>(await _repository.GetProductionPlansAsync(year, planType)).ToListAsync();
    }
    [HttpGet("Unbind")]
    public async Task<List<ProductionPlanResponse>> FindByUnbindStatus()
    {
        return await _mapper.ProjectTo<ProductionPlanResponse>(await _repository.GetUnbindProductionPlansAsync()).ToListAsync();
    }
    [HttpGet("UnbindProjects")]
    public async Task<List<ProjectDto>> FindUnbindProjects()
    {
        var projectIds = await _repository.GetBoundProductionPlansAsync();
        var unbindProjects = await _pmRepository.GetUnbindProjectsAsync(projectIds);
        return await _mapper.ProjectTo<ProjectDto>(unbindProjects).ToListAsync();
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

    [HttpGet("CycleTime/{year}/{month}")]
    public async Task<ActionResult<CycleTimeResponse>> GetCycleTimeByMonth(int year, int month)
    {
        return await _repository.GetCycleTimeByMonthAsync(year,month);
    }

    [HttpGet("CycleTime/{year}")]
    public async Task<ActionResult<CycleTimeResponse>> GetCycleTimeByYear(int year)
    {
        return await _repository.GetCycleTimeByYearAsync(year);
    }



    [HttpPost("Add")]
    public async Task<ActionResult<Guid>> Add(AddProductionPlanRequest request)
    {
        var plan = new ProductionPlan(Guid.NewGuid(), request.OdpReleaseTime, request.SqNumber, request.Name, request.Quantity, request.ModelSummary, request.ProductionFinishTime, request.DrawingReleaseTarget, request.MonthOfInvoice, request.ProductionPlanType, request.Remarks);
        await _dbContext.ProductionPlans.AddAsync(plan);

        //Todo:发出集成事件，绑定潜在的项目
        var eventData = new ProductionPlanCreatedEvent(plan.Id, plan.Name);
        //发布集成事件
        _eventBus.Publish("PlanService.ProductionPlan.Created", eventData);

        return plan.Id;
    }
    
    [HttpPut("BindProject")]
    public async Task<ActionResult> BindProject(ProductionPlanResponse request)
    {
        var prodPlan = await _repository.GetProductionPlanByIdAsync(request.Id);
        if (prodPlan == null) return NotFound($"没有Id={request.Id}的ProductionPlan");
        prodPlan.ChangeProjectId(request.ProjectId);
        if (request.ProjectId != null)
        {
            //发出集成事件
            var eventData = new BindProjectEvent(request.ProjectId, prodPlan.ProductionFinishTime);
            _eventBus.Publish("PlanService.ProductionPlan.BindProject", eventData);
        }
        return Ok();
    }
    [HttpPut("DrawingReleaseActual")]
    public async Task<ActionResult> DrawingReleaseActual(ProductionPlanResponse request)
    {
        var prodPlan = await _repository.GetProductionPlanByIdAsync(request.Id);
        if (prodPlan == null) return NotFound($"没有Id={request.Id}的ProductionPlan");
        prodPlan.ChangeDrawingReleaseActual(request.DrawingReleaseActual);
        return Ok();
    }
    [HttpPut("{id}")]
    public async Task<ActionResult> Update([RequiredGuid] Guid id, ProductionPlanResponse request)
    {
        var prodPlan = await _repository.GetProductionPlanByIdAsync(id);
        if (prodPlan == null) return NotFound($"没有Id={id}的ProductionPlan");

        prodPlan.ChangeOdpReleaseTime(request.OdpReleaseTime)
            .ChangeSqNumber(request.SqNumber)
            .ChangeName(request.Name)
            .ChangeQuantity(request.Quantity)
            .ChangeModelSummary(request.ModelSummary)
            .ChangeProductionFinishTime(request.ProductionFinishTime)
            .ChangeDrawingReleaseTarget(request.DrawingReleaseTarget)
            .ChangeMonthOfInvoice(request.MonthOfInvoice)
            .ChangeProductionPlanType(request.ProductionPlanType)
            .ChangeRemarks(request.Remarks);
        if (prodPlan.ProjectId != null)
        {
            //发出集成事件
            var eventData = new BindProjectEvent(prodPlan.ProjectId, prodPlan.ProductionFinishTime);
            _eventBus.Publish("PlanService.ProductionPlan.BindProject", eventData);
        }
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