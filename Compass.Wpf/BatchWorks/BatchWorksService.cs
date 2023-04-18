﻿using Compass.Wpf.Extensions;
using Prism.Events;
using Prism.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Compass.Wasm.Shared.Projects;

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
            #region 根据模型名称获取模型制图接口
            var name = $"I{moduleDto.ModelName.Split('_')[0]}autodrawing"; //构建接口
            var modelType =
                Interfaces.FirstOrDefault(x => x.Name.Contains(name, StringComparison.OrdinalIgnoreCase)); //匹配接口名
            var modelInterface = _provider.Resolve(modelType); //从容器中获取接口
            var model = modelInterface as IAutoDrawing; //将接口装入父接口中 
            #endregion

            if (!moduleDto.IsModuleDataOk)
            {
                _aggregator.SendMessage($"{moduleDto.ItemNumber}-{moduleDto.Name}-{moduleDto.ModelName}\t******制图参数不OK，跳过制图过程******", Filter_e.Batch);
                continue;
            }
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
            if (!moduleDto.IsFilesOk)
            {
                _aggregator.SendMessage($"{moduleDto.ItemNumber}-{moduleDto.Name}-{moduleDto.ModelName}\t******文件不存在,跳过导Dxf图******", Filter_e.Batch);
                continue;
            }
            if (moduleDto.IsCutListOk)
            {
                _aggregator.SendMessage($"{moduleDto.ItemNumber}-{moduleDto.Name}-{moduleDto.ModelName}\t******CutList已经存在，此操作会覆盖原CutList数据******", Filter_e.Batch);
            }
            await exportDxfService.ExportHoodDxfAsync(moduleDto);
            _aggregator.SendMessage("--------------------------------", Filter_e.Batch);
        }
    }

    /// <summary>
    /// 批量打印CutList
    /// </summary>
    public async Task BatchPrintCutListAsync(List<ModuleDto> moduleDtos)
    {
        var printService = _provider.Resolve<IPrintsService>();
        await printService.BatchPrintCutListAsync(moduleDtos);
    }

    /// <summary>
    /// 批量打印CutList
    /// </summary>
    public async Task BatchPrintJobCardAsync(List<ModuleDto> moduleDtos)
    {
        var printService = _provider.Resolve<IPrintsService>();
        await printService.BatchPrintJobCardAsync(moduleDtos);
    }



}