using System.Threading.Tasks;
using Compass.Wasm.Shared.ProjectService;

namespace Compass.Wpf.BatchWorks;

public interface IAutoDrawing
{
    Task AutoDrawingAsync(ModuleDto moduleDto);
}