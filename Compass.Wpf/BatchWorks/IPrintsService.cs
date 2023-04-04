using Compass.Wasm.Shared.ProjectService;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Compass.Wpf.BatchWorks;

public interface IPrintsService
{
    Task BatchPrintCutListAsync(List<ModuleDto> moduleDtos);
    Task PrintOneCutListAsync(ModuleDto moduleDto);



}