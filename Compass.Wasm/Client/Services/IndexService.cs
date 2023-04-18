using System.Net.Http.Json;
using Compass.Wasm.Shared;
using Compass.Wasm.Shared.Plans;
using Compass.Wasm.Shared.Projects;

namespace Compass.Wasm.Client.Services;

public class IndexService : IIndexService
{
    private readonly HttpClient _http;
    public IndexService(HttpClient http)
    {
        _http = http;
    }

    public event Action? TrackingsChanged;
    public List<IndexModel> Trackings { get; set; } = new();
    public string Message { get; set; }
    public int CurrentPage { get; set; }
    public int PageCount { get; set; }
    public string LastSearchText { get; set; }
    public async Task GetTrackingsAsync(int page)
    {
        List<TrackingResponse> trackingResponses = new();
        var result =
            await _http.GetFromJsonAsync<ApiPaginationResponse<List<TrackingResponse>>>($"api/Tracking/All/{page}");
        if (result is { Result: { } }) trackingResponses = result.Result;
        CurrentPage = result.CurrentPage;
        PageCount = result.Pages;
        if (trackingResponses.Count == 0) Message = "No trackings found.";
        else await BuildTrackings(trackingResponses);
    }

    public async Task SearchTrackings(string searchText, int page)
    {
        LastSearchText = searchText;//查询字符串赋值
        List<TrackingResponse> trackingResponses = new();
        var result = await _http.GetFromJsonAsync<ApiPaginationResponse<List<TrackingResponse>>>($"api/Tracking/search/{searchText}/{page}");
        if (result is { Result: { } })
        {
            trackingResponses = result.Result;
            CurrentPage = result.CurrentPage;//当前页码
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
            var result = await _http.GetFromJsonAsync<ApiResponse<ProjectDto>>($"api/Project/{response.Id}");
            var project = result.Result;
            var trackingModel = new IndexModel
            {
                Id = response.Id,

                SortDate = response.SortDate,
                WarehousingTime = response.WarehousingTime,
                ShippingStartTime = response.ShippingStartTime,
                ShippingEndTime = response.ShippingEndTime,
                ProblemNotResolved = project.IsProblemNotResolved,
                #region Project
                OdpNumber = project.OdpNumber,
                ProjectName = project.Name,
                ProjectStatus = project.ProjectStatus,
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
            var problems = await _http.GetFromJsonAsync<List<ProblemResponse>>($"api/Problem/NotResolved/{response.Id}");
            trackingModel.Problems = problems;
            Trackings.Add(trackingModel);
        }
        TrackingsChanged?.Invoke();//发出属性变更事件，告诉事件订阅者，需要开始干活了
    }


    //public Task<List<string>> GetTrackingSearchSuggestions(string searchText)
    //{

    //}
}