using CommunityToolkit.Mvvm.ComponentModel;
using Compass.Maui.ApiServices;
using Compass.Wasm.Shared.Plans;
using System.Collections.ObjectModel;
using Compass.Maui.Extensions;
using RestSharp;
using Compass.Dtos;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Json;
using CommunityToolkit.Mvvm.Input;

namespace Compass.Maui.ViewModels;

public class MainViewModel:ObservableObject, IQueryAttributable
{
    #region ctor
    private readonly IServiceProvider _provider;
    private readonly IMainPlanService _mainPlanService;
    public MainViewModel(IServiceProvider provider)
    {
        _provider = provider;
        _mainPlanService = provider.GetService<IMainPlanService>();
        MainPlanDtos = new ObservableCollection<MainPlanDto>();
       
    }
    #endregion

    private AsyncRelayCommand showCommand;
    public AsyncRelayCommand ShowCommand => showCommand ??= new AsyncRelayCommand(async () => { await GetDataAsync();});
    private AsyncRelayCommand linkCommand;
    public AsyncRelayCommand LinkCommand => linkCommand ??= new AsyncRelayCommand(async () => { await Launcher.Default.OpenAsync("http://10.9.18.31"); });
    public ObservableCollection<MainPlanDto> MainPlanDtos { get; }

    public async void ApplyQueryAttributes(IDictionary<string, object> query)
    {

      
        
    }




    public async Task GetDataAsync()
    {
        var httpClient = new HttpClient();//先使用不规范的方式试试
        var response = await httpClient.GetAsync("http://10.9.18.31/api/MainPlan/All");
        var result = await response.Content.ReadFromJsonAsync<ApiResponse<List<MainPlanDto>>>();
        MainPlanDtos.Clear();
        MainPlanDtos.AddRange(result.Result);
    }
    

}