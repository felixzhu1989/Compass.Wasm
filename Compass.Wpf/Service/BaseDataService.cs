using Compass.Wasm.Shared;
using System.Threading.Tasks;
using System;

namespace Compass.Wpf.Service;

public class BaseDataService<TEntity> : IBaseDataService<TEntity> where TEntity : class
{
    private readonly HttpRestClient _client;
    private readonly string _serviceName;

    public BaseDataService(HttpRestClient client, string serviceName)
    {
        _client = client;
        _serviceName = serviceName;
    }
    public async Task<ApiResponse<TEntity>> UpdateAsync(Guid id, TEntity entity)
    {
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