using System.Collections.Generic;
using System.Threading.Tasks;
using Compass.Wasm.Shared;

namespace Compass.Wpf.ApiService;

public interface IBaseService<TEntity> where TEntity : class
{
    Task<ApiResponse<TEntity>> AddAsync(TEntity entity);
    Task<ApiResponse<TEntity>> UpdateAsync(Guid id, TEntity entity);
    Task<ApiResponse<TEntity>> DeleteAsync(Guid id);
    Task<ApiResponse<TEntity>> GetFirstOrDefault(Guid id);
    Task<ApiResponse<List<TEntity>>> GetAllAsync();
}