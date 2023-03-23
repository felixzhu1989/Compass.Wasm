using Compass.Wasm.Shared;
using System.Threading.Tasks;
using System;

namespace Compass.Wpf.Service;

public class BaseDataService<TEntity> : IBaseDataService<TEntity> where TEntity : class
{
    private readonly HttpRestClient _client;
    private readonly string _serviceName;
    private readonly IModuleService _moduleService;

    public BaseDataService(HttpRestClient client, string serviceName,IModuleService moduleService)
    {
        _client = client;
        _serviceName = serviceName;
        _moduleService = moduleService;
    }
    public async Task<ApiResponse<TEntity>> UpdateAsync(Guid id, TEntity entity)
    {
        //修改分段数据的同时将分段中数据标记为OK
        var result =await _moduleService.GetFirstOrDefault(id);
        if (result.Status)
        {
            result.Result.IsModuleDataOk=true;
            await _moduleService.UpdateAsync(id, result.Result);
        }
        BaseRequest request = new BaseRequest
        {
            Method = RestSharp.Method.Put,
            Route = $"api/{_serviceName}/{id}",
            Parameter = entity
        };
        return await _client.ExecuteAsync<TEntity>(request);
    }
    public async Task<ApiResponse<TEntity>> GetFirstOrDefault(Guid id)
    {
        BaseRequest request = new BaseRequest
        {
            Method = RestSharp.Method.Get,
            Route = $"api/{_serviceName}/{id}"
        };
        return await _client.ExecuteAsync<TEntity>(request);
    }

}