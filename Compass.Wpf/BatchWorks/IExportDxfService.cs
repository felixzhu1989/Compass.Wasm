using Compass.Wasm.Shared.ProjectService;
using System.Threading.Tasks;

namespace Compass.Wpf.BatchWorks;

public interface IExportDxfService
{
    Task ExportHoodDxfAsync(ModuleDto moduleDto);
}