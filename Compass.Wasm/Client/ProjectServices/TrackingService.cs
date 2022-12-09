using System.Net.Http.Json;
using Compass.Wasm.Shared;
using Compass.Wasm.Shared.PlanService;
using Compass.Wasm.Shared.ProjectService;

namespace Compass.Wasm.Client.ProjectServices;

public class TrackingService : ITrackingService
{
    private readonly HttpClient _http;
    public TrackingService(HttpClient http)
    {
        _http = http;
    }

    public event Action? TrackingsChanged;
    public List<TrackingModel> Trackings { get; set; } = new();
    public string Message { get; set; }
    public int CurrentPage { get; set; }
    public int PageCount { get; set; }
    public string LastSearchText { get; set; }
    public async Task GetTrackingsAsync(int page)
    {
        List<TrackingResponse> trackingResponses = new();
        var result =
            await _http.GetFromJsonAsync<PaginationResult<List<TrackingResponse>>>($"api/Tracking/All/{page}");
        if (result is { Data: { } }) trackingResponses = result.Data;
        CurrentPage = 1;
        PageCount = 0;
        if (trackingResponses.Count == 0) Message = "No trackings found.";
        else await BuildTrackings(trackingResponses);
    }

    public async Task SearchTrackings(string searchText, int page)
    {
        LastSearchText = searchText;//查询字符串赋值
        List<TrackingResponse> trackingResponses = new();
        var result = await _http.GetFromJsonAsync<PaginationResult<List<TrackingResponse>>>($"api/Tracking/search/{searchText}/{page}");
        if (result is { Data: { } })
        {
            trackingResponses = result.Data;
            CurrentPage=result.CurrentPage;//当前页码
            PageCount = result.Pages;//总页数
        }
        if (trackingResponses.Count == 0) Message = "No trackings found.";
        else await BuildTrackings(trackingResponses);
    }

    private async Task BuildTrackings(List<TrackingResponse> trackingResponses)
    {
        Trackings.Clear();
        //构建Index页面Tracking模型
        foreach (var response in trackingResponses)
        {
            var project = await _http.GetFromJsonAsync<ProjectResponse>($"api/Project/{response.Id}");

            var trackingModel = new TrackingModel
            {
                Id = response.Id,
                ProjectStatus = response.ProjectStatus,
                SortDate = response.SortDate,
                WarehousingTime = response.WarehousingTime,
                ShippingStartTime = response.ShippingTime,
                ShippingEndTime = response.ClosedTime,
                ProblemNotResolved = response.ProblemNotResolved,
                #region Project
                OdpNumber = project.OdpNumber,
                ProjectName = project.Name,
                #endregion
            };
            var prodPlanResult = await _http.GetAsync($"api/ProductionPlan/ProjectId/{response.Id}");
            if (prodPlanResult.IsSuccessStatusCode)
            {
                var prodPlan = await prodPlanResult.Content.ReadFromJsonAsync<ProductionPlanResponse>();
                trackingModel.ProductionPlanOk = true;
                trackingModel.OdpReleaseTime = prodPlan.OdpReleaseTime;
                trackingModel.ProductionFinishTime = prodPlan.ProductionFinishTime;
                trackingModel.DrawingReleaseTarget = prodPlan.DrawingReleaseTarget;
                trackingModel.DrawingReleaseActual = prodPlan.DrawingReleaseActual;
            }
            else
            {
                trackingModel.ProductionPlanOk = false;
            }
            Trackings.Add(trackingModel);
        }
        TrackingsChanged?.Invoke();//发出属性变更事件，告诉事件订阅者，需要开始干活了
    }


    //public Task<List<string>> GetTrackingSearchSuggestions(string searchText)
    //{

    //}
}