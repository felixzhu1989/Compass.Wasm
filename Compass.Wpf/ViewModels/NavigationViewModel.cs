using Prism.Mvvm;

namespace Compass.Wpf.ViewModels;

/// <summary>
/// 可导航页面的基类
/// </summary>
public class NavigationViewModel : BindableBase, INavigationAware
{
    public IRegionNavigationJournal Journal;
    public readonly IContainerProvider Provider;
    public readonly IEventAggregator Aggregator;//事件聚合器
    public readonly IRegionManager RegionManager;
    public readonly IDialogHostService DialogHost;
    public NavigationViewModel(IContainerProvider provider)
    {
        Provider = provider;
        RegionManager = provider.Resolve<IRegionManager>();
        DialogHost = provider.Resolve<IDialogHostService>();//给弹窗使用的服务
        Aggregator = provider.Resolve<IEventAggregator>();
    }
    public virtual bool IsNavigationTarget(NavigationContext navigationContext)
    {
        return true;//是否重用以前的窗口
    }
    public virtual void OnNavigatedTo(NavigationContext navigationContext)
    {
    }
    public virtual void OnNavigatedFrom(NavigationContext navigationContext)
    {
    }
    //发送消息，展开窗口
    public void UpdateLoading(bool IsOpen)
    {
        Aggregator.UpdateLoading(new Common.Events.UpdateModel { IsOpen = IsOpen });
    }

}