using AutoMapper;
using Compass.Wasm.Client.ProjectService;
using Compass.Wasm.Shared.ProjectService;
using System.ComponentModel.DataAnnotations;

namespace Compass.Wasm.Server.Controllers;

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

    public DrawingController(PMDomainService domainService, PMDbContext dbContext, IPMRepository repository, IMapper mapper)
    {
        _domainService = domainService;
        _dbContext = dbContext;
        _repository = repository;
        _mapper = mapper;
    }


    [HttpGet("All/{projectId}")]
    public async Task<DrawingResponse[]> FindAll([RequiredGuid] Guid projectId)
    {
        return await _mapper.ProjectTo<DrawingResponse>(await _repository.GetDrawingsByProjectIdAsync(projectId)).ToArrayAsync();
    }
    [HttpGet("{id}")]
    public async Task<ActionResult<DrawingResponse?>> FindById([RequiredGuid] Guid id)
    {
        var drawing = await _repository.GetDrawingByIdAsync(id);
        if (drawing == null) return NotFound($"没有Id={id}的Project");
        return _mapper.Map<DrawingResponse>(drawing);
    }

    [HttpPost("Add")]
    public async Task<ActionResult<Guid>> Add(AddDrawingRequest request)
    {
        var drawing = new Drawing(Guid.NewGuid(), request.ProjectId, request.ItemNumber,request.DrawingUrl);
        await _dbContext.Drawings.AddAsync(drawing);
        return drawing.Id;
    }


    [HttpPut("{id}")]
    public async Task<ActionResult> Update([RequiredGuid] Guid id, DrawingResponse request)
    {
        var drawing = await _repository.GetDrawingByIdAsync(id);
        if (drawing == null) return NotFound($"没有Id={id}的Drawing");
        drawing.ChangeItemNumber(request.ItemNumber)
            .ChangeDrawingUrl(request.DrawingUrl);
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