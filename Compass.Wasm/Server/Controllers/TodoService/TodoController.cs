﻿using Compass.TodoService.Infrastructure;
using Compass.Wasm.Shared.TodoService;
using System.ComponentModel.DataAnnotations;
using Compass.Wasm.Server.TodoService;
using Compass.Wasm.Shared;
using Compass.Wasm.Shared.Parameter;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Irony.Parsing.Construction;

namespace Compass.Wasm.Server.Controllers.TodoService;

[Route("api/[controller]")]
[ApiController]
[UnitOfWork(typeof(TodoDbContext))]
public class TodoController : ControllerBase
{
    private readonly ITodoService _service;
    public TodoController(ITodoService service)
    {
        _service = service;
    }

    private Guid GetUserId()
    {
        try
        {
            //获取当前请求的用户的ID
            return Guid.Parse(Request.HttpContext.User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value);
        }
        catch (Exception e)
        {
            return Guid.Empty;
        }
    }

    #region 标准增删改查
    [HttpGet("All")]
    public async Task<ApiResponse<List<TodoDto>>> GetAll() => await _service.GetAllAsync();


    [HttpGet("{id}")]
    public async Task<ApiResponse<TodoDto>> GetSingle([RequiredGuid] Guid id) => await _service.GetSingleAsync(id);

    [HttpPost("Add")]
    public async Task<ApiResponse<TodoDto>> Add(TodoDto dto) => await _service.AddAsync(dto);

    [HttpPut("{id}")]
    public async Task<ApiResponse<TodoDto>> Update([RequiredGuid] Guid id, TodoDto dto) => await _service.UpdateAsync(id, dto);

    [HttpDelete("{id}")]
    public async Task<ApiResponse<TodoDto>> Delete([RequiredGuid] Guid id) => await _service.DeleteAsync(id);
    #endregion



    #region 增加了特定用户的基本增查(删改不需要)
    [Authorize]
    [HttpGet("User/All")]
    public async Task<ApiResponse<List<TodoDto>>> GetUserAll() => await _service.GetUserAllAsync(GetUserId());

    [Authorize]
    [HttpPost("User/Add")]
    public async Task<ApiResponse<TodoDto>> UserAdd(TodoDto dto) => await _service.UserAddAsync(dto, GetUserId());

    #endregion

    /// <summary>
    /// 根据查询条件筛选结果
    /// </summary>
    [Authorize]
    [HttpGet("Filter")]
    public async Task<ApiResponse<List<TodoDto>>> GetAllFilter(TodoParameter parameter) => await _service.GetAllFilterAsync(parameter, GetUserId());

    /// <summary>
    /// 查询汇总结果
    /// </summary>
    [Authorize]
    [HttpGet("Summary")]
    public async Task<ApiResponse<TodoSummaryDto>> GetUserSummary() => await _service.GetSummary(GetUserId());
}