using System.Threading.Tasks;

namespace Compass.Wpf.BatchWorks;

public interface IAutoDrawing
{
    Task AutoDrawingAsync(ModuleDto moduleDto);
}