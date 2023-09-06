using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Navigation.Xaml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;
using ZXing.Net.Mobile.Forms;

namespace Compass.Andro.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        #region ctor
        public MainPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            Title = "Main Page";
            ScannerCommand = new DelegateCommand(Scanner);
            UploadCommand = new DelegateCommand(Upload);
        }
        public DelegateCommand ScannerCommand { get; }
        public DelegateCommand UploadCommand { get; }
        #endregion

        #region 属性
        private string code;
        public string Code
        {
            get => code;
            set => SetProperty(ref code, value);
        }
        private string imagePath;
        public string ImagePath
        {
            get => imagePath;
            set => SetProperty(ref imagePath, value);
        }
        private ImageSource imageFile;
        public ImageSource ImageFile
        {
            get => imageFile;
            set => SetProperty(ref imageFile, value);
        }
        #endregion

        private async void Scanner()
        {
           await NavigationService.NavigateAsync("ScannerPage");

        }
        private async void Upload()
        {
            var photo = await MediaPicker.CapturePhotoAsync();
            if(photo==null)return;
            var type= photo.GetType();
            ImagePath = Path.Combine("/storage/emulated/0/Pictures", photo.FileName);
            using (var stream=await photo.OpenReadAsync())
            using (var newStream = File.OpenWrite(ImagePath))
            {
                await stream.CopyToAsync(newStream);
            }
            ImageFile = ImageSource.FromFile(ImagePath);
        }
        //接收导航到页面时传递的参数
        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            Code = parameters.GetValue<string>("value");
        }
    }
}
