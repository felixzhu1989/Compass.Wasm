using System.Collections.ObjectModel;
using Compass.Wpf.Common;
using Compass.Wpf.Common.Models;
using Compass.Wpf.Extensions;
using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;

namespace Compass.Wpf.ViewModels;
public class MainViewModel : NavigationViewModel, IConfigureService
{
    #region ctor-主界面
    public MainViewModel(IContainerProvider provider):base(provider)
    {
        MenuBars = new ObservableCollection<MenuBar>();
        //从菜单中添加导航
        NavigateCommand = new DelegateCommand<MenuBar>(Navigate);
        GoBackCommand = new DelegateCommand(() =>
        {
            if (Journal is { CanGoBack: true }) Journal.GoBack();
        });
        GoForwardCommand = new DelegateCommand(() =>
        {
            if (Journal is { CanGoForward: true }) Journal.GoForward();
        });
        HomeCommand = new DelegateCommand(() =>
        {
            RegionManager.Regions[PrismManager.MainViewRegionName].RequestNavigate("IndexView", back => { Journal = back.Context.NavigationService.Journal; });
        });
        LogoutCommand=new DelegateCommand(() => { App.Logout(provider, RegionManager); });//注销登录
    }
    #endregion

    #region 属性
    private ObservableCollection<MenuBar> menuBars = null!;
    /// <summary>
    /// 菜单集合
    /// </summary>
    public ObservableCollection<MenuBar> MenuBars
    {
        get => menuBars;
        set { menuBars = value; RaisePropertyChanged(); }
    }
    //用于绑定菜单栏显示用户名
    private string userName = null!;
    public string UserName
    {
        get => userName;
        set { userName = value; RaisePropertyChanged(); }
    }
    #endregion

    #region Commands
    public DelegateCommand<MenuBar> NavigateCommand { get; }
    public DelegateCommand GoBackCommand { get; }
    public DelegateCommand GoForwardCommand { get; }
    public DelegateCommand HomeCommand { get; }
    //退出登录
    public DelegateCommand LogoutCommand { get; }
    #endregion

    /// <summary>
    /// 导航上下页面
    /// </summary>
    /// <param name="obj"></param>
    private void Navigate(MenuBar? obj)
    {
        UserName=AppSession.UserName;
        if (obj==null||string.IsNullOrWhiteSpace(obj.NameSpace)) return;
        RegionManager.Regions[PrismManager.MainViewRegionName].RequestNavigate(obj.NameSpace, back =>
        {
            Journal = back.Context.NavigationService.Journal;
        });
    }

    /// <summary>
    /// 初始化菜单集合
    /// </summary>
    void CreateMenuBar()
    {
        MenuBars.Add(new MenuBar { Icon = "Home", Title = "首页", NameSpace = "IndexView" });
        MenuBars.Add(new MenuBar { Icon = "Notebook", Title = "待办事项", NameSpace = "TodoView" });
        MenuBars.Add(new MenuBar { Icon = "NotebookPlus", Title = "备忘录", NameSpace = "MemoView" });
        MenuBars.Add(new MenuBar { Icon = "Cog", Title = "设置", NameSpace = "SettingsView" });
    }

    /// <summary>
    /// 初始化配置默认首页
    /// </summary>
    public void Configure()
    {
        CreateMenuBar(); //模拟生成菜单数据，初始化菜单集合
        UserName=AppSession.UserName;//初始化用户名
        RegionManager.Regions[PrismManager.MainViewRegionName].RequestNavigate("IndexView", back => { Journal = back.Context.NavigationService.Journal; });
    }
}
