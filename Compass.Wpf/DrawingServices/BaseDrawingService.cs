using Compass.Wpf.BatchWorks;
using SolidWorks.Interop.sldworks;

namespace Compass.Wpf.DrawingServices;

public class BaseDrawingService
{
    public readonly ISldWorks SwApp;
    public readonly IEventAggregator Aggregator;
    public BaseDrawingService(IContainerProvider provider)
    {
        SwApp = provider.Resolve<ISldWorksService>().SwApp;
        Aggregator= provider.Resolve<IEventAggregator>();
    }
}