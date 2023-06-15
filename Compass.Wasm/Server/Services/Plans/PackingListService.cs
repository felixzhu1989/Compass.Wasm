using AutoMapper;
using Compass.PlanService.Domain;
using Compass.PlanService.Domain.Entities;
using Compass.PlanService.Infrastructure;
using Compass.Wasm.Shared;
using Compass.Wasm.Shared.Plans;
using DocumentFormat.OpenXml.Office2010.Excel;

namespace Compass.Wasm.Server.Services.Plans;

public interface IPackingListService : IBaseService<PackingListDto>
{
    Task<ApiResponse<PackingListDto>> GetSingleByMainPlanIdAndBathAsync(PackingListDto listDto);
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
            var model = new PackingList(Guid.NewGuid(), dto.MainPlanId, dto.Product, dto.PackingType, dto.deliveryType, dto.AssyPath);
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

    public async Task<ApiResponse<PackingListDto>> GetSingleByMainPlanIdAndBathAsync(PackingListDto listDto)
    {
        try
        {
            var mainPlan = await _repository.GetMainPlanByProjectIdAndBatchAsync(listDto.ProjectId,listDto.Batch);
            var model = await _repository.GetPackingListByMainPlanIdAsync(mainPlan.Id);

            if (model == null) return new ApiResponse<PackingListDto> { Status = false, Message = "查询数据失败" };
            var dto = _mapper.Map<PackingListDto>(model);
            dto.ProjectId=listDto.ProjectId;
            dto.Batch=listDto.Batch;
            dto.ProjectName=$"{mainPlan}-{mainPlan.Name}";
            return new ApiResponse<PackingListDto> { Status = true, Result = dto };
        }
        catch (Exception e)
        {
            return new ApiResponse<PackingListDto> { Status = false, Message = e.Message };
        }
    }




    #endregion

}