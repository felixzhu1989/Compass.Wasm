using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Compass.Wasm.Shared.ProjectService;
using Compass.Wpf.Common;
using Prism.Ioc;

namespace Compass.Wpf.BatchWorks;

public class BatchWorksService:IBatchWorksService
{
    

    private readonly IContainerProvider _containerProvider;
    public List<Type> Interfaces { get;}
    public BatchWorksService(IContainerProvider containerProvider)
    {
        _containerProvider = containerProvider;
        //获取程序中所有的接口
        Interfaces =typeof(IAutoDrawing).Assembly.GetTypes().Where(x => x.IsInterface).ToList();
    }

    /// <summary>
    /// 自动绘图
    /// </summary>
    public Task BatchDrawingAsync(List<ModuleDto> moduleDtos)
    {
        OpenConsole("自动作图", async () =>
        {
            foreach (var moduleDto in moduleDtos)
            {
                var name = $"I{moduleDto.ModelName.Split('_')[0]}autodrawing"; //构建接口
                var modelType =
                    Interfaces.FirstOrDefault(x => x.Name.Contains(name, StringComparison.OrdinalIgnoreCase)); //匹配接口名
                var modelInterface = _containerProvider.Resolve(modelType); //从容器中获取接口
                var model = modelInterface as IAutoDrawing; //将接口装入父接口中

                await model!.AutoDrawingAsync(moduleDto);

            }
        });
        return Task.CompletedTask; 
    }

    /// <summary>
    /// 导出DXF图
    /// </summary>
    public Task BatchExportDxfAsync(List<ModuleDto> moduleDtos)
    {
        OpenConsole("导出Dxf", async () =>
        {
            //将对象延迟到调用时创建，防止卡死界面
            var exportDxfService = _containerProvider.Resolve<IExportDxfService>();
            foreach (var moduleDto in moduleDtos)
            {
                await exportDxfService.ExportHoodDxfAsync(moduleDto);
            }
        });
        return Task.CompletedTask;
    }

    public async Task BatchPrintCutListAsync(List<ModuleDto> moduleDtos)
    {
        var printService = _containerProvider.Resolve<IPrintsService>();
        await printService.BatchPrintCutListAsync(moduleDtos);
    }

    public Task BatchPrintJobCardAsync(List<ModuleDto> moduleDtos)
    {
        throw new NotImplementedException();
    }


    private void OpenConsole(string actionName, Action action)
    {
        //todo:显示控制台
        AppSession.AllocConsole();
        Console.WriteLine($"正在{actionName}，请勿关闭此窗口");
        action.Invoke();
        AppSession.FreeConsole();
    }
}