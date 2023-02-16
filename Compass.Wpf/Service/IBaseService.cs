using System;
using System.Collections.Generic;
using Compass.Wasm.Shared;
using RestSharp;
using System.Threading.Tasks;

namespace Compass.Wpf.Service;

public interface IBaseService<TEntity> where TEntity : class
{
    Task<ApiResponse<TEntity>> AddAsync(TEntity entity);
    Task<ApiResponse<TEntity>> UpdateAsync(Guid id, TEntity entity);
    Task<ApiResponse<TEntity>> DeleteAsync(Guid id);
    Task<ApiResponse<TEntity>> GetFirstOrDefault(Guid id);
    Task<ApiResponse<List<TEntity>>> GetAllAsync();
}