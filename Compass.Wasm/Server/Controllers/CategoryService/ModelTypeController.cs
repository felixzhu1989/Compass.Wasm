using AutoMapper;
using Compass.Wasm.Shared.CategoryService;
using System.ComponentModel.DataAnnotations;

namespace Compass.Wasm.Server.Controllers.CategoryService;

[Route("api/[controller]")]
[ApiController]
[UnitOfWork(typeof(CSDbContext))]
//[Authorize(Roles = "admin,pm,designer")]
public class ModelTypeController : ControllerBase
{
    private readonly CSDomainService _domainService;
    private readonly CSDbContext _dbContext;
    private readonly ICSRepository _repository;
    private readonly IMapper _mapper;
    public ModelTypeController(CSDomainService domainService, CSDbContext dbContext, ICSRepository repository, IMapper mapper)
    {
        _domainService = domainService;
        _dbContext = dbContext;
        _repository = repository;
        _mapper = mapper;
    }
    [HttpGet("All/{modelId}")]
    public async Task<List<ModelTypeResponse>> FindAllByProductId([RequiredGuid] Guid modelId)
    {
        //使用AutoMapper将Model转换成ModelResponse（Dto）
        return await _mapper.ProjectTo<ModelTypeResponse>(await _repository.GetModelTypesByProductIdAsync(modelId)).ToListAsync();
    }
    [HttpGet("{id}")]
    public async Task<ActionResult<ModelTypeResponse?>> FindById([RequiredGuid] Guid id)
    {
        var modelType = await _repository.GetModelTypeByIdAsync(id);
        if (modelType == null) return NotFound($"没有Id={id}的ModelType");
        return _mapper.Map<ModelTypeResponse>(modelType);
    }
    //根据Id查询SBU，Product，Model，ModelType
    [HttpGet("Category/{id}")]
    public async Task<ActionResult<CategoryResponse?>> CategoryById([RequiredGuid] Guid id)
    {
        var modelType = await _repository.GetModelTypeByIdAsync(id);
        if (modelType == null) return NotFound($"没有Id={id}的ModelType");
        var model = await _repository.GetModelByIdAsync(modelType.ModelId);
        var product = await _repository.GetProductByIdAsync(model.ProductId);
        return new CategoryResponse { Sbu = product.Sbu, ProductId = product.Id, ModelId = model.Id ,Description = modelType.Description};
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> Add(AddModelTypeRequest request)
    {
        ModelType modelType = await _domainService.AddModelTypeAsync(request.ModelId, request.Name,request.Description);
        await _dbContext.ModelTypes.AddAsync(modelType);
        return modelType.Id;
    }
    [HttpPut("{id}")]
    public async Task<ActionResult> Update([RequiredGuid] Guid id, ModelTypeResponse request)
    {
        var modelType = await _repository.GetModelTypeByIdAsync(id);
        if (modelType == null) return NotFound($"没有Id={id}的ModelType");
        modelType.ChangeName(request.Name).ChangeDescription(request.Description);
        return Ok();
    }
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete([RequiredGuid] Guid id)
    {
        var modelType = await _repository.GetModelTypeByIdAsync(id);
        //这样做仍然是幂等的，因为“调用N次，确保服务器处于与第一次调用相同的状态。”与响应无关
        if (modelType == null) return NotFound($"没有Id={id}的ModelType");
        modelType.SoftDelete();
        return Ok();
    }
    [HttpPut("Sort/{modelId}")]
    public async Task<ActionResult> Sort([RequiredGuid] Guid modelId, CategorySortRequest request)
    {
        await _domainService.SortModelTypesAsync(modelId, request.SortedIds);
        return Ok();
    }

}