using Prism.Navigation;
using Xamarin.Forms;
using ZXing;

namespace Compass.Andro.Views
{
    public partial class ScannerPage : ContentPage
    {
        private readonly INavigationService _navigationService;
        public ScannerPage(INavigationService navigationService)
        {
            _navigationService = navigationService;
            InitializeComponent();
        }

        public void Handle_OnScanResult(Result result)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                //弹窗显示扫描结果
                //await DisplayAlert("Scanned result", result.Text, "OK");
                //跳转到其他页面，todo:传递参数
                var navParam = new NavigationParameters()
                {
                    {"value",result.Text}
                };
                await _navigationService.NavigateAsync("MainPage", navParam);
            });
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            _scanView.IsScanning = true;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            _scanView.IsScanning = false;
        }

    }
}
