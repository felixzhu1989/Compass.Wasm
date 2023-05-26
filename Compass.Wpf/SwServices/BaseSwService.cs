using Compass.Wpf.BatchWorks;
using SolidWorks.Interop.sldworks;

namespace Compass.Wpf.SwServices;

public class BaseSwService
{
    public readonly ISldWorks SwApp;
    public readonly IEventAggregator Aggregator;
    public BaseSwService(IContainerProvider provider)
    {
        SwApp = provider.Resolve<ISldWorksService>().SwApp;
        Aggregator= provider.Resolve<IEventAggregator>();
    }
}