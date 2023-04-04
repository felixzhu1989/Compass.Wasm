using Prism.Ioc;

namespace Compass.Wpf.DrawingServices;

public class SharePartService : BaseDrawingService, ISharePartService
{
    public SharePartService(IContainerProvider provider) : base(provider)
    {
    }


}