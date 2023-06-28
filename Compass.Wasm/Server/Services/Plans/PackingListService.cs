using AutoMapper;
using Compass.PlanService.Domain;
using Compass.PlanService.Domain.Entities;
using Compass.PlanService.Infrastructure;
using Compass.Wasm.Shared;
using Compass.Wasm.Shared.Params;
using Compass.Wasm.Shared.Plans;

namespace Compass.Wasm.Server.Services.Plans;

public interface IPackingListService : IBaseService<PackingListDto>
{
    Task<ApiResponse<PackingListDto>> GetSingleByProjectIdAndBathAsync(PackingListParam param);
    Task<ApiResponse<PackingListDto>> AddByProjectIdAndBathAsync(PackingListDto dto);
}

public class PackingListService:IPackingListService
{
    #region ctor
    private readonly PlanDbContext _dbContext;
    private readonly IPlanRepository _repository;
    private readonly IMapper _mapper;
    public PackingListService(PlanDbContext dbContext, IPlanRepository repository, IMapper mapper)
    {
        _dbContext = dbContext;
        _repository = repository;
        _mapper = mapper;
    }
    #endregion

    #region 基本增删改查
    public async Task<ApiResponse<List<PackingListDto>>> GetAllAsync()
    {
        try
        {
            var models = await _repository.GetPackingListsAsync();
            var orderModels = models.OrderByDescending(x => x.CreationTime);
            var dtos = await _mapper.ProjectTo<PackingListDto>(orderModels).ToListAsync();
            return new ApiResponse<List<PackingListDto>> { Status = true, Result = dtos };
        }
        catch (Exception e)
        {
            return new ApiResponse<List<PackingListDto>> { Status = false, Message = e.Message };
        }
    }

    public async Task<ApiResponse<PackingListDto>> GetSingleAsync(Guid id)
    {
        try
        {
            var model = await _repository.GetPackingListByIdAsync(id);
            if (model == null) return new ApiResponse<PackingListDto> { Status = false, Message = "查询数据失败" };
            var dto = _mapper.Map<PackingListDto>(model);
            return new ApiResponse<PackingListDto> { Status = true, Result = dto };
        }
        catch (Exception e)
        {
            return new ApiResponse<PackingListDto> { Status = false, Message = e.Message };
        }
    }

    public async Task<ApiResponse<PackingListDto>> AddAsync(PackingListDto dto)
    {
        try
        {
            var model = new PackingList(Guid.NewGuid(), dto.MainPlanId.Value, dto.Product, dto.PackingType, dto.deliveryType, dto.AssyPath);
            await _dbContext.PackingLists.AddAsync(model);
            dto.Id = model.Id;
            return new ApiResponse<PackingListDto> { Status = true, Result = dto };
        }
        catch (Exception e)
        {
            return new ApiResponse<PackingListDto> { Status = false, Message = e.Message };
        }
    }

    public async Task<ApiResponse<PackingListDto>> UpdateAsync(Guid id, PackingListDto dto)
    {
        try
        {
            var model = await _repository.GetPackingListByIdAsync(id);
            if (model == null) return new ApiResponse<PackingListDto> { Status = false, Message = "更新数据失败" };
            model.Update(dto);
            return new ApiResponse<PackingListDto> { Status = true, Result = dto };
        }
        catch (Exception e)
        {
            return new ApiResponse<PackingListDto> { Status = false, Message = e.Message };
        }
    }

    public async Task<ApiResponse<PackingListDto>> DeleteAsync(Guid id)
    {
        try
        {
            var model = await _repository.GetPackingListByIdAsync(id);
            if (model == null) return new ApiResponse<PackingListDto> { Status = false, Message = "删除数据失败" };
            model.SoftDelete();//软删除
            return new ApiResponse<PackingListDto> { Status = true };
        }
        catch (Exception e)
        {
            return new ApiResponse<PackingListDto> { Status = false, Message = e.Message };
        }
    }



    #endregion

    #region 扩展查询功能
    public async Task<ApiResponse<PackingListDto>> GetSingleByProjectIdAndBathAsync(PackingListParam param)
    {
        try
        {
            var mainPlan = await _repository.GetMainPlanByProjectIdAndBatchAsync(param.ProjectId.Value, param.Batch.Value);
            if (mainPlan == null) return new ApiResponse<PackingListDto> { Status = false};
            var model = await _repository.GetPackingListByMainPlanIdAsync(mainPlan.Id);

            if (model == null) return new ApiResponse<PackingListDto> { Status = false, Message = "查询数据失败" };
            var dto = _mapper.Map<PackingListDto>(model);
            dto.ProjectId=mainPlan.ProjectId.Value;
            dto.Batch=mainPlan.Batch;
            dto.ProjectName=$"{mainPlan.Number}-{mainPlan.Name}";
            //查询PackingItem列表
            var packingItems = await _repository.GetPackingItemsByListIdAsync(dto.Id.Value);
            var piDtos = await _mapper.ProjectTo<PackingItemDto>(packingItems).ToListAsync();
            dto.PackingItemDtos =piDtos;

            return new ApiResponse<PackingListDto> { Status = true, Result = dto };
        }
        catch (Exception e)
        {
            return new ApiResponse<PackingListDto> { Status = false, Message = e.Message };
        }
    }

    public async Task<ApiResponse<PackingListDto>> AddByProjectIdAndBathAsync(PackingListDto dto)
    {
        var mainPlan = await _repository.GetMainPlanByProjectIdAndBatchAsync(dto.ProjectId.Value, dto.Batch.Value);
            dto.MainPlanId=mainPlan.Id;
        return await AddAsync(dto);
    }
    #endregion
}