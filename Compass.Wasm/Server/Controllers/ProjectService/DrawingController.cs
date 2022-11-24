using AutoMapper;
using Compass.Wasm.Client.ProjectService;
using Compass.Wasm.Shared.ProjectService;
using System.ComponentModel.DataAnnotations;

namespace Compass.Wasm.Server.Controllers.ProjectService;

[Route("api/[controller]")]
[ApiController]
[UnitOfWork(typeof(PMDbContext))]
//[Authorize(Roles = "admin,pm,designer")]
public class DrawingController : ControllerBase
{
    private readonly PMDomainService _domainService;
    private readonly PMDbContext _dbContext;
    private readonly IPMRepository _repository;
    private readonly IMapper _mapper;
    private readonly IEventBus _eventBus;
    private readonly IdUserManager _userManager;

    public DrawingController(PMDomainService domainService, PMDbContext dbContext, IPMRepository repository, IMapper mapper,IEventBus eventBus, IdUserManager userManager)
    {
        _domainService = domainService;
        _dbContext = dbContext;
        _repository = repository;
        _mapper = mapper;
        _eventBus = eventBus;
        _userManager = userManager;
    }
    

    [HttpGet("All/{projectId}")]
    public async Task<DrawingResponse[]> FindAll([RequiredGuid] Guid projectId)
    {
        return await _mapper.ProjectTo<DrawingResponse>(await _repository.GetDrawingsByProjectIdAsync(projectId)).ToArrayAsync();
    }

    [HttpGet("User/{userName}")]
    public async Task<Dictionary<Guid, List<DrawingResponse>>> FindAllByUserName(string userName)
    {
        Dictionary<Guid, List<DrawingResponse>> dic = new ();
        var user = await _userManager.FindByNameAsync(userName);
        var drawings= await _mapper.ProjectTo<DrawingResponse>(await _repository.GetDrawingsByUserIdAsync(user.Id)).ToListAsync();
        //按照项目对Drawings进行分组
        var group= drawings.GroupBy(x => x.ProjectId);
        foreach (var g in group)
        {
            if (!dic.ContainsKey(g.Key)) dic.Add(g.Key, new List<DrawingResponse>());
            foreach (var d in g)
            {
                dic[g.Key].Add(d);
            }
        }
        return dic;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<DrawingResponse?>> FindById([RequiredGuid] Guid id)
    {
        var drawing = await _repository.GetDrawingByIdAsync(id);
        if (drawing == null) return NotFound($"没有Id={id}的Drawing");
        return _mapper.Map<DrawingResponse>(drawing);
    }

    [HttpGet("Exists/{projectId}")]
    public async Task<ActionResult<bool>> DrawingExists([RequiredGuid] Guid projectId)
    {
        return await _repository.DrawingExistsInProjectAsync(projectId);
    }
    [HttpGet("Total/{projectId}")]
    public async Task<ActionResult<int>> TotalDrawingsCount([RequiredGuid] Guid projectId)
    {
        return await _repository.GetTotalDrawingsCountInProjectAsync(projectId);
    }
    [HttpGet("NotAssigned/{projectId}")]
    public async Task<ActionResult<int>> NotAssignedDrawingsCount([RequiredGuid] Guid projectId)
    {
        return await _repository.GetNotAssignedDrawingsCountInProjectAsync(projectId);
    }

    [HttpGet("UserName/{drawingId}")]
    public async Task<ActionResult<string>> GetUserName([RequiredGuid] Guid drawingId)
    {
        //查找图纸负责人名字
        var drawing = await _repository.GetDrawingByIdAsync(drawingId);
        if (drawing!.UserId != null && !drawing.Equals(Guid.Empty))
        {
            return (await _userManager.FindByIdAsync(drawing.UserId.ToString())).UserName;
        }
        return "";
    }

    [HttpPost("Add")]
    public async Task<ActionResult<Guid>> Add(AddDrawingRequest request)
    {
        var drawing = new Drawing(Guid.NewGuid(), request.ProjectId, request.ItemNumber, request.DrawingUrl);
        await _dbContext.Drawings.AddAsync(drawing);
        return drawing.Id;
    }


    [HttpPut("{id}")]
    public async Task<ActionResult> Update([RequiredGuid] Guid id, DrawingResponse request)
    {
        var drawing = await _repository.GetDrawingByIdAsync(id);
        if (drawing == null) return NotFound($"没有Id={id}的Drawing");
        drawing.ChangeItemNumber(request.ItemNumber)
            .ChangeDrawingUrl(request.DrawingUrl!);
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete([RequiredGuid] Guid id)
    {
        var drawing = await _repository.GetDrawingByIdAsync(id);
        //这样做仍然是幂等的，因为“调用N次，确保服务器处于与第一次调用相同的状态。”与响应无关
        if (drawing == null) return NotFound($"没有Id={id}的Drawing");
        drawing.SoftDelete();//软删除
        return Ok();
    }

}