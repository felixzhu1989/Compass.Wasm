using System.Threading.Tasks;
using Compass.Wasm.Shared.ProjectService;
using Compass.Wpf.Service.Hoods;
using SolidWorks.Interop.sldworks;

namespace Compass.Wpf.BatchWorks.Hoods;

public class KviAutoDrawing : IKviAutoDrawing
{
    private readonly IKviDataService _service;
    private readonly ISldWorks _swApp;

    public KviAutoDrawing(IKviDataService service, ISldWorksService sldWorksService)
    {
        _service = service;
        _swApp = sldWorksService.SwApp;
    }
    public async Task AutoDrawingAsync(ModuleDto moduleDto)
    {
        var dataResult = await _service.GetFirstOrDefault(moduleDto.Id.Value);
        var dataDto = dataResult.Result;//获取制图数据
        var modelDir = @"D:\halton\01 Tech Dept\05 Products Library\02 Hood\KVI555.SLDASM";







    }
}