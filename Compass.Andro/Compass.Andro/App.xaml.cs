using Compass.Andro.ApiService;
using Compass.Andro.ViewModels;
using Compass.Andro.Views;
using DryIoc;
using Prism;
using Prism.DryIoc;
using Prism.Ioc;
using Prism.Services.Dialogs;
using RestSharp;
using Xamarin.Essentials.Implementation;
using Xamarin.Essentials.Interfaces;
using Xamarin.Forms;
using ZXing.Net.Mobile.Forms;

namespace Compass.Andro
{
    public partial class App
    {
        public App(IPlatformInitializer initializer)
            : base(initializer)
        {
        }

        protected override async void OnInitialized()
        {
            InitializeComponent();
            //todo:��¼ҳ��
            
            //��������ҳ
            await NavigationService.NavigateAsync("NavigationPage/MainPage");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            #region ע��RestSharp����
            //��ȡ������Ȼ��ע��HttpRestClient���������캯������Ĭ��ֵ
            containerRegistry.GetContainer()
                .Register<HttpRestClient>(made: Parameters.Of.Type<string>(serviceKey: "apiUrl"));
            //��������API
            containerRegistry.GetContainer().RegisterInstance(@"http://10.9.18.31/", serviceKey: "apiUrl");
            #endregion

            containerRegistry.RegisterSingleton<IAppInfo, AppInfoImplementation>();
            containerRegistry.Register<IFileUploadService,FileUploadService>();
            containerRegistry.Register<IMainPlanService, MainPlanService>();



            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
            containerRegistry.RegisterForNavigation<ScannerPage, ScannerPageViewModel>();
        }
    }
}
