using SolidWorks.Interop.sldworks;

namespace Compass.Wpf.SwServices;

public interface ICeilingService
{
	#region CJ腔
    void Cj300(ModelDoc2 swModelTop, AssemblyDoc swAssyTop, string suffix, string module, CjData data);
    void Cj330(ModelDoc2 swModelTop, AssemblyDoc swAssyTop, string suffix, string module, CjData data);

    void Bcj300(ModelDoc2 swModelTop, AssemblyDoc swAssyTop, string suffix, string module, CjData data);
    void Bcj330(ModelDoc2 swModelTop, AssemblyDoc swAssyTop, string suffix, string module, CjData data);

    void Nocj300(ModelDoc2 swModelTop, AssemblyDoc swAssyTop, string suffix, string module, CjData data);
    void Nocj330(ModelDoc2 swModelTop, AssemblyDoc swAssyTop, string suffix, string module, CjData data);
    void Nocj340(ModelDoc2 swModelTop, AssemblyDoc swAssyTop, string suffix, string module, CjData data);

    void Dp330(ModelDoc2 swModelTop, AssemblyDoc swAssyTop, string suffix, string module, DpData data);


    #endregion


}