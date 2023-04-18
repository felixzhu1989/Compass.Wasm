using System.Threading.Tasks;
using Compass.Wasm.Shared.Projects;

namespace Compass.Wpf.BatchWorks;

public interface IExportDxfService
{
    Task ExportHoodDxfAsync(ModuleDto moduleDto);
}