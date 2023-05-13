using Compass.Wasm.Shared;
using Compass.Wpf.ApiService;
using SolidWorks.Interop.sldworks;
using System.Threading.Tasks;

namespace Compass.Wpf.ApiServices.Plans;

public interface IMainPlanService : IBaseService<MainPlanDto>
{
    Task<ApiResponse<MainPlanDto>> UpdateStatusesAsync(Guid id, MainPlanDto dto);
}
public class MainPlanService : BaseService<MainPlanDto>, IMainPlanService
{
    private readonly HttpRestClient _client;
    public MainPlanService(HttpRestClient client) : base(client, "MainPlan")
    {
        _client = client;
    }

    public async Task<ApiResponse<MainPlanDto>> UpdateStatusesAsync(Guid id, MainPlanDto dto)
    {
        BaseRequest request = new BaseRequest
        {
            Method = RestSharp.Method.Put,
            Route = $"api/MainPlan/UpdateStatuses/{id}",
            Parameter = dto
        };
        return await _client.ExecuteAsync<MainPlanDto>(request);
    }
}