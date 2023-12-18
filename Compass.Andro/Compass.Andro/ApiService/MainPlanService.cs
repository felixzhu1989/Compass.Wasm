using System.Collections.Generic;
using System.Threading.Tasks;
using Compass.Andro.Dtos;
using Compass.Andro.Dtos.Plans;
using Newtonsoft.Json;
using RestSharp;

namespace Compass.Andro.ApiService
{
    public interface IMainPlanService
    {
        Task<List<MainPlanDto>> GetMainPlanDtosAsync();
    }
    public class MainPlanService:IMainPlanService
    {
        private RestClient restClient;
        public MainPlanService()
        {
            restClient=new RestClient("http://10.9.18.31");
        }
        private List<MainPlanDto> mainPlanDtos = new List<MainPlanDto>();
        public async Task<List<MainPlanDto>> GetMainPlanDtosAsync()
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
}
