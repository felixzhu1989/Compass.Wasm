using AutoMapper;
using Compass.Dtos;
using Compass.Wasm.Shared;
using Compass.Wasm.Shared.Categories;

namespace Compass.Wasm.Server.Services.Categories;

public interface IMaterialItemService : IBaseService<MaterialItemDto>
{
    //扩展
    Task<ApiResponse<MaterialItemDto>> UpdateInventoryAsync(Guid id, MaterialItemDto dto);
    Task<ApiResponse<MaterialItemDto>> UpdateOtherAsync(Guid id, MaterialItemDto dto);
    Task<ApiResponse<List<MaterialItemDto>>> GetTop50Async();

}

public class MaterialItemService : IMaterialItemService
{
    #region ctor
    private readonly CategoryDomainService _domainService;
    private readonly CategoryDbContext _dbContext;
    private readonly ICategoryRepository _repository;
    private readonly IMapper _mapper;
    public MaterialItemService(CategoryDomainService domainService, CategoryDbContext dbContext, ICategoryRepository repository, IMapper mapper)
    {
        _domainService = domainService;
        _dbContext = dbContext;
        _repository = repository;
        _mapper = mapper;
    } 
    #endregion

    #region 基本增删改查
    public async Task<ApiResponse<List<MaterialItemDto>>> GetAllAsync()
    {
        try
        {
            var models = await _repository.GetMaterialItemsAsync();
            var dtos =await _mapper.ProjectTo<MaterialItemDto>(models).OrderBy(x=>x.Order).ToListAsync();
            return new ApiResponse<List<MaterialItemDto>> { Status = true, Result = dtos };
        }
        catch (Exception e)
        {
            return new ApiResponse<List<MaterialItemDto>> { Status = false, Message = e.Message };
        }
    }

    public async Task<ApiResponse<MaterialItemDto>> GetSingleAsync(Guid id)
    {
        try
        {
            var model = await _repository.GetMaterialItemByIdAsync(id);
            if (model == null) return new ApiResponse<MaterialItemDto> { Status = false, Message = "查询数据失败" };
            var dto = _mapper.Map<MaterialItemDto>(model);
            return new ApiResponse<MaterialItemDto> { Status = true, Result = dto };
        }
        catch (Exception e)
        {
            return new ApiResponse<MaterialItemDto> { Status = false, Message = e.Message };
        }
    }

    public async Task<ApiResponse<MaterialItemDto>> AddAsync(MaterialItemDto dto)
    {
        try
        {
            var model = new MaterialItem(Guid.NewGuid(), dto.MtlNumber,dto.Description,dto.EnDescription,dto.Type,dto.Unit);
            await _dbContext.MaterialItems.AddAsync(model);
            dto.Id = model.Id;
            return new ApiResponse<MaterialItemDto> { Status = true, Result = dto };
        }
        catch (Exception e)
        {
            return new ApiResponse<MaterialItemDto> { Status = false, Message = e.Message };
        }
    }

    public async Task<ApiResponse<MaterialItemDto>> UpdateAsync(Guid id, MaterialItemDto dto)
    {
        try
        {
            var model = await _repository.GetMaterialItemByIdAsync(id);
            if (model == null) return new ApiResponse<MaterialItemDto> { Status = false, Message = "更新数据失败" };
            model.Update(dto);
            return new ApiResponse<MaterialItemDto> { Status = true, Result = dto };
        }
        catch (Exception e)
        {
            return new ApiResponse<MaterialItemDto> { Status = false, Message = e.Message };
        }
    }

    public async Task<ApiResponse<MaterialItemDto>> DeleteAsync(Guid id)
    {
        try
        {
            var model = await _repository.GetMaterialItemByIdAsync(id);
            if (model == null) return new ApiResponse<MaterialItemDto> { Status = false, Message = "删除数据失败" };
            model.SoftDelete();//软删除
            return new ApiResponse<MaterialItemDto> { Status = true };
        }
        catch (Exception e)
        {
            return new ApiResponse<MaterialItemDto> { Status = false, Message = e.Message };
        }
    }
    #endregion
    
    #region 扩展
    public async Task<ApiResponse<MaterialItemDto>> UpdateInventoryAsync(Guid id, MaterialItemDto dto)
    {
        try
        {
            var model = await _repository.GetMaterialItemByIdAsync(id);
            if (model == null) return new ApiResponse<MaterialItemDto> { Status = false, Message = "更新数据失败" };
            model.UpdateInventory(dto);
            return new ApiResponse<MaterialItemDto> { Status = true, Result = dto };
        }
        catch (Exception e)
        {
            return new ApiResponse<MaterialItemDto> { Status = false, Message = e.Message };
        }
    }

    public async Task<ApiResponse<MaterialItemDto>> UpdateOtherAsync(Guid id, MaterialItemDto dto)
    {
        try
        {
            var model = await _repository.GetMaterialItemByIdAsync(id);
            if (model == null) return new ApiResponse<MaterialItemDto> { Status = false, Message = "更新数据失败" };
            model.UpdateOther(dto);
            return new ApiResponse<MaterialItemDto> { Status = true, Result = dto };
        }
        catch (Exception e)
        {
            return new ApiResponse<MaterialItemDto> { Status = false, Message = e.Message };
        }
    }

    public async Task<ApiResponse<List<MaterialItemDto>>> GetTop50Async()
    {
        try
        {
            var models = await _repository.GetMaterialItemsAsync();
            //todo:有待改进这个逻辑
            var dtos = await _mapper.ProjectTo<MaterialItemDto>(models.Take(50)).ToListAsync();
            return new ApiResponse<List<MaterialItemDto>> { Status = true, Result = dtos };
        }
        catch (Exception e)
        {
            return new ApiResponse<List<MaterialItemDto>> { Status = false, Message = e.Message };
        }
    }

    #endregion
}