using System.Security.AccessControl;
using AutoMapper;
using Compass.PlanService.Domain;
using Compass.PlanService.Domain.Entities;
using Compass.PlanService.Infrastructure;
using Compass.PlanService.Infrastructure.Migrations;
using Compass.Wasm.Server.Events.Plans;
using Compass.Wasm.Shared;
using Compass.Wasm.Shared.Plans;

namespace Compass.Wasm.Server.Services.Plans;
public interface IMainPlanService : IBaseService<MainPlanDto>
{
    //扩展查询
    Task<ApiResponse<MainPlanDto>> UpdateStatusesAsync(Guid id, MainPlanDto dto);
    //GetIndexDataAsync
    Task<ApiResponse<List<MainPlanDto>>> GetIndexDataAsync();
    Task<ApiResponse<List<MainPlanDto>>> GetAllByProjectIdAsync(Guid projectId);
}
public class MainPlanService : IMainPlanService
{
    private readonly PlanDomainService _domainService;
    private readonly IIssueService _issueService;
    private readonly PlanDbContext _dbContext;
    private readonly IPlanRepository _repository;
    private readonly IMapper _mapper;
    private readonly IEventBus _eventBus;
    private readonly IProjectRepository _projectRepository;

    public MainPlanService(PlanDomainService domainService,IIssueService issueService, PlanDbContext dbContext, IPlanRepository repository, IMapper mapper, IEventBus eventBus, IProjectRepository projectRepository)
    {
        _domainService = domainService;
        _issueService = issueService;
        _dbContext = dbContext;
        _repository = repository;
        _mapper = mapper;
        _eventBus = eventBus;
        _projectRepository = projectRepository;
    }

    #region 基本增删改查
    public async Task<ApiResponse<List<MainPlanDto>>> GetAllAsync()
    {
        try
        {
            var models = await _repository.GetMainPlansAsync();
            var orderModels = models.OrderByDescending(x => x.FinishTime);
            var dtos = await _mapper.ProjectTo<MainPlanDto>(orderModels).ToListAsync();
            foreach (var dto in dtos)
            {
                await GetIssues(dto);
            }
            return new ApiResponse<List<MainPlanDto>> { Status = true, Result = dtos };
        }
        catch (Exception e)
        {
            return new ApiResponse<List<MainPlanDto>> { Status = false, Message = e.Message };
        }
    }

    public async Task<ApiResponse<MainPlanDto>> GetSingleAsync(Guid id)
    {
        try
        {
            var model = await _repository.GetMainPlanByIdAsync(id);
            if (model == null) return new ApiResponse<MainPlanDto> { Status = false, Message = "查询数据失败" };
            var dto = _mapper.Map<MainPlanDto>(model);
            await GetIssues(dto);
            return new ApiResponse<MainPlanDto> { Status = true, Result = dto };
        }
        catch (Exception e)
        {
            return new ApiResponse<MainPlanDto> { Status = false, Message = e.Message };
        }
    }

    public async Task<ApiResponse<MainPlanDto>> AddAsync(MainPlanDto dto)
    {
        try
        {
            var model = new MainPlan(Guid.NewGuid(), dto.CreateTime, dto.Number, dto.Name, dto.Quantity, dto.ModelSummary, dto.FinishTime, dto.DrwReleaseTarget, dto.MonthOfInvoice, dto.MainPlanType, dto.Remarks,dto.Batch,dto.ItemLine,dto.Workload,dto.Value);
            await _dbContext.MainPlans.AddAsync(model);
            dto.Id = model.Id;

            //Todo:发出集成事件，绑定潜在的项目
            var eventData = new MainPlanCreatedEvent(dto.Id.Value, dto.Name);
            //发布集成事件
            _eventBus.Publish("PlanService.MainPlan.Created", eventData);

            return new ApiResponse<MainPlanDto> { Status = true, Result = dto };
        }
        catch (Exception e)
        {
            return new ApiResponse<MainPlanDto> { Status = false, Message = e.Message };
        }
    }

    public async Task<ApiResponse<MainPlanDto>> UpdateAsync(Guid id, MainPlanDto dto)
    {
        try
        {
            var model = await _repository.GetMainPlanByIdAsync(id);
            if (model == null) return new ApiResponse<MainPlanDto> { Status = false, Message = "更新数据失败" };
            model.Update(dto);
            return new ApiResponse<MainPlanDto> { Status = true, Result = dto };
        }
        catch (Exception e)
        {
            return new ApiResponse<MainPlanDto> { Status = false, Message = e.Message };
        }
    }

    public async Task<ApiResponse<MainPlanDto>> DeleteAsync(Guid id)
    {
        try
        {
            var model = await _repository.GetMainPlanByIdAsync(id);
            if (model == null) return new ApiResponse<MainPlanDto> { Status = false, Message = "删除数据失败" };
            model.SoftDelete();//软删除
            return new ApiResponse<MainPlanDto> { Status = true };
        }
        catch (Exception e)
        {
            return new ApiResponse<MainPlanDto> { Status = false, Message = e.Message };
        }
    }


    #endregion


    #region 扩展
    public async Task<ApiResponse<MainPlanDto>> UpdateStatusesAsync(Guid id, MainPlanDto dto)
    {
        try
        {
            var model = await _repository.GetMainPlanByIdAsync(id);
            if (model == null) return new ApiResponse<MainPlanDto> { Status = false, Message = "更新数据失败" };
            model.UpdateStatuses(dto);
            return new ApiResponse<MainPlanDto> { Status = true, Result = dto };
        }
        catch (Exception e)
        {
            return new ApiResponse<MainPlanDto> { Status = false, Message = e.Message };
        }
    }

    public async Task<ApiResponse<List<MainPlanDto>>> GetIndexDataAsync()
    {
        try
        {
            var models = await _repository.GetMainPlansAsync();
            var orderModels = models.OrderByDescending(x => x.FinishTime);
            var dtos = await _mapper.ProjectTo<MainPlanDto>(orderModels).ToListAsync();
            foreach (var dto in dtos)
            {
                await GetIssues(dto);
            }
            return new ApiResponse<List<MainPlanDto>> { Status = true, Result = dtos };
        }
        catch (Exception e)
        {
            return new ApiResponse<List<MainPlanDto>> { Status = false, Message = e.Message };
        }
    }

    public async Task<ApiResponse<List<MainPlanDto>>> GetAllByProjectIdAsync(Guid projectId)
    {
        try
        {
            var models = await _repository.GetMainPlansByProjectIdAsync(projectId);
            var dtos = await _mapper.ProjectTo<MainPlanDto>(models).ToListAsync();
            foreach (var dto in dtos)
            {
                await GetIssues(dto);
            }
            return new ApiResponse<List<MainPlanDto>> { Status = true, Result = dtos };
        }
        catch (Exception e)
        {
            return new ApiResponse<List<MainPlanDto>> { Status = false, Message = e.Message };
        }
    }

    #endregion

    private async Task GetIssues(MainPlanDto dto)
    {
        dto.IssueDtos = (await _issueService.GetAllByMainPlanIdAsync(dto.Id.Value)).Result;
        dto.AllIssueClosed = true;
        foreach (var issueDto in dto.IssueDtos)
        {
            dto.AllIssueClosed = dto.AllIssueClosed && issueDto.IsClosed;
        }
    }


}