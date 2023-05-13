using System.Collections.Generic;
using System.Threading.Tasks;

namespace Compass.Wpf.BatchWorks;
public interface IBatchWorksService
{
    //作图
    Task BatchDrawingAsync(List<ModuleDto> moduleDtos);
    //导出DXF
    Task BatchExportDxfAsync(List<ModuleDto> moduleDtos);
    //CutList
    Task BatchPrintCutListAsync(List<ModuleDto> moduleDtos);
    //JobCard
    Task BatchPrintJobCardAsync(List<ModuleDto> moduleDtos);
}