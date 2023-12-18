using AutoMapper;
using Compass.Wasm.Shared;
using Compass.Wasm.Shared.Categories;
using Compass.Wasm.Shared.Params;

namespace Compass.Wasm.Server.Services.Categories;

public interface IAccCutListService : IBaseService<AccCutListDto>
{
    Task<ApiResponse<List<AccCutListDto>>> GetAllByAccTypeAsync(AccCutListParam param);
}

public class AccCutListService: IAccCutListService
{
    #region ctor
    private readonly CategoryDomainService _domainService;
    private readonly CategoryDbContext _dbContext;
    private readonly ICategoryRepository _repository;
    private readonly IMapper _mapper;
    public AccCutListService(CategoryDomainService domainService, CategoryDbContext dbContext, ICategoryRepository repository, IMapper mapper)
    {
        _domainService = domainService;
        _dbContext = dbContext;
        _repository = repository;
        _mapper = mapper;
    }
    #endregion

    #region 基本增删改查
    public async Task<ApiResponse<List<AccCutListDto>>> GetAllAsync()
    {
        try
        {
            var models = await _repository.GetAccCutListsAsync();
            var dtos = await _mapper.ProjectTo<AccCutListDto>(models).OrderBy(x => x.AccType).ToListAsync();
            return new ApiResponse<List<AccCutListDto>> { Status = true, Result = dtos };
        }
        catch (Exception e)
        {
            return new ApiResponse<List<AccCutListDto>> { Status = false, Message = e.Message };
        }
    }

    public async Task<ApiResponse<AccCutListDto>> GetSingleAsync(Guid id)
    {
        try
        {
            var model = await _repository.GetAccCutListByIdAsync(id);
            if (model == null) return new ApiResponse<AccCutListDto> { Status = false, Message = "查询数据失败" };
            var dto = _mapper.Map<AccCutListDto>(model);
            return new ApiResponse<AccCutListDto> { Status = true, Result = dto };
        }
        catch (Exception e)
        {
            return new ApiResponse<AccCutListDto> { Status = false, Message = e.Message };
        }
    }

    public async Task<ApiResponse<AccCutListDto>> AddAsync(AccCutListDto dto)
    {
        try
        {
            var model = new AccCutList(Guid.NewGuid(), dto.AccType, dto.PartDescription, dto.Length, dto.Width, dto.Thickness, dto.Quantity, dto.Material, dto.PartNo, dto.BendingMark);
            await _dbContext.AccCutLists.AddAsync(model);
            dto.Id = model.Id;
            return new ApiResponse<AccCutListDto> { Status = true, Result = dto };
        }
        catch (Exception e)
        {
            return new ApiResponse<AccCutListDto> { Status = false, Message = e.Message };
        }
    }

    public async Task<ApiResponse<AccCutListDto>> UpdateAsync(Guid id, AccCutListDto dto)
    {
        try
        {
            var model = await _repository.GetAccCutListByIdAsync(id);
            if (model == null) return new ApiResponse<AccCutListDto> { Status = false, Message = "更新数据失败" };
            model.Update(dto);
            return new ApiResponse<AccCutListDto> { Status = true, Result = dto };
        }
        catch (Exception e)
        {
            return new ApiResponse<AccCutListDto> { Status = false, Message = e.Message };
        }
    }

    public async Task<ApiResponse<AccCutListDto>> DeleteAsync(Guid id)
    {
        try
        {
            var model = await _repository.GetAccCutListByIdAsync(id);
            if (model == null) return new ApiResponse<AccCutListDto> { Status = false, Message = "删除数据失败" };
            model.SoftDelete();//软删除
            return new ApiResponse<AccCutListDto> { Status = true };
        }
        catch (Exception e)
        {
            return new ApiResponse<AccCutListDto> { Status = false, Message = e.Message };
        }
    }
    #endregion

    #region 扩展
    public async Task<ApiResponse<List<AccCutListDto>>> GetAllByAccTypeAsync(AccCutListParam param)
    {
        try
        {
            var models = await _repository.GetAccCutListsAsync();
            var dtos = await _mapper.ProjectTo<AccCutListDto>(models).Where(x => x.AccType == param.AccType).ToListAsync();
            return new ApiResponse<List<AccCutListDto>> { Status = true, Result = dtos };
        }
        catch (Exception e)
        {
            return new ApiResponse<List<AccCutListDto>> { Status = false, Message = e.Message };
        }
    }
    #endregion
}