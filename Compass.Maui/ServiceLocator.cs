using Compass.Maui.ApiServices;
using Compass.Maui.ViewModels;

namespace Compass.Maui;
/// <summary>
/// 服务定位器
/// </summary>
public class ServiceLocator
{
    private IServiceProvider _provider;
    public ServiceLocator()
    {
        var sc = new ServiceCollection();
        //sc.AddSingleton<HttpRestClient>();


        //ViewModel
        sc.AddSingleton<MainViewModel>();
        //sc.AddScoped<IMainPlanService, MainPlanService>();

        _provider = sc.BuildServiceProvider();

    }
    //依赖注入，IOC
    public MainViewModel MainViewModel => _provider.GetService<MainViewModel>();



}