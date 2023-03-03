using Compass.Wasm.Shared;

namespace Compass.Wasm.Server;

public interface IBaseDataUpdateService<T> where T : class
{
    Task<ApiResponse<T>> UpdateAsync(Guid id, T dto);
}