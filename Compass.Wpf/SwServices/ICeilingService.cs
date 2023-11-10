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
    void Dp340(ModelDoc2 swModelTop, AssemblyDoc swAssyTop, string suffix, string module, DpData data);
    void DpCj330(ModelDoc2 swModelTop, AssemblyDoc swAssyTop, string suffix, string module, DpData data);
    #endregion

    #region LFU
    void LfuSa(ModelDoc2 swModelTop, AssemblyDoc swAssyTop, string suffix, string module, LfuData data);
    void LfuSc(ModelDoc2 swModelTop, AssemblyDoc swAssyTop, string suffix, string module, LfuData data);
    void LfuSs(ModelDoc2 swModelTop, AssemblyDoc swAssyTop, string suffix, string module, LfuData data);

    #endregion



}