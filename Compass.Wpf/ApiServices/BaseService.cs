using System.Collections.Generic;
using Compass.Dtos;

namespace Compass.Wpf.ApiService;

public interface IBaseService<TEntity> where TEntity : class
{
    Task<ApiResponse<TEntity>> AddAsync(TEntity entity);
    Task<ApiResponse<TEntity>> UpdateAsync(Guid id, TEntity entity);
    Task<ApiResponse<TEntity>> DeleteAsync(Guid id);
    Task<ApiResponse<TEntity>> GetFirstOrDefault(Guid id);
    Task<ApiResponse<List<TEntity>>> GetAllAsync();
}

//抽象的普通数据请求类
public abstract class BaseService<TEntity> : IBaseService<TEntity> where TEntity : class
{
    private readonly HttpRestClient _client;
    private readonly string _serviceName;

    protected BaseService(HttpRestClient client, string serviceName)
    {
        _client = client;
        _serviceName = serviceName;
    }

    public async Task<ApiResponse<TEntity>> AddAsync(TEntity entity)
    {
        var request = new BaseRequest
        {
            Method = RestSharp.Method.Post,
            Route = $"api/{_serviceName}/Add",
            Param = entity
        };
        return await _client.ExecuteAsync<TEntity>(request);
    }

    public async Task<ApiResponse<TEntity>> UpdateAsync(Guid id, TEntity entity)
    {
        var request = new BaseRequest
        {
            Method = RestSharp.Method.Put,
            Route = $"api/{_serviceName}/{id}",
            Param = entity
        };
        return await _client.ExecuteAsync<TEntity>(request);
    }

    public async Task<ApiResponse<TEntity>> DeleteAsync(Guid id)
    {
        var request = new BaseRequest
        {
            Method = RestSharp.Method.Delete,
            Route = $"api/{_serviceName}/{id}"
        };
        return await _client.ExecuteAsync<TEntity>(request);
    }

    public async Task<ApiResponse<TEntity>> GetFirstOrDefault(Guid id)
    {
        var request = new BaseRequest
        {
            Method = RestSharp.Method.Get,
            Route = $"api/{_serviceName}/{id}"
        };
        return await _client.ExecuteAsync<TEntity>(request);
    }

    public async Task<ApiResponse<List<TEntity>>> GetAllAsync()
    {
        var request = new BaseRequest
        {
            Method = RestSharp.Method.Get,
            Route = $"api/{_serviceName}/All"
        };
        return await _client.ExecuteAsync<List<TEntity>>(request);
    }
}