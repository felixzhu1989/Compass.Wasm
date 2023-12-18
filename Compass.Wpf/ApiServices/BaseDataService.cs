using Compass.Wasm.Shared;

namespace Compass.Wpf.ApiServices;

public interface IBaseDataService<TEntity> where TEntity : class
{
    Task<ApiResponse<TEntity>> UpdateAsync(Guid id, TEntity entity);
    Task<ApiResponse<TEntity>> GetFirstOrDefault(Guid id);
}

//抽象的模型数据请求类，为了给模型继承使用
public abstract class BaseDataService<TEntity> : IBaseDataService<TEntity> where TEntity : class
{
    private readonly HttpRestClient _client;
    private readonly string _serviceName;
    private readonly IModuleService _moduleService;

    protected BaseDataService(HttpRestClient client, string serviceName, IModuleService moduleService)
    {
        _client = client;
        _serviceName = serviceName;
        _moduleService = moduleService;
    }
    public async Task<ApiResponse<TEntity>> UpdateAsync(Guid id, TEntity entity)
    {
        //修改分段数据的同时将分段中数据标记为OK
        var result = await _moduleService.GetFirstOrDefault(id);
        if (result.Status)
        {
            result.Result.IsModuleDataOk = true;
            await _moduleService.UpdateAsync(id, result.Result);
        }
        BaseRequest request = new BaseRequest
        {
            Method = RestSharp.Method.Put,
            Route = $"api/{_serviceName}/{id}",
            Param = entity
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