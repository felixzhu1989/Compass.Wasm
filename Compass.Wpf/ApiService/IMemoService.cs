using System.Collections.Generic;
using System.Threading.Tasks;
using Compass.Wasm.Shared;
using Compass.Wasm.Shared.Parameter;
using Compass.Wasm.Shared.TodoService;

namespace Compass.Wpf.ApiService;

public interface IMemoService : IBaseService<MemoDto>
{
    //扩展的请求
    Task<ApiResponse<MemoDto>> UserAddAsync(MemoDto dto);
    Task<ApiResponse<List<MemoDto>>> GetAllFilterAsync(QueryParameter parameter);
}