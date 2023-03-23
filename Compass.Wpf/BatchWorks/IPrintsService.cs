using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Media;
using Compass.Wasm.Shared.ProjectService;

namespace Compass.Wpf.BatchWorks;

public interface IPrintsService
{
    Task BatchPrintCutListAsync(List<ModuleDto> moduleDtos);
    Task PrintOneCutListAsync(ModuleDto moduleDto);
    


}