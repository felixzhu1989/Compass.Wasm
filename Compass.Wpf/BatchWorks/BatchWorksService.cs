using Compass.Wasm.Shared.ProjectService;
using Compass.Wpf.Extensions;
using Prism.Events;
using Prism.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Compass.Wpf.BatchWorks;

public class BatchWorksService : IBatchWorksService
{
    #region ctor
    private readonly IContainerProvider _provider;
    private readonly IEventAggregator _aggregator;
    public List<Type> Interfaces { get; }
    public BatchWorksService(IContainerProvider provider)
    {
        _provider = provider;
        _aggregator = provider.Resolve<IEventAggregator>();
        //获取程序中所有的接口
        Interfaces =typeof(IAutoDrawing).Assembly.GetTypes().Where(x => x.IsInterface).ToList();
    }
    #endregion

    /// <summary>
    /// 自动绘图
    /// </summary>
    public async Task BatchDrawingAsync(List<ModuleDto> moduleDtos)
    {
        foreach (var moduleDto in moduleDtos)
        {
            var name = $"I{moduleDto.ModelName.Split('_')[0]}autodrawing"; //构建接口
            var modelType =
                Interfaces.FirstOrDefault(x => x.Name.Contains(name, StringComparison.OrdinalIgnoreCase)); //匹配接口名
            var modelInterface = _provider.Resolve(modelType); //从容器中获取接口
            var model = modelInterface as IAutoDrawing; //将接口装入父接口中

            await model!.AutoDrawingAsync(moduleDto);
            _aggregator.SendMessage("--------------------------------", Filter_e.Batch);
        }
    }

    /// <summary>
    /// 导出DXF图
    /// </summary>
    public async Task BatchExportDxfAsync(List<ModuleDto> moduleDtos)
    {
        //将对象延迟到调用时创建，防止卡死界面
        var exportDxfService = _provider.Resolve<IExportDxfService>();
        foreach (var moduleDto in moduleDtos)
        {
            await exportDxfService.ExportHoodDxfAsync(moduleDto);
            _aggregator.SendMessage("--------------------------------", Filter_e.Batch);
        }
    }

    public async Task BatchPrintCutListAsync(List<ModuleDto> moduleDtos)
    {
        var printService = _provider.Resolve<IPrintsService>();
        await printService.BatchPrintCutListAsync(moduleDtos);
    }

    public Task BatchPrintJobCardAsync(List<ModuleDto> moduleDtos)
    {
        throw new NotImplementedException();
    }
}