using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Compass.Dtos;
using Compass.TodoService.Infrastructure;
using Compass.Wasm.Server.Services.Todos;
using Compass.Wasm.Shared;
using Compass.Wasm.Shared.Params;
using Compass.Wasm.Shared.Todos;
using Microsoft.AspNetCore.Authorization;

namespace Compass.Wasm.Server.Controllers.Todos;

[Route("api/[controller]")]
[ApiController]
[UnitOfWork(typeof(TodoDbContext))]
public class MemoController : ControllerBase
{
    private readonly IMemoService _service;
    public MemoController(IMemoService service)
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
        catch (Exception)
        {
            return Guid.Empty;
        }
    }
    #region 标准增删改查
    [HttpGet("All")]
    public async Task<ApiResponse<List<MemoDto>>> GetAll() => await _service.GetAllAsync();

    [HttpGet("{id}")]
    public async Task<ApiResponse<MemoDto>> GetSingle([RequiredGuid] Guid id) => await _service.GetSingleAsync(id);

    [HttpPost("Add")]
    public async Task<ApiResponse<MemoDto>> Add(MemoDto dto) => await _service.AddAsync(dto);

    [HttpPut("{id}")]
    public async Task<ApiResponse<MemoDto>> Update([RequiredGuid] Guid id, MemoDto dto) => await _service.UpdateAsync(id, dto);

    [HttpDelete("{id}")]
    public async Task<ApiResponse<MemoDto>> Delete([RequiredGuid] Guid id) => await _service.DeleteAsync(id);
    #endregion

    #region 增加了特定用户的基本增查(删改不需要)
    [Authorize]
    [HttpGet("User/All")]
    public async Task<ApiResponse<List<MemoDto>>> GetUserAll() => await _service.GetUserAllAsync(GetUserId());

    [Authorize]
    [HttpPost("User/Add")]
    public async Task<ApiResponse<MemoDto>> UserAdd(MemoDto dto) => await _service.UserAddAsync(dto, GetUserId());

    #endregion

    /// <summary>
    /// 根据查询条件筛选结果
    /// </summary>
    [Authorize]
    [HttpGet("Filter")]
    public async Task<ApiResponse<List<MemoDto>>> GetAllFilter(QueryParam parameter) => await _service.GetAllFilterAsync(parameter, GetUserId());

}