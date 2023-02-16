using Compass.TodoService.Infrastructure;
using Compass.Wasm.Shared.TodoService;
using System.ComponentModel.DataAnnotations;
using Compass.Wasm.Server.TodoService;
using Compass.Wasm.Shared;
using Compass.Wasm.Shared.Parameter;

namespace Compass.Wasm.Server.Controllers.TodoService;

[Route("api/[controller]")]
[ApiController]
[UnitOfWork(typeof(TodoDbContext))]
public class TodoController : ControllerBase
{
    private readonly ITodoService _service;
    public TodoController(ITodoService service)
    {
        _service = service;
    }

    #region 标准增删改查
    [HttpGet("All")]
    public async Task<ApiResponse<List<TodoDto>>> GetAll() => await _service.GetAllAsync();

    [HttpGet("{id}")]
    public async Task<ApiResponse<TodoDto>> GetSingle([RequiredGuid] Guid id) => await _service.GetSingleAsync(id);

    [HttpPost("Add")]
    public async Task<ApiResponse<TodoDto>> Add(TodoDto dto) => await _service.AddAsync(dto);

    [HttpPut("{id}")]
    public async Task<ApiResponse<TodoDto>> Update([RequiredGuid] Guid id, TodoDto dto) => await _service.UpdateAsync(id, dto);

    [HttpDelete("{id}")]
    public async Task<ApiResponse<TodoDto>> Delete([RequiredGuid] Guid id) => await _service.DeleteAsync(id);
    #endregion

    /// <summary>
    /// 根据查询条件筛选结果
    /// </summary>
    [HttpGet("Filter")]
    public async Task<ApiResponse<List<TodoDto>>> GetAllFilter(TodoParameter parameter) => await _service.GetAllFilterAsync(parameter);

    /// <summary>
    /// 查询汇总结果
    /// </summary>
    [HttpGet("Summary")]
    public async Task<ApiResponse<TodoSummaryDto>> GetSummary() => await _service.GetSummary();
}