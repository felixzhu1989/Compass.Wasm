using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Compass.QualityService.Domain;
using Compass.QualityService.Domain.Entities;
using Compass.QualityService.Infrastructure;
using Compass.Wasm.Shared.Categories;
using Compass.Wasm.Shared.Quality;

namespace Compass.Wasm.Server.Controllers.Quality;

[Route("api/[controller]")]
[ApiController]
[UnitOfWork(typeof(QualityDbContext))]
public class FinalInspectionCheckItemTypeController : ControllerBase
{
    private readonly QualityDomainService _domainService;
    private readonly QualityDbContext _dbContext;
    private readonly IQualityRepository _repository;
    private readonly IMapper _mapper;
    public FinalInspectionCheckItemTypeController(QualityDomainService domainService,QualityDbContext dbContext,IQualityRepository repository,IMapper mapper)
    {
        _domainService = domainService;
        _dbContext = dbContext;
        _repository = repository;
        _mapper = mapper;
    }
    [HttpGet("All")]
    public async Task<List<FinalInspectionCheckItemTypeResponse>> FindAll()
    {
        //使用AutoMapper将类型转换成Response（Dto）
        return await _mapper.ProjectTo<FinalInspectionCheckItemTypeResponse>(await _repository.GetFinalInspectionCheckItemTypesAsync()).ToListAsync();
    }
    [HttpGet("{id}")]
    public async Task<ActionResult<FinalInspectionCheckItemTypeResponse?>> FindById([RequiredGuid] Guid id)
    {
        var checkItemType = await _repository.GetFinalInspectionCheckItemTypeByIdAsync(id);
        if (checkItemType == null) return NotFound($"没有Id={id}的FinalInspectionCheckItemType");
        return _mapper.Map<FinalInspectionCheckItemTypeResponse>(checkItemType);
    }
    [HttpPost]
    public async Task<ActionResult<Guid>> Add(AddFinalInspectionCheckItemTypeRequest request)
    {
        FinalInspectionCheckItemType checkItemType = await _domainService.AddFinalInspectionCheckItemTypeAsync(request.Name);
        await _dbContext.FinalInspectionCheckItemTypes.AddAsync(checkItemType);
        return checkItemType.Id;
    }
    [HttpPut("{id}")]
    public async Task<ActionResult> Update([RequiredGuid] Guid id, FinalInspectionCheckItemTypeResponse request)
    {
        var checkItemType = await _repository.GetFinalInspectionCheckItemTypeByIdAsync(id);
        if (checkItemType == null) return NotFound($"没有Id={id}的FinalInspectionCheckItemType");
        checkItemType.ChangeName(request.Name);
        return Ok();
    }
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete([RequiredGuid] Guid id)
    {
        var checkItemType = await _repository.GetFinalInspectionCheckItemTypeByIdAsync(id);
        //这样做仍然是幂等的，因为“调用N次，确保服务器处于与第一次调用相同的状态。”与响应无关
        if (checkItemType == null) return NotFound($"没有Id={id}的Product");
        checkItemType.SoftDelete();
        return Ok();
    }
    





}