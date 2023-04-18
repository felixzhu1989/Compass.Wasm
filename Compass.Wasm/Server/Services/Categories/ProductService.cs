using AutoMapper;
using Compass.Wasm.Shared;
using Compass.Wasm.Shared.Categories;

namespace Compass.Wasm.Server.Services.Categories;

public class ProductService : IProductService
{

    private readonly CategoryDomainService _domainService;
    private readonly CategoryDbContext _dbContext;
    private readonly ICategoryRepository _repository;
    private readonly IMapper _mapper;
    public ProductService(CategoryDomainService domainService, CategoryDbContext dbContext, ICategoryRepository repository, IMapper mapper)
    {
        _domainService = domainService;
        _dbContext = dbContext;
        _repository = repository;
        _mapper = mapper;
    }

    #region 基本增删改查
    public async Task<ApiResponse<List<ProductDto>>> GetAllAsync()
    {
        try
        {
            var models = await _repository.GetProductsAsync();
            var dtos = await _mapper.ProjectTo<ProductDto>(models).ToListAsync();
            return new ApiResponse<List<ProductDto>> { Status = true, Result = dtos };
        }
        catch (Exception e)
        {
            return new ApiResponse<List<ProductDto>> { Status = false, Message = e.Message };
        }
    }

    public async Task<ApiResponse<ProductDto>> GetSingleAsync(Guid id)
    {
        try
        {
            var model = await _repository.GetProductByIdAsync(id);
            if (model != null)
            {
                var dto = _mapper.Map<ProductDto>(model);
                return new ApiResponse<ProductDto> { Status = true, Result = dto };
            }
            return new ApiResponse<ProductDto> { Status = false, Message = "查询数据失败" };
        }
        catch (Exception e)
        {
            return new ApiResponse<ProductDto> { Status = false, Message = e.Message };
        }
    }

    public async Task<ApiResponse<ProductDto>> AddAsync(ProductDto dto)
    {
        try
        {
            var model = await _domainService.AddProductAsync(dto.Name, dto.Sbu);
            await _dbContext.Products.AddAsync(model);
            dto.Id = model.Id;
            return new ApiResponse<ProductDto> { Status = true, Result = dto };
        }
        catch (Exception e)
        {
            return new ApiResponse<ProductDto> { Status = false, Message = e.Message };
        }
    }

    public async Task<ApiResponse<ProductDto>> UpdateAsync(Guid id, ProductDto dto)
    {
        try
        {
            var model = await _repository.GetProductByIdAsync(id);
            if (model != null)
            {
                model.ChangeName(dto.Name).ChangeSbu(dto.Sbu);
                return new ApiResponse<ProductDto> { Status = true, Result = dto };
            }
            return new ApiResponse<ProductDto> { Status = false, Message = "更新数据失败" };
        }
        catch (Exception e)
        {
            return new ApiResponse<ProductDto> { Status = false, Message = e.Message };
        }
    }

    public async Task<ApiResponse<ProductDto>> DeleteAsync(Guid id)
    {
        try
        {
            var model = await _repository.GetProductByIdAsync(id);
            if (model != null)
            {
                model.SoftDelete();//软删除
                return new ApiResponse<ProductDto> { Status = true };
            }
            return new ApiResponse<ProductDto> { Status = false, Message = "删除数据失败" };
        }
        catch (Exception e)
        {
            return new ApiResponse<ProductDto> { Status = false, Message = e.Message };
        }
    }



    #endregion


    #region 扩展的查询功能,WPF
    public async Task<ApiResponse<List<ProductDto>>> GetModelTypeTreeAsync()
    {
        try
        {
            //先查询所有的产品类型
            var products = await _repository.GetProductsAsync();
            var productDtos = await _mapper.ProjectTo<ProductDto>(products).ToListAsync();
            foreach (var productDto in productDtos)
            {
                //再查询产品下的所有Model
                var models = await _repository.GetModelsByProductIdAsync(productDto.Id.Value);
                productDto.ModelDtos = await _mapper.ProjectTo<ModelDto>(models).ToListAsync();
                foreach (var modelDto in productDto.ModelDtos)
                {
                    //查询model下所有的ModelType
                    var modelTypes = await _repository.GetModelTypesByModelIdAsync(modelDto.Id.Value);
                    modelDto.ModelTypeDtos = await _mapper.ProjectTo<ModelTypeDto>(modelTypes).ToListAsync();
                    //将子模型名称装入子模型中
                    modelDto.ModelTypeDtos.ForEach(x => x.ModelName = $"{modelDto.Name}_{x.Name}"); //改成下划线
                }
            }
            return new ApiResponse<List<ProductDto>> { Status = true, Result = productDtos };
        }
        catch (Exception e)
        {
            return new ApiResponse<List<ProductDto>> { Status = false, Message = e.Message };
        }
    }
    #endregion
}