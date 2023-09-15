using Compass.Wasm.Shared;
using Compass.Wpf.ApiService;
using System.Threading.Tasks;
using Compass.Dtos;

namespace Compass.Wpf.ApiServices.Plans;

public interface IMainPlanService : IBaseService<MainPlanDto>
{
    Task<ApiResponse<MainPlanDto>> UpdateStatusesAsync(Guid id, MainPlanDto dto);
    Task<ApiResponse<MainPlanCountDto>> GetCountAsync();
}
public class MainPlanService : BaseService<MainPlanDto>, IMainPlanService
{
    #region ctor
    private readonly HttpRestClient _client;
    public MainPlanService(HttpRestClient client) : base(client, "MainPlan")
    {
        _client = client;
    }
    #endregion

    #region 扩展
    public async Task<ApiResponse<MainPlanDto>> UpdateStatusesAsync(Guid id, MainPlanDto dto)
    {
        var request = new BaseRequest
        {
            Method = RestSharp.Method.Put,
            Route = $"api/MainPlan/UpdateStatuses/{id}",
            Param = dto
        };
        return await _client.ExecuteAsync<MainPlanDto>(request);
    }

    public async Task<ApiResponse<MainPlanCountDto>> GetCountAsync()
    {
        var request = new BaseRequest
        {
            Method = RestSharp.Method.Get,
            Route = "api/MainPlan/Count"
        };
        return await _client.ExecuteAsync<MainPlanCountDto>(request);
    } 
    #endregion
}