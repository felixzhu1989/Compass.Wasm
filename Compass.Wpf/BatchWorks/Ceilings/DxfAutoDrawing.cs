namespace Compass.Wpf.BatchWorks.Ceilings;

public interface IDxfAutoDrawing : IAutoDrawing
{

}

public class DxfAutoDrawing:BaseAutoDrawing,IDxfAutoDrawing
{
    public DxfAutoDrawing(IContainerProvider provider) : base(provider)
    {
    }

    public Task AutoDrawingAsync(ModuleDto moduleDto)
    {
        //无需作图,空白实现
        return Task.CompletedTask;
    }
}