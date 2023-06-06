using SolidWorks.Interop.sldworks;
using System.Diagnostics;

namespace Compass.Wpf.BatchWorks;

public interface ISldWorksService
{
    ISldWorks SwApp { get; }
}

public class SldWorksService : ISldWorksService
{
    public ISldWorks SwApp { get; }
    private const string ProgId = "SldWorks.Application";
    public SldWorksService()
    {
        var swType =Type.GetTypeFromProgID(ProgId);
        SwApp=(ISldWorks)Activator.CreateInstance(swType!)!;
        SwApp.Visible=true;
        var swRev = Convert.ToInt32(SwApp.RevisionNumber()[..2]) - 8;
        Debug.Print($"SolidWorks 20{swRev}");
    }
}