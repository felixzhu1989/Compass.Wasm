using Compass.Wasm.Shared;
using System.Net.Http.Json;

namespace Compass.Wasm.Client.Services;

public class BaseService<T> : IBaseService<T> where T : class
{
    private readonly HttpClient _http;
    private readonly string _serviceName;
    public BaseService(HttpClient http, string serviceName)
    {
        _http = http;
        _serviceName = serviceName;
    }
    public Task<HttpResponseMessage> AddAsync(T entity)
    {
        return _http.PostAsJsonAsync($"api/{_serviceName}/Add", entity);
    }

    public  Task<HttpResponseMessage> UpdateAsync(Guid id, T entity)
    {
        return _http.PutAsJsonAsync($"api/{_serviceName}/{id}", entity);
    }
    public Task<HttpResponseMessage> DeleteAsync(Guid id)
    {
        return _http.DeleteAsync($"api/{_serviceName}/{id}");
    }

    public Task<ApiResponse<T>> GetFirstOrDefault(Guid id)
    {
        return _http.GetFromJsonAsync<ApiResponse<T>>($"api/{_serviceName}/{id}")!;
    }

    public  Task<ApiResponse<List<T>>> GetAllAsync()
    {
        return _http.GetFromJsonAsync<ApiResponse<List<T>>>($"api/{_serviceName}/All")!;
    }
}