using Compass.Wasm.Shared.ProjectService;
using Compass.Wpf.Extensions;
using Prism.Commands;
using Prism.Ioc;
using Prism.Regions;
using System;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using Compass.Wpf.ApiService;

namespace Compass.Wpf.ViewModels;

public class DrawingViewModel : NavigationViewModel
{
    #region ctor-图纸页面，修改截图
    private readonly IDrawingService _drawingService;
    private readonly IFileUploadService _fileUploadService;
    public DrawingViewModel(IContainerProvider containerProvider) : base(containerProvider)
    {
        _drawingService = containerProvider.Resolve<IDrawingService>();
        _fileUploadService = containerProvider.Resolve<IFileUploadService>();
        ExecuteCommand = new DelegateCommand<string>(Execute);
    }
    #endregion

    #region 属性
    private DrawingDto currentDrawing = null!;
    public DrawingDto CurrentDrawing
    {
        get => currentDrawing;
        set { currentDrawing = value; RaisePropertyChanged(); }
    }
    private BitmapSource? oldImage;
    public BitmapSource? OldImage
    {
        get => oldImage;
        set { oldImage = value; RaisePropertyChanged(); }
    }
    private BitmapSource? imageSource;
    public BitmapSource? ImageSource
    {
        get => imageSource;
        set { imageSource = value; RaisePropertyChanged(); }
    }
    #endregion

    public DelegateCommand<string> ExecuteCommand { get; }

    #region 截图操作
    private void Execute(string obj)
    {
        switch (obj)
        {
            case "Paste": Paste(); break;
            case "Clear": Clear(); break;
            case "Save": Save(); break;
        }
    }

    #region 从剪切板粘贴图片
    private async void Paste()
    {
        if (!Clipboard.ContainsImage())
        {
            await DialogHost.Question("剪切板没有截图", "请使用截图工具（微信、QQ等）截图");
            return;
        }
        ImageSource = Clipboard.GetImage();
    }
    #endregion

    /// <summary>
    /// 清除
    /// </summary>
    private void Clear()
    {
        ImageSource=null;//清空图片
    }

    /// <summary>
    /// 保存
    /// </summary>
    private async void Save()
    {
        if (ImageSource != null)
        {
            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(ImageSource));
            await using var stream = new FileStream("temp.png", FileMode.Create);
            encoder.Save(stream);
            stream.Close();
            OldImage = ImageSource;
        }
        var imagePath = Path.Combine(Environment.CurrentDirectory, "temp.png");
        var result = await _fileUploadService.Upload(imagePath);
        CurrentDrawing.ImageUrl = result.RemoteUrl.ToString();

        var response = await _drawingService.UpdateAsync(CurrentDrawing.Id.Value, CurrentDrawing);
        if (!response.Status) return;
        Aggregator.SendMessage($"分段{CurrentDrawing.ItemNumber}修改成功！");
        Clear();//清空图片
    }
    #endregion

    #region 初始化
    public override void OnNavigatedTo(NavigationContext navigationContext)
    {
        base.OnNavigatedTo(navigationContext);
        CurrentDrawing=navigationContext.Parameters.ContainsKey("Value") ?
            navigationContext.Parameters.GetValue<DrawingDto>("Value")
            : new DrawingDto();
        OldImage = !string.IsNullOrEmpty(CurrentDrawing.ImageUrl) ? new BitmapImage(new Uri(CurrentDrawing.ImageUrl)) : null;
    }
    #endregion
}