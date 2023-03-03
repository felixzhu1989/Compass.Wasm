using Compass.Wasm.Shared;
using System.Threading.Tasks;
using System;

namespace Compass.Wpf.Service;

public interface IBaseDataService<TEntity> where TEntity : class
{
    Task<ApiResponse<TEntity>> UpdateAsync(Guid id, TEntity entity);
    Task<ApiResponse<TEntity>> GetFirstOrDefault(Guid id);
}

