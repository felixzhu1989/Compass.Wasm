using AutoMapper;
using Compass.Wasm.Shared.CategoryService;
using System.ComponentModel.DataAnnotations;

namespace Compass.Wasm.Server.Controllers.CategoryService
{
    [Route("api/[controller]")]
    [ApiController]
    [UnitOfWork(typeof(CSDbContext))]
    //[Authorize(Roles = "admin,pm,designer")]
    public class ModelController : ControllerBase
    {
        private readonly CSDomainService _domainService;
        private readonly CSDbContext _dbContext;
        private readonly ICSRepository _repository;
        private readonly IMapper _mapper;
        public ModelController(CSDomainService domainService, CSDbContext dbContext, ICSRepository repository, IMapper mapper)
        {
            _domainService = domainService;
            _dbContext = dbContext;
            _repository = repository;
            _mapper = mapper;
        }
        [HttpGet("All/{productId}")]
        public async Task<List<ModelResponse>> FindAllByProductId([RequiredGuid] Guid productId)
        {
            //使用AutoMapper将Model转换成ModelResponse（Dto）
            return await _mapper.ProjectTo<ModelResponse>(await _repository.GetModelsByProductIdAsync(productId)).ToListAsync();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<ModelResponse?>> FindById([RequiredGuid] Guid id)
        {
            var model = await _repository.GetModelByIdAsync(id);
            if (model == null) return NotFound($"没有Id={id}的Model");
            return _mapper.Map<ModelResponse>(model);
        }
        [HttpPost]
        public async Task<ActionResult<Guid>> Add(AddModelRequest request)
        {
            Model model = await _domainService.AddModelAsync(request.ProductId, request.Name, request.Workload);
            await _dbContext.Models.AddAsync(model);
            return model.Id;
        }
        [HttpPut("{id}")]
        public async Task<ActionResult> Update([RequiredGuid] Guid id, ModelResponse request)
        {
            var model = await _repository.GetModelByIdAsync(id);
            if (model == null) return NotFound($"没有Id={id}的Model");
            model.ChangeName(request.Name).ChangeWorkload(request.Workload);
            return Ok();
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([RequiredGuid] Guid id)
        {
            var model = await _repository.GetModelByIdAsync(id);
            //这样做仍然是幂等的，因为“调用N次，确保服务器处于与第一次调用相同的状态。”与响应无关
            if (model == null) return NotFound($"没有Id={id}的Model");
            model.SoftDelete();
            return Ok();
        }
        [HttpPut("Sort/{productId}")]
        public async Task<ActionResult> Sort([RequiredGuid] Guid productId, CategorySortRequest request)
        {
            await _domainService.SortModelsAsync(productId, request.SortedIds);
            return Ok();
        }
    }
}
