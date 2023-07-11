using AutoMapper;
using Compass.PlanService.Domain;
using Compass.PlanService.Domain.Entities;
using Compass.PlanService.Infrastructure;
using Compass.Wasm.Shared;
using Compass.Wasm.Shared.Plans;

namespace Compass.Wasm.Server.Services.Plans;

public interface IPackingItemService : IBaseService<PackingItemDto>
{
    Task<ApiResponse<PackingItemDto>> AddPalletAsync(PackingItemDto dto);
    Task<ApiResponse<PackingItemDto>> UpdatePalletAsync(Guid id, PackingItemDto dto);
    Task<ApiResponse<PackingItemDto>> UpdatePalletNumberAsync(Guid id, object obj);
}

public class PackingItemService:IPackingItemService
{
    #region ctor
    private readonly PlanDbContext _dbContext;
    private readonly IPlanRepository _repository;
    private readonly IMapper _mapper;
    public PackingItemService(PlanDbContext dbContext, IPlanRepository repository, IMapper mapper)
    {
        _dbContext = dbContext;
        _repository = repository;
        _mapper = mapper;
    }
    #endregion

    #region 基本增删改查
    public async Task<ApiResponse<List<PackingItemDto>>> GetAllAsync()
    {
        try
        {
            var models = await _repository.GetPackingItemsAsync();
            var orderModels = models.OrderByDescending(x => x.CreationTime);
            var dtos = await _mapper.ProjectTo<PackingItemDto>(orderModels).ToListAsync();
            return new ApiResponse<List<PackingItemDto>> { Status = true, Result = dtos };
        }
        catch (Exception e)
        {
            return new ApiResponse<List<PackingItemDto>> { Status = false, Message = e.Message };
        }
    }

    public async Task<ApiResponse<PackingItemDto>> GetSingleAsync(Guid id)
    {
        try
        {
            var model = await _repository.GetPackingItemByIdAsync(id);
            if (model == null) return new ApiResponse<PackingItemDto> { Status = false, Message = "查询数据失败" };
            var dto = _mapper.Map<PackingItemDto>(model);
            return new ApiResponse<PackingItemDto> { Status = true, Result = dto };
        }
        catch (Exception e)
        {
            return new ApiResponse<PackingItemDto> { Status = false, Message = e.Message };
        }
    }

    public async Task<ApiResponse<PackingItemDto>> AddAsync(PackingItemDto dto)
    {
        try
        {
            var model = new PackingItem(Guid.NewGuid(), dto.PackingListId.Value, dto.MtlNumber, dto.Description, dto.EnDescription, dto.Type,dto.Quantity,dto.Unit,dto.Length,dto.Width,dto.Height,dto.Material,dto.Remark,dto.CalcRule,dto.Pallet,dto.NoLabel,dto.OneLabel,dto.Order);
            await _dbContext.PackingItems.AddAsync(model);
            dto.Id = model.Id;
            return new ApiResponse<PackingItemDto> { Status = true, Result = dto };
        }
        catch (Exception e)
        {
            return new ApiResponse<PackingItemDto> { Status = false, Message = e.Message };
        }
    }

    public async Task<ApiResponse<PackingItemDto>> UpdateAsync(Guid id, PackingItemDto dto)
    {
        try
        {
            var model = await _repository.GetPackingItemByIdAsync(id);
            if (model == null) return new ApiResponse<PackingItemDto> { Status = false, Message = "更新数据失败" };
            model.Update(dto);
            return new ApiResponse<PackingItemDto> { Status = true, Result = dto };
        }
        catch (Exception e)
        {
            return new ApiResponse<PackingItemDto> { Status = false, Message = e.Message };
        }
    }

    public async Task<ApiResponse<PackingItemDto>> DeleteAsync(Guid id)
    {
        try
        {
            var model = await _repository.GetPackingItemByIdAsync(id);
            if (model == null) return new ApiResponse<PackingItemDto> { Status = false, Message = "删除数据失败" };
            model.SoftDelete();//软删除
            return new ApiResponse<PackingItemDto> { Status = true };
        }
        catch (Exception e)
        {
            return new ApiResponse<PackingItemDto> { Status = false, Message = e.Message };
        }
    }
    #endregion


    #region 扩展新增和更新Pallet
    /// <summary>
    /// 生成装箱清单号
    /// </summary>
    public async Task<ApiResponse<PackingItemDto>> UpdatePalletNumberAsync(Guid id, object obj)
    {
        try
        {
            var model = await _repository.GetPackingItemsByListIdAsync(id);
            if (model == null) return new ApiResponse<PackingItemDto> { Status = false, Message = "更新数据失败" };
            var pallets = model.Where(x => x.Pallet && x.Type != "托盘").OrderBy(x=>x.Order).ThenBy(x=>x.MtlNumber);
            var i = 1;
            foreach (var item in pallets)
            {
                item.ChangePalletNumber(i.ToString());
                i++;
            }
            return new ApiResponse<PackingItemDto> { Status = true, Result = null };
        }
        catch (Exception e)
        {
            return new ApiResponse<PackingItemDto> { Status = false, Message = e.Message };
        }
    }

    public async Task<ApiResponse<PackingItemDto>> AddPalletAsync(PackingItemDto dto)
    {
        try
        {
            var model = new PackingItem(Guid.NewGuid(), dto.PackingListId.Value, dto.MtlNumber, dto.PalletNumber, dto.PalletLength, dto.PalletWidth, dto.PalletHeight, dto.GrossWeight, dto.NetWeight, dto.PalletRemark);
            await _dbContext.PackingItems.AddAsync(model);
            dto.Id = model.Id;
            return new ApiResponse<PackingItemDto> { Status = true, Result = dto };
        }
        catch (Exception e)
        {
            return new ApiResponse<PackingItemDto> { Status = false, Message = e.Message };
        }
    }
    public async Task<ApiResponse<PackingItemDto>> UpdatePalletAsync(Guid id, PackingItemDto dto)
    {
        try
        {
            var model = await _repository.GetPackingItemByIdAsync(id);
            if (model == null) return new ApiResponse<PackingItemDto> { Status = false, Message = "更新数据失败" };
            model.UpdatePallet(dto);
            return new ApiResponse<PackingItemDto> { Status = true, Result = dto };
        }
        catch (Exception e)
        {
            return new ApiResponse<PackingItemDto> { Status = false, Message = e.Message };
        }
    } 
    #endregion
}