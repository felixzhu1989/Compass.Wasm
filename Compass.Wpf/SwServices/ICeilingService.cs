using SolidWorks.Interop.sldworks;

namespace Compass.Wpf.SwServices;

public interface ICeilingService
{
	#region CJ腔
    void Cj300(ModelDoc2 swModelTop, AssemblyDoc swAssyTop, string suffix, string module, CjData data);
    void Cj330(ModelDoc2 swModelTop, AssemblyDoc swAssyTop, string suffix, string module, CjData data);
    



    #endregion


}