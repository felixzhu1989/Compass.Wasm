using Compass.Wpf.BatchWorks;
using SolidWorks.Interop.sldworks;

namespace Compass.Wpf.DrawingServices;

public class SharePartService : ISharePartService
{
    private readonly ISldWorks _swApp;
    public SharePartService(ISldWorksService sldWorksService)
    {
        _swApp = sldWorksService.SwApp;
    }




}