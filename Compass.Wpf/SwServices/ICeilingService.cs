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

    #region An(Gutter)
    void An135(ModelDoc2 swModelTop, AssemblyDoc swAssyTop, string suffix, string module, AnData data);
    #endregion

    #region SSP
    void SspFlat(ModelDoc2 swModelTop, AssemblyDoc swAssyTop, string suffix, string module, SspData data);
    void SspDome(ModelDoc2 swModelTop, AssemblyDoc swAssyTop, string suffix, string module, SspData data);
    void SspHalf(ModelDoc2 swModelTop, AssemblyDoc swAssyTop, string suffix, string module, SspData data);
    void SspDLk(ModelDoc2 swModelTop, AssemblyDoc swAssyTop, string suffix, string module, SspData data);
    void SspSLk(ModelDoc2 swModelTop, AssemblyDoc swAssyTop, string suffix, string module, SspData data);

    #endregion

    #region LK
    void LightPanelSsGlass(AssemblyDoc swAssyLevel1, string suffx, string module, double totalLength, int longGlassNumber, int shortGlassNumber);
    void LightPanelSsLed(AssemblyDoc swAssyLevel1, string suffx, string module, double totalLength);
    void LightPanelSsJapan(AssemblyDoc swAssyLevel1, string suffx, string module, double totalLength, int longGlassNumber, int shortGlassNumber, double leftLength, double rightLength, double middleLength);
    void LkSs270(ModelDoc2 swModelTop, AssemblyDoc swAssyTop, string suffix, string module, LkData data);
    void LkSt270(ModelDoc2 swModelTop, AssemblyDoc swAssyTop, string suffix, string module, LkData data);

    void LkAl258(ModelDoc2 swModelTop, AssemblyDoc swAssyTop, string suffix, string module, LkData data);
    #endregion

    #region LP,INF
    void LpZ(ModelDoc2 swModelTop, AssemblyDoc swAssyTop, string suffix, string module, LpData data);
    void Inf(ModelDoc2 swModelTop, AssemblyDoc swAssyTop, string suffix, string module, InfData data);

    #endregion
}