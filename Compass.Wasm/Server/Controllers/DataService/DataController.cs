using Compass.DataService.Domain;
using Compass.DataService.Infrastructure;
using Compass.Wasm.Shared.DataService;
using System.ComponentModel.DataAnnotations;
using Compass.Wasm.Shared.DataService.Entities;

namespace Compass.Wasm.Server.Controllers.DataService;

[Route("api/[controller]")]
[ApiController]
[UnitOfWork(typeof(DataDbContext))]
//[Authorize(Roles = "admin,pm,designer")]
public class DataController : ControllerBase
{
    private readonly DataDomainService _domainService;
    private readonly DataDbContext _dataDbContext;
    private readonly IDataRepository _repository;
    private readonly IProjectRepository _projectRepository;
    private readonly IEventBus _eventBus;

    public DataController(DataDomainService domainService, DataDbContext dataDbContext, IDataRepository repository, IProjectRepository projectRepository, IEventBus eventBus)
    {
        _domainService = domainService;
        _dataDbContext = dataDbContext;
        _repository = repository;
        _projectRepository = projectRepository;
        _eventBus = eventBus;
    }

    [HttpGet("All")]
    public async Task<List<ModuleData>> FindAll()
    {
        var result = await _repository.GetModulesDataAsync();
        return result;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ModuleData>> FindById([RequiredGuid] Guid id)
    {
        //返回ValueTask的需要await的一下
        var data = await _repository.GetModuleDataByIdAsync(id);
        if (data == null) return NotFound($"没有Id={id}的ModuleData");
        //此处自动获取了需要的子类，在客户端接收时，使用子类去接收即可
        //比如：_uviData = await Http.GetFromJsonAsync<UviData>($"api/Data/{Id}");
        //Console.WriteLine(data.GetType());
        return data;
    }



    [HttpPost("Add")]
    public async Task<ActionResult> Add(AddModuleDataRequest request)
    {
        var moduleData = ModuleDataFactory.GetModuleData(request.Model);
        //添加Module时添加长宽高
        if (moduleData == null) return NotFound($"型号{request.Model}不存在！");

        moduleData.ChangeId(request.Id);
        moduleData.Length=request.Length;
        moduleData.Width=request.Width;
        moduleData.Height=request.Height;
        
        await _dataDbContext.ModulesData.AddAsync(moduleData);
        return Ok();
    }

}