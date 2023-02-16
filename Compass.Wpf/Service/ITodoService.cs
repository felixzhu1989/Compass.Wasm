using System.Collections.Generic;
using Compass.Wasm.Shared.Parameter;
using Compass.Wasm.Shared;
using Compass.Wasm.Shared.TodoService;
using System.Threading.Tasks;
namespace Compass.Wpf.Service;

public interface ITodoService : IBaseService<TodoDto>
{
    Task<ApiResponse<List<TodoDto>>> GetAllFilterAsync(TodoParameter parameter);
    Task<ApiResponse<TodoSummaryDto>> GetSummaryAsync();
}