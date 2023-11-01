using SolidWorks.Interop.sldworks;

namespace Compass.Wpf.SwServices;
public interface IBeamService
{
	#region 排风腔
    void KcjDb800(ModelDoc2 swModelTop, AssemblyDoc swAssyTop, string suffix,string module, KcjData data);
    void KcjSb535(ModelDoc2 swModelTop, AssemblyDoc swAssyTop, string suffix, string module, KcjData data);
    void KcjSb290(ModelDoc2 swModelTop, AssemblyDoc swAssyTop, string suffix, string module, KcjData data);
    void KcjSb265(ModelDoc2 swModelTop, AssemblyDoc swAssyTop, string suffix, string module, KcjData data);
    void UcjDb800(ModelDoc2 swModelTop, AssemblyDoc swAssyTop, string suffix, string module, UcjData data);
    void UcjSb535(ModelDoc2 swModelTop, AssemblyDoc swAssyTop, string suffix, string module, UcjData data);
    void UcjSb385(ModelDoc2 swModelTop, AssemblyDoc swAssyTop, string suffix, string module, UcjData data);
    void KcwDb800(ModelDoc2 swModelTop, AssemblyDoc swAssyTop, string suffix, string module, KcwData data);
    void KcwSb535(ModelDoc2 swModelTop, AssemblyDoc swAssyTop, string suffix, string module, KcwData data);
    void KcwSb265(ModelDoc2 swModelTop, AssemblyDoc swAssyTop, string suffix, string module, KcwData data);
    void UcwDb800(ModelDoc2 swModelTop, AssemblyDoc swAssyTop, string suffix, string module, UcwData data);
    void UcwSb535(ModelDoc2 swModelTop, AssemblyDoc swAssyTop, string suffix, string module, UcwData data);
    #endregion
}