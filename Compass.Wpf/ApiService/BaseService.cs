using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Compass.Wasm.Shared;

namespace Compass.Wpf.ApiService;

public class BaseService<TEntity> : IBaseService<TEntity> where TEntity : class
{
    private readonly HttpRestClient _client;
    private readonly string _serviceName;

    public BaseService(HttpRestClient client, string serviceName)
    {
        _client = client;
        _serviceName = serviceName;
    }

    public async Task<ApiResponse<TEntity>> AddAsync(TEntity entity)
    {
        BaseRequest request = new BaseRequest
        {
            Method = RestSharp.Method.Post,
            Route = $"api/{_serviceName}/Add",
            Parameter = entity
        };
        return await _client.ExecuteAsync<TEntity>(request);
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

    public async Task<ApiResponse<TEntity>> DeleteAsync(Guid id)
    {
        BaseRequest request = new BaseRequest
        {
            Method = RestSharp.Method.Delete,
            Route = $"api/{_serviceName}/{id}"
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

    public async Task<ApiResponse<List<TEntity>>> GetAllAsync()
    {
        BaseRequest request = new BaseRequest
        {
            Method = RestSharp.Method.Get,
            Route = $"api/{_serviceName}/All"
        };
        return await _client.ExecuteAsync<List<TEntity>>(request);
    }
}