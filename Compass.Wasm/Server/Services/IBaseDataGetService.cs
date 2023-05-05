using Compass.Wasm.Shared;

namespace Compass.Wasm.Server.Services;

public interface IBaseDataGetService<T> where T : class
{
    Task<ApiResponse<T>> GetSingleAsync(Guid id);
}