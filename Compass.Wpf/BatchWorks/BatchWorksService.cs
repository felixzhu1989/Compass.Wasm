using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Compass.Wasm.Shared;
using Compass.Wasm.Shared.ProjectService;
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
        Interfaces=typeof(IAutoDrawing).Assembly.GetTypes().Where(x => x.IsInterface).ToList();
    }
    public async Task<ApiResponse<bool>> BatchDrawingAsync(List<ModuleDto> moduleDtos)
    {
        try
        {
            foreach (var moduleDto in moduleDtos)
            {
                var name = $"I{moduleDto.ModelName.Split('-')[0]}autodrawing";//构建接口
                var modelType =
                    Interfaces.FirstOrDefault(x => x.Name.Contains(name, StringComparison.OrdinalIgnoreCase));//匹配接口名
                var modelInterface = _containerProvider.Resolve(modelType);//从容器中获取接口
                var model = modelInterface as IAutoDrawing;//将接口装入父接口中
                await model!.AutoDrawingAsync(moduleDto);
            }
            return new ApiResponse<bool> { Status = true, Result = true };
        }
        catch (Exception e)
        {
            return new ApiResponse<bool> { Status = false, Message = e.Message};
        }
    }
}