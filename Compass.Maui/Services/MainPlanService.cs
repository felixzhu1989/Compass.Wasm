using System.Net.Http.Json;
using Compass.Wasm.Shared;
using Newtonsoft.Json;
using RestSharp;


namespace Compass.Maui.Services;

public class MainPlanService
{
    private HttpClient httpClient;
    private RestClient restClient;
    public MainPlanService()
    {
        httpClient=new HttpClient();
        restClient=new RestClient("http://10.9.18.31");
    }

    private List<MainPlanDto> mainPlanDtos=new();

    public async Task<List<MainPlanDto>> GetMainPlanDtosAsync()
    {
        if (mainPlanDtos.Count !=0) mainPlanDtos.Clear();
        var url = "http://10.9.18.31/api/MainPlan/All";
        var response= await httpClient.GetAsync(url);
        if (!response.IsSuccessStatusCode) return mainPlanDtos;
        var result= await response.Content.ReadFromJsonAsync<ApiResponse<List<MainPlanDto>>>();
        mainPlanDtos.AddRange(result.Result);
        return mainPlanDtos;
    }

    public async Task<List<MainPlanDto>> GetMainPlanDtosRestAsync()
    {
        if (mainPlanDtos.Count !=0) mainPlanDtos.Clear();
        var request = new RestRequest("api/MainPlan/All", Method.Get);
        var response = await restClient.ExecuteAsync(request);
        if (!response.IsSuccessStatusCode) return mainPlanDtos;
        var result = JsonConvert.DeserializeObject<ApiResponse<List<MainPlanDto>>>(response.Content);
        mainPlanDtos.AddRange(result.Result);
        return mainPlanDtos;
    }
}