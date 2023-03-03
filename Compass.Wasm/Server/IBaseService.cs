using Compass.Wasm.Shared;

namespace Compass.Wasm.Server;

public interface IBaseService<T> where T : class
{

    Task<ApiResponse<List<T>>> GetAllAsync();
    Task<ApiResponse<T>> GetSingleAsync(Guid id);
    Task<ApiResponse<T>> AddAsync(T dto);
    Task<ApiResponse<T>> UpdateAsync(Guid id, T dto);
    Task<ApiResponse<T>> DeleteAsync(Guid id);
}