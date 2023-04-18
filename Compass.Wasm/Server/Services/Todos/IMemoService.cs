using Compass.Wasm.Shared;
using Compass.Wasm.Shared.Parameters;
using Compass.Wasm.Shared.Todos;

namespace Compass.Wasm.Server.Services.Todos;

public interface IMemoService : IBaseService<MemoDto>
{
    //扩展标准增删改查之外的查询功能
    Task<ApiResponse<List<MemoDto>>> GetUserAllAsync(Guid userId);
    Task<ApiResponse<MemoDto>> UserAddAsync(MemoDto dto, Guid userId);

    Task<ApiResponse<List<MemoDto>>> GetAllFilterAsync(QueryParameter parameter, Guid userId);
}