using System.Threading.Tasks;
using Compass.Wasm.Shared.Projects;

namespace Compass.Wpf.BatchWorks;

public interface IAutoDrawing
{
    Task AutoDrawingAsync(ModuleDto moduleDto);
}