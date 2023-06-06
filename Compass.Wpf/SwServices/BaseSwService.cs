using Compass.Wpf.BatchWorks;
using SolidWorks.Interop.sldworks;

namespace Compass.Wpf.SwServices;
//抽象类，为了复用代码
public abstract class BaseSwService
{
    public readonly ISldWorks SwApp;
    public readonly IEventAggregator Aggregator;
    protected BaseSwService(IContainerProvider provider)
    {
        SwApp = provider.Resolve<ISldWorksService>().SwApp;
        Aggregator= provider.Resolve<IEventAggregator>();
    }
}