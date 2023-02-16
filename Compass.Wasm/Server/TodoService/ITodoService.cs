using Compass.Wasm.Shared;
using Compass.Wasm.Shared.Parameter;
using Compass.Wasm.Shared.TodoService;

namespace Compass.Wasm.Server.TodoService;

public interface ITodoService : IBaseService<TodoDto>
{
    //扩展标准增删改查之外的查询功能
    Task<ApiResponse<List<TodoDto>>> GetAllFilterAsync(TodoParameter parameter);
    Task<ApiResponse<TodoSummaryDto>> GetSummary();
}