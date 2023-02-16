using Compass.TodoService.Infrastructure;
using Compass.Wasm.Shared.TodoService;
using System.ComponentModel.DataAnnotations;
using Compass.Wasm.Server.TodoService;
using Compass.Wasm.Shared;

namespace Compass.Wasm.Server.Controllers.TodoService;

[Route("api/[controller]")]
[ApiController]
[UnitOfWork(typeof(TodoDbContext))]
public class MemoController : ControllerBase
{
    private readonly IMemoService _service;
    public MemoController(IMemoService service)
    {
        _service = service;
    }

    [HttpGet("All")]
    public async Task<ApiResponse<List<MemoDto>>> GetAll() => await _service.GetAllAsync();

    [HttpGet("{id}")]
    public async Task<ApiResponse<MemoDto>> GetSingle([RequiredGuid] Guid id) => await _service.GetSingleAsync(id);

    [HttpPost("Add")]
    public async Task<ApiResponse<MemoDto>> Add(MemoDto dto) => await _service.AddAsync(dto);

    [HttpPut("{id}")]
    public async Task<ApiResponse<MemoDto>> Update([RequiredGuid] Guid id, MemoDto dto) => await _service.UpdateAsync(id, dto);

    [HttpDelete("{id}")]
    public async Task<ApiResponse<MemoDto>> Delete([RequiredGuid] Guid id) => await _service.DeleteAsync(id);



}