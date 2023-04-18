using System.Collections.Generic;
using System.Threading.Tasks;
using Compass.Wasm.Shared.Projects;

namespace Compass.Wpf.BatchWorks;

public interface IPrintsService
{
    Task BatchPrintCutListAsync(List<ModuleDto> moduleDtos);
    Task PrintOneCutListAsync(ModuleDto moduleDto);

    Task BatchPrintJobCardAsync(List<ModuleDto> moduleDtos);
    Task PrintOneJobCardAsync(ModuleDto moduleDto);



}