﻿using System;
using System.Threading.Tasks;
using Compass.Wasm.Shared;

namespace Compass.Wpf.ApiService;

public interface IBaseDataService<TEntity> where TEntity : class
{
    Task<ApiResponse<TEntity>> UpdateAsync(Guid id, TEntity entity);
    Task<ApiResponse<TEntity>> GetFirstOrDefault(Guid id);
}
