﻿using Compass.Wasm.Shared;
using Compass.Wasm.Shared.Parameters;
using Compass.Wasm.Shared.Todos;

namespace Compass.Wasm.Server.Services.Todos;

public interface ITodoService : IBaseService<TodoDto>
{
    //扩展标准增删改查之外的查询功能
    Task<ApiResponse<List<TodoDto>>> GetUserAllAsync(Guid userId);
    Task<ApiResponse<TodoDto>> UserAddAsync(TodoDto dto, Guid userId);

    Task<ApiResponse<List<TodoDto>>> GetAllFilterAsync(TodoParameter parameter, Guid userId);
    Task<ApiResponse<TodoSummaryDto>> GetSummary(Guid userId);
}