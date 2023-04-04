using Compass.Wpf.Common.Models;
using Compass.Wpf.Extensions;
using Prism.Commands;
using Prism.Ioc;
using Prism.Regions;
using System.Collections.ObjectModel;

namespace Compass.Wpf.ViewModels;

public class SettingsViewModel : NavigationViewModel
{
    #region ctor-设置界面
    public SettingsViewModel(IContainerProvider provider) : base(provider)
    {
        MenuBars = new ObservableCollection<MenuBar>();
        CreateMenuBar();
        NavigateCommand = new DelegateCommand<MenuBar>(Navigate);
    }
    public DelegateCommand<MenuBar> NavigateCommand { get; }
    #endregion

    #region 属性
    private ObservableCollection<MenuBar> menuBars;
    public ObservableCollection<MenuBar> MenuBars
    {
        get => menuBars;
        set { menuBars = value; RaisePropertyChanged(); }
    }
    #endregion
    
    #region 导航
    private void Navigate(MenuBar? obj)
    {
        if (obj==null||string.IsNullOrWhiteSpace(obj.NameSpace)) return;
        RegionManager.Regions[PrismManager.SettingsViewRegionName].RequestNavigate(obj.NameSpace, back =>
        {
            Journal = back.Context.NavigationService.Journal;
        });
    }
    #endregion

    #region 初始化
    /// <summary>
    /// 初始化菜单集合
    /// </summary>
    void CreateMenuBar()
    {
        MenuBars.Add(new MenuBar { Icon = "Cog", Title = "系统设置", NameSpace = "" });
        MenuBars.Add(new MenuBar { Icon = "Information", Title = "关于更多", NameSpace = "AboutView" });
    } 
    #endregion
}