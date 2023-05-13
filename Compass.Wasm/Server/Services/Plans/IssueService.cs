﻿using Compass.Wasm.Shared;
using Compass.Wasm.Shared.Plans;
using AutoMapper;
using Compass.PlanService.Domain;
using Compass.PlanService.Domain.Entities;
using Compass.PlanService.Infrastructure;

namespace Compass.Wasm.Server.Services.Plans;
public interface IIssueService : IBaseService<IssueDto>
{
    //扩展查询
    Task<ApiResponse<List<IssueDto>>> GetAllByMainPlanIdAsync(Guid mainPlanId);


}

public class IssueService : IIssueService
{
    private readonly PlanDbContext _dbContext;
    private readonly IPlanRepository _repository;
    private readonly IMapper _mapper;

    public IssueService(PlanDomainService domainService, PlanDbContext dbContext, IPlanRepository repository, IMapper mapper, IEventBus eventBus, IdentityUserManager userManager)
    {
        _dbContext = dbContext;
        _repository = repository;
        _mapper = mapper;
    }

    #region 基本增删改查
    public async Task<ApiResponse<List<IssueDto>>> GetAllAsync()
    {
        try
        {
            var models = await _repository.GetIssuesAsync();
            var orderModels = models.OrderByDescending(x => x.CreationTime);
            var dtos = await _mapper.ProjectTo<IssueDto>(orderModels).ToListAsync();
            return new ApiResponse<List<IssueDto>> { Status = true, Result = dtos };
        }
        catch (Exception e)
        {
            return new ApiResponse<List<IssueDto>> { Status = false, Message = e.Message };
        }
    }

    public async Task<ApiResponse<IssueDto>> GetSingleAsync(Guid id)
    {
        try
        {
            var model = await _repository.GetIssueByIdAsync(id);
            if (model == null) return new ApiResponse<IssueDto> { Status = false, Message = "查询数据失败" };
            var dto = _mapper.Map<IssueDto>(model);
            return new ApiResponse<IssueDto> { Status = true, Result = dto };
        }
        catch (Exception e)
        {
            return new ApiResponse<IssueDto> { Status = false, Message = e.Message };
        }
    }

    public async Task<ApiResponse<IssueDto>> AddAsync(IssueDto dto)
    {
        try
        {
            var model = new Issue(Guid.NewGuid(), dto.MainPlanId, dto.ReporterId, dto.Title, dto.Content, dto.ContentUrl);
            await _dbContext.Issues.AddAsync(model);
            dto.Id = model.Id;
            return new ApiResponse<IssueDto> { Status = true, Result = dto };
        }
        catch (Exception e)
        {
            return new ApiResponse<IssueDto> { Status = false, Message = e.Message };
        }
    }

    public async Task<ApiResponse<IssueDto>> UpdateAsync(Guid id, IssueDto dto)
    {
        try
        {
            var model = await _repository.GetIssueByIdAsync(id);
            if (model == null) return new ApiResponse<IssueDto> { Status = false, Message = "更新数据失败" };
            model.Update(dto);
            return new ApiResponse<IssueDto> { Status = true, Result = dto };
        }
        catch (Exception e)
        {
            return new ApiResponse<IssueDto> { Status = false, Message = e.Message };
        }
    }

    public async Task<ApiResponse<IssueDto>> DeleteAsync(Guid id)
    {
        try
        {
            var model = await _repository.GetIssueByIdAsync(id);
            if (model == null) return new ApiResponse<IssueDto> { Status = false, Message = "删除数据失败" };
            model.SoftDelete();//软删除
            return new ApiResponse<IssueDto> { Status = true };
        }
        catch (Exception e)
        {
            return new ApiResponse<IssueDto> { Status = false, Message = e.Message };
        }
    }
    #endregion

    #region 扩展查询功能，Blazor
    public async Task<ApiResponse<List<IssueDto>>> GetAllByMainPlanIdAsync(Guid mainPlanId)
    {
        try
        {
            var models = await _repository.GetIssuesByMainPlanIdAsync(mainPlanId);
            var dtos = await _mapper.ProjectTo<IssueDto>(models).ToListAsync();
            return new ApiResponse<List<IssueDto>> { Status = true, Result = dtos };
        }
        catch (Exception e)
        {
            return new ApiResponse<List<IssueDto>> { Status = false, Message = e.Message };
        }
    }
    #endregion
}