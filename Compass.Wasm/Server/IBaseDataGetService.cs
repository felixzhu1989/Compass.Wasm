using Compass.Wasm.Shared;

namespace Compass.Wasm.Server;

public interface IBaseDataGetService<T> where T : class
{
    Task<ApiResponse<T>> GetSingleAsync(Guid id);
}