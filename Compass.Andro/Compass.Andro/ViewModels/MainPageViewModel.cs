using Prism.Commands;
using Prism.Navigation;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Compass.Andro.ApiService;
using Prism.Ioc;
using Xamarin.Essentials;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Compass.Andro.Dtos.Plans;
using System.Threading.Tasks;
using System;

namespace Compass.Andro.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        #region ctor
        private readonly IFileUploadService _fileUploadService;
        private readonly IMainPlanService _mainPlanService;
        public MainPageViewModel(INavigationService navigationService, IContainerProvider provider)
            : base(navigationService)
        {
            _fileUploadService = provider.Resolve<IFileUploadService>();
            _mainPlanService = provider.Resolve<IMainPlanService>();

            Title = "Main Page";
            GetMainPlanDtosCommand = new DelegateCommand(async () => await GetMainPlanDtosAsync());
            ScannerCommand = new DelegateCommand(Scanner);
            UploadCommand = new DelegateCommand<string>(Upload);
            MultiImagesCommand = new DelegateCommand(MultiImages);
            PdfFileCommand = new DelegateCommand(PdfFile);
            Images = new ObservableCollection<ImageSource>();
            LogoImage=ImageSource.FromResource("Compass.Andro.Images.halton_logo.png");
        }


        public ICommand GetMainPlanDtosCommand { get; }
        public DelegateCommand ScannerCommand { get; }
        public DelegateCommand<string> UploadCommand { get; }
        public DelegateCommand MultiImagesCommand { get; }
        public DelegateCommand PdfFileCommand { get; }
        #endregion

        #region 属性
        public ObservableCollection<MainPlanDto> MainPlanDtos { get; } = new ObservableCollection<MainPlanDto>();

        private ImageSource logoImage;
        public ImageSource LogoImage
        {
            get => logoImage;
            set => SetProperty(ref logoImage, value);
        }
        private string code;
        public string Code
        {
            get => code;
            set => SetProperty(ref code, value);
        }
        private string uploadStatus;
        public string UploadStatus
        {
            get => uploadStatus;
            set => SetProperty(ref uploadStatus, value);
        }
        private ImageSource imageFile;
        public ImageSource ImageFile
        {
            get => imageFile;
            set => SetProperty(ref imageFile, value);
        }
        private ObservableCollection<ImageSource> images;
        public ObservableCollection<ImageSource> Images
        {
            get => images;
            set => SetProperty(ref images, value);
        }
        #endregion
        /// <summary>
        /// 扫描
        /// </summary>
        private async void Scanner()
        {
            await NavigationService.NavigateAsync("ScannerPage");

        }
        /// <summary>
        /// 上传图片
        /// </summary>
        private async void Upload(string obj)
        {
            FileResult photo = null;
            switch (obj)
            {
                case "CapturePhoto":
                    photo = await MediaPicker.CapturePhotoAsync();
                    //组合文件的地址
                    var localPath = Path.Combine("/storage/emulated/0/Pictures", photo.FileName);
                    //读取文件流，将图片保存到本地
                    using (var stream = await photo.OpenReadAsync())
                    using (var newStream = File.OpenWrite(localPath))
                    {
                        await stream.CopyToAsync(newStream);//将文件保存到目标地址
                        //stream.Position = 0;//重置流的读取位置
                        //ImageFile =ImageSource.FromStream(() => stream);//给图片的ImageSource读取流
                    }
                    break;
                case "PickPhoto":
                    photo = await MediaPicker.PickPhotoAsync();
                    break;
            }
            if (photo==null) return;

            Debug.Print(photo.FullPath);
            //上传到WebApi服务器
            var result = await _fileUploadService.Upload(photo.FullPath);
            UploadStatus = $"文件上传成功，地址：{result.RemoteUrl}";

            //using (var stream = await photo.OpenReadAsync())
            //{
            //    ImageFile =ImageSource.FromStream(() => stream);//给图片的ImageSource读取流
            //}
            //ImageFile = ImageSource.FromFile(photo.FullPath);//从文件读取
            ImageFile=ImageSource.FromUri(result.RemoteUrl);//从网络地址读取,如果图片太大会导致很慢

        }
        /// <summary>
        /// 选择多张图片展示
        /// </summary>
        private async void MultiImages()
        {
            //选择多个文件
            var pickResult = await FilePicker.PickMultipleAsync(
                new PickOptions
                {
                    FileTypes = FilePickerFileType.Images,
                    PickerTitle = "Pick image(s)"
                });
            if (pickResult==null) return;
            Images.Clear();
            foreach (var image in pickResult)
            {
                var bytes = File.ReadAllBytes(image.FullPath);
                Images.Add(ImageSource.FromStream(() => new MemoryStream(bytes)));
            }

            //选择单个文件添加到列表
            /*var photo = await MediaPicker.PickPhotoAsync();
            var bytes = File.ReadAllBytes(photo.FullPath);
            Images.Add(ImageSource.FromStream(() => new MemoryStream(bytes)));*/

        }
        /// <summary>
        /// 选择Pdf文件
        /// </summary>
        private async void PdfFile()
        {
            var customFileType = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>{{ DevicePlatform.Android, new[] { "application/pdf" } } });

            var pickResult = await FilePicker.PickAsync(
                new PickOptions
                {
                    FileTypes = customFileType,
                    PickerTitle = "Pick Pdf"
                });
            if (pickResult==null) return;
            UploadStatus = $"选择了文件：{pickResult.FileName}";
        }

        private async Task GetMainPlanDtosAsync()
        {
            
            try
            {
                var dtos = await _mainPlanService.GetMainPlanDtosAsync();
                if (MainPlanDtos.Count!=0) MainPlanDtos.Clear();
                foreach (var dto in dtos)
                    MainPlanDtos.Add(dto);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                await Shell.Current.DisplayAlert("Error!", $"Unable to get mainplans:{ex.Message}", "OK");
            }
            
        }


        //接收导航到页面时传递的参数
        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            Code = parameters.GetValue<string>("value");
        }
    }
}
