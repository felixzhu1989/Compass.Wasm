using System.Windows.Input;
using Compass.Update;

namespace Compass.Wpf.Views;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainView : Window
{
    private readonly IDialogHostService _dialogHost;
    public MainView(IEventAggregator aggregator, IDialogHostService dialogHost)
    {
        _dialogHost = dialogHost;
        InitializeComponent();
        //注册snackbar提示消息,只订阅来自Main的消息，默认的消息。
        aggregator.RegisterMessage(arg =>
        {
            Snackbar.MessageQueue!.Enqueue(arg.Message);//往消息队列中添加消息
        });

        //注册等待消息窗口
        //aggregator.Register(arg =>
        //{
        //    DialogHost.IsOpen = arg.IsOpen;
        //    if (DialogHost.IsOpen) DialogHost.DialogContent = new ProcessView();
        //});


        //移动窗体
        ColorZone.MouseMove += (s, e) =>
        {
            if (e.LeftButton == MouseButtonState.Pressed) DragMove();
        };
        //最大化，最小化窗体
        ColorZone.MouseDoubleClick += (s, e) =>
        {
            WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
        };
        BtnMin.Click += (s, e) => { WindowState = WindowState.Minimized; };
        BtnMax.Click += (s, e) =>
        {
            WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
        };
        //点击了菜单后让菜单面板收缩
        MenuBar.SelectionChanged += (s, e) =>
        {
            DrawerHost.IsLeftDrawerOpen = false;
        };

        //系统退出询问
        BtnClose.Click += async (s, e) =>
        {
            var dialogResult = await _dialogHost.Question("温馨提示", "确认退出系统吗");
            if (dialogResult.Result != ButtonResult.OK) return;
            Close();
        };
        
        this.Loaded += (s, e) =>
        {
            var updateMgr = new UpdateManager();
            if(!updateMgr.NeedUpdate)return;
            Close();
            //启动升级程序
            System.Diagnostics.Process.Start("Compass.Update.exe");
        };


        BtnUpdate.Click += async (s, e) =>
        {
            var dialogResult = await dialogHost.Question("系统升级","确认要现在升级吗?");
            if (dialogResult.Result != ButtonResult.OK) return;
            Close();
            //启动升级程序
            System.Diagnostics.Process.Start("Compass.Update.exe");
        };
    }
}