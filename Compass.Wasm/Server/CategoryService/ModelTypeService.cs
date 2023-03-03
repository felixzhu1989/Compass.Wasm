using AutoMapper;
using Compass.Wasm.Shared;
using Compass.Wasm.Shared.CategoryService;

namespace Compass.Wasm.Server.CategoryService;

public class ModelTypeService:IModelTypeService
{
    private readonly CategoryDomainService _domainService;
    private readonly CategoryDbContext _dbContext;
    private readonly ICategoryRepository _repository;
    private readonly IMapper _mapper;
    public ModelTypeService(CategoryDomainService domainService, CategoryDbContext dbContext, ICategoryRepository repository, IMapper mapper)
    {
        _domainService = domainService;
        _dbContext = dbContext;
        _repository = repository;
        _mapper = mapper;
    }
    public async Task<ApiResponse<List<ModelTypeDto>>> GetAllAsync()
    {
        try
        {
            var models = await _repository.GetModelTypesAsync();
            var dtos = await _mapper.ProjectTo<ModelTypeDto>(models).ToListAsync();
            return new ApiResponse<List<ModelTypeDto>> { Status = true, Result = dtos };
        }
        catch (Exception e)
        {
            return new ApiResponse<List<ModelTypeDto>> { Status = false, Message = e.Message };
        }
    }

    public async Task<ApiResponse<ModelTypeDto>> GetSingleAsync(Guid id)
    {
        try
        {
            var model = await _repository.GetModelTypeByIdAsync(id);
            if (model != null)
            {
                var dto = _mapper.Map<ModelTypeDto>(model);
                return new ApiResponse<ModelTypeDto> { Status = true, Result = dto };
            }
            return new ApiResponse<ModelTypeDto> { Status = false, Message = "查询数据失败" };
        }
        catch (Exception e)
        {
            return new ApiResponse<ModelTypeDto> { Status = false, Message = e.Message };
        }
    }

    public async Task<ApiResponse<ModelTypeDto>> AddAsync(ModelTypeDto dto)
    {
        try
        {
            var model = await _domainService.AddModelTypeAsync(dto.ModelId, dto.Name, dto.Description, dto.Length, dto.Width, dto.Height);
            await _dbContext.ModelTypes.AddAsync(model);
            dto.Id= model.Id;
            return new ApiResponse<ModelTypeDto> { Status = true, Result = dto };
        }
        catch (Exception e)
        {
            return new ApiResponse<ModelTypeDto> { Status = false, Message = e.Message };
        }
    }

    public async Task<ApiResponse<ModelTypeDto>> UpdateAsync(Guid id, ModelTypeDto dto)
    {
        try
        {
            var model = await _repository.GetModelTypeByIdAsync(id);
            if (model != null)
            {
                model.ChangeName(dto.Name).ChangeDescription(dto.Description)
                    .ChangeLength(dto.Length).ChangeWidth(dto.Width).ChangeHeight(dto.Height);
                return new ApiResponse<ModelTypeDto> { Status = true, Result = dto };
            }
            return new ApiResponse<ModelTypeDto> { Status = false, Message = "更新数据失败" };
        }
        catch (Exception e)
        {
            return new ApiResponse<ModelTypeDto> { Status = false, Message = e.Message };
        }
    }

    public async Task<ApiResponse<ModelTypeDto>> DeleteAsync(Guid id)
    {
        try
        {
            var model = await _repository.GetModelTypeByIdAsync(id);
            if (model != null)
            {
                model.SoftDelete();//软删除
                return new ApiResponse<ModelTypeDto> { Status = true };
            }
            return new ApiResponse<ModelTypeDto> { Status = false, Message = "删除数据失败" };
        }
        catch (Exception e)
        {
            return new ApiResponse<ModelTypeDto> { Status = false, Message = e.Message };
        }
    }
}