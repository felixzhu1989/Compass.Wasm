using Compass.Wasm.Shared;
using Compass.Wasm.Shared.Parameter;

namespace Compass.Wasm.Server;

public interface IBaseService<T>
{

    Task<ApiResponse<List<T>>> GetAllAsync();
    Task<ApiResponse<T>> GetSingleAsync(Guid id);
    Task<ApiResponse<T>> AddAsync(T dto);
    Task<ApiResponse<T>> UpdateAsync(Guid id, T dto);
    Task<ApiResponse<T>> DeleteAsync(Guid id);
}