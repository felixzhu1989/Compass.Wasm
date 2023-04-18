using AutoMapper;
using Compass.Wasm.Shared;
using Compass.Wasm.Shared.Categories;

namespace Compass.Wasm.Server.Services.Categories;

public class ModelService : IModelService
{
    private readonly CategoryDomainService _domainService;
    private readonly CategoryDbContext _dbContext;
    private readonly ICategoryRepository _repository;
    private readonly IMapper _mapper;
    public ModelService(CategoryDomainService domainService, CategoryDbContext dbContext, ICategoryRepository repository, IMapper mapper)
    {
        _domainService = domainService;
        _dbContext = dbContext;
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ApiResponse<List<ModelDto>>> GetAllAsync()
    {
        try
        {
            var models = await _repository.GetModelsAsync();
            var dtos = await _mapper.ProjectTo<ModelDto>(models).ToListAsync();
            return new ApiResponse<List<ModelDto>> { Status = true, Result = dtos };
        }
        catch (Exception e)
        {
            return new ApiResponse<List<ModelDto>> { Status = false, Message = e.Message };
        }
    }

    public async Task<ApiResponse<ModelDto>> GetSingleAsync(Guid id)
    {
        try
        {
            var model = await _repository.GetModelByIdAsync(id);
            if (model != null)
            {
                var dto = _mapper.Map<ModelDto>(model);
                return new ApiResponse<ModelDto> { Status = true, Result = dto };
            }
            return new ApiResponse<ModelDto> { Status = false, Message = "查询数据失败" };
        }
        catch (Exception e)
        {
            return new ApiResponse<ModelDto> { Status = false, Message = e.Message };
        }
    }

    public async Task<ApiResponse<ModelDto>> AddAsync(ModelDto dto)
    {
        try
        {
            var model = await _domainService.AddModelAsync(dto.ProductId, dto.Name, dto.Workload);
            await _dbContext.Models.AddAsync(model);
            dto.Id = model.Id;
            return new ApiResponse<ModelDto> { Status = true, Result = dto };
        }
        catch (Exception e)
        {
            return new ApiResponse<ModelDto> { Status = false, Message = e.Message };
        }
    }

    public async Task<ApiResponse<ModelDto>> UpdateAsync(Guid id, ModelDto dto)
    {
        try
        {
            var model = await _repository.GetModelByIdAsync(id);
            if (model != null)
            {
                model.ChangeName(dto.Name).ChangeWorkload(dto.Workload);
                return new ApiResponse<ModelDto> { Status = true, Result = dto };
            }
            return new ApiResponse<ModelDto> { Status = false, Message = "更新数据失败" };
        }
        catch (Exception e)
        {
            return new ApiResponse<ModelDto> { Status = false, Message = e.Message };
        }
    }

    public async Task<ApiResponse<ModelDto>> DeleteAsync(Guid id)
    {
        try
        {
            var model = await _repository.GetModelByIdAsync(id);
            if (model != null)
            {
                model.SoftDelete();//软删除
                return new ApiResponse<ModelDto> { Status = true };
            }
            return new ApiResponse<ModelDto> { Status = false, Message = "删除数据失败" };
        }
        catch (Exception e)
        {
            return new ApiResponse<ModelDto> { Status = false, Message = e.Message };
        }
    }
}