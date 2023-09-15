using System.Net.Http.Json;
using Compass.Dtos;
using Compass.Wasm.Shared;
using Compass.Wasm.Shared.Plans;

namespace Compass.Wasm.Client.Services.Plans;

public interface IMainPlanService : IBaseService<MainPlanDto>
{
    //主计划绑定项目
    Task<HttpResponseMessage> UpdateStatusesAsync(Guid id, MainPlanDto dto);
    //显示主页查询
    Task<ApiResponse<List<MainPlanDto>>> GetIndexDataAsync();
    Task<ApiResponse<List<MainPlanDto>>> GetAllByProjectIdAsync(Guid projectId);

}
public class MainPlanService : BaseService<MainPlanDto>, IMainPlanService
{
    private readonly HttpClient _http;
    public MainPlanService(HttpClient http) : base(http, "MainPlan")
    {
        _http = http;
    }
    public Task<HttpResponseMessage> UpdateStatusesAsync(Guid id, MainPlanDto dto)
    {
        return _http.PutAsJsonAsync($"api/MainPlan/UpdateStatuses/{id}", dto);
    }

    public Task<ApiResponse<List<MainPlanDto>>> GetIndexDataAsync()
    {
        return _http.GetFromJsonAsync<ApiResponse<List<MainPlanDto>>>("api/MainPlan/IndexData")!;
    }

    public Task<ApiResponse<List<MainPlanDto>>> GetAllByProjectIdAsync(Guid projectId)
    {
        return _http.GetFromJsonAsync<ApiResponse<List<MainPlanDto>>>($"api/MainPlan/Project/{projectId}")!;
    }
}