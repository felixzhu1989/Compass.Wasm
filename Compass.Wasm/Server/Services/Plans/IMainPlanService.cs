using Compass.Wasm.Shared;
using Compass.Wasm.Shared.Plans;

namespace Compass.Wasm.Server.Services.Plans;

public interface IMainPlanService : IBaseService<MainPlanDto>
{
    //扩展查询
    Task<ApiResponse<MainPlanDto>> UpdateStatusesAsync(Guid id, MainPlanDto dto);
    //GetIndexDataAsync
    Task<ApiResponse<List<MainPlanDto>>> GetIndexDataAsync();
}