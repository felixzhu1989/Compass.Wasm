using System.Collections.Generic;
using System.Threading.Tasks;
using Compass.Wasm.Shared;
using Compass.Wasm.Shared.Parameters;
using Compass.Wasm.Shared.Todos;
using Compass.Wpf.ApiService;

namespace Compass.Wpf.ApiServices.Todos;

public interface IMemoService : IBaseService<MemoDto>
{
    //扩展的请求
    Task<ApiResponse<MemoDto>> UserAddAsync(MemoDto dto);
    Task<ApiResponse<List<MemoDto>>> GetAllFilterAsync(QueryParameter parameter);
}