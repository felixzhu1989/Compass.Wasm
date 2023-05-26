using Compass.Wpf.SwServices;
using SolidWorks.Interop.sldworks;

namespace Compass.Wpf.BatchWorks;

public class BaseAutoDrawing
{
    public readonly ISldWorks SwApp;
    public readonly IExhaustService ExhaustService;
    public readonly ISidePanelService SidePanelService;
    public readonly IMidRoofService MidRoofService;
    public readonly ISupplyService SupplyService;
    public readonly IEventAggregator Aggregator;

    public BaseAutoDrawing(IContainerProvider provider)
    {
        SwApp = provider.Resolve<ISldWorksService>().SwApp;
        ExhaustService = provider.Resolve<IExhaustService>();
        SidePanelService = provider.Resolve<ISidePanelService>();
        MidRoofService = provider.Resolve<IMidRoofService>();
        SupplyService = provider.Resolve<ISupplyService>();
        Aggregator = provider.Resolve<IEventAggregator>();

    }
}