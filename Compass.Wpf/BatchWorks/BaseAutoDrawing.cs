﻿using SolidWorks.Interop.sldworks;

namespace Compass.Wpf.BatchWorks;
using System.Threading.Tasks;

public interface IAutoDrawing
{
    Task AutoDrawingAsync(ModuleDto moduleDto);
}

public abstract class BaseAutoDrawing
{
    public readonly ISldWorks SwApp;
    public readonly IExhaustService ExhaustService;
    public readonly ISidePanelService SidePanelService;
    public readonly IMidRoofService MidRoofService;
    public readonly ISupplyService SupplyService;
    public readonly IBeamService BeamService;
    public readonly ICeilingService CeilingService;


    public readonly IEventAggregator Aggregator;
    protected BaseAutoDrawing(IContainerProvider provider)
    {
        ExhaustService = provider.Resolve<IExhaustService>();
        SidePanelService = provider.Resolve<ISidePanelService>();
        MidRoofService = provider.Resolve<IMidRoofService>();
        SupplyService = provider.Resolve<ISupplyService>();
        BeamService = provider.Resolve<IBeamService>();
        CeilingService = provider.Resolve<ICeilingService>();


        Aggregator = provider.Resolve<IEventAggregator>();
        SwApp = SwUtility.ConnectSw(Aggregator);
    }
}