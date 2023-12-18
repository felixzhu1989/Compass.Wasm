using Compass.Wasm.Shared;
namespace Compass.Wasm.Client.Services;
public interface IBaseService<T> where T:class
{
    Task<HttpResponseMessage> AddAsync(T entity);
    Task<HttpResponseMessage> UpdateAsync(Guid id, T entity);
    Task<HttpResponseMessage> DeleteAsync(Guid id);
    Task<ApiResponse<T>> GetFirstOrDefault(Guid id);
    Task<ApiResponse<List<T>>> GetAllAsync();
}