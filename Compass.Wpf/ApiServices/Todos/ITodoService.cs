using System.Collections.Generic;
using System.Threading.Tasks;
using Compass.Wasm.Shared;
using Compass.Wasm.Shared.Parameters;
using Compass.Wpf.ApiService;

namespace Compass.Wpf.ApiServices.Todos;

public interface ITodoService : IBaseService<TodoDto>
{
    //扩展的请求
    Task<ApiResponse<TodoDto>> UserAddAsync(TodoDto dto);
    Task<ApiResponse<List<TodoDto>>> GetAllFilterAsync(TodoParameter parameter);
    Task<ApiResponse<TodoSummaryDto>> GetSummaryAsync();
}