using Compass.Wasm.Shared.Data;
using SolidWorks.Interop.sldworks;

namespace Compass.Wpf.DrawingServices;

public interface IMidRoofService
{
    void MidRoofFs(AssemblyDoc swAssyTop, string suffix, double length, double width, ExhaustType_e exhaustType, UvLightType_e uvLightType, bool bluetooth, double middleToRight, LightType_e lightType, int spotLightNumber, double spotLightDistance,bool marvel, bool ansul, int ansulDropNumber, double ansulDropToFront, double ansulDropDis1, double ansulDropDis2, double ansulDropDis3, double ansulDropDis4, double ansulDropDis5, int ansulDetectorNumber, AnsulDetectorEnd_e ansulDetectorEnd, double ansulDetectorDis1, double ansulDetectorDis2, double ansulDetectorDis3, double ansulDetectorDis4, double ansulDetectorDis5);
}