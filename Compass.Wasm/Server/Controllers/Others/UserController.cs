using Compass.Wasm.Shared;
using Compass.Wasm.Shared.Identities;
using Compass.Wasm.Server.Services.Identities;
using Compass.Wasm.Shared.Projects;
using System.ComponentModel.DataAnnotations;
using Compass.Dtos;

namespace Compass.Wasm.Server.Controllers.Others;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _service;
    public UserController(IUserService service)
    {
        _service = service;
    }

    #region 基本增删改查
    [HttpGet("All")]
    public async Task<ApiResponse<List<UserDto>>> GetAll() => await _service.GetAllAsync();

    [HttpGet("{id}")]
    public async Task<ApiResponse<UserDto>> GetSingle([RequiredGuid] Guid id) => await _service.GetSingleAsync(id);

    [HttpPost("Add")]
    public async Task<ApiResponse<UserDto>> Add(UserDto dto) => await _service.AddAsync(dto);

    [HttpPut("{id}")]
    public async Task<ApiResponse<UserDto>> Update([RequiredGuid] Guid id, UserDto dto) => await _service.UpdateAsync(id, dto);

    [HttpDelete("{id}")]
    public async Task<ApiResponse<UserDto>> Delete([RequiredGuid] Guid id) => await _service.DeleteAsync(id);

    #endregion

    #region Blazor扩展
    [HttpGet("Roles/{roles}")]
    public async Task<ApiResponse<List<UserDto>>> GetUsersInRoles(string roles) => await _service.GetUsersInRolesAsync(roles);
    [HttpPut("ResetPwd/{id}")]
    public async Task<ApiResponse<UserDto>> ResetPwd([RequiredGuid] Guid id, UserDto dto) => await _service.ResetPwdAsync(id, dto);
    [HttpPut("ChangePwd/{id}")]
    public async Task<ApiResponse<UserDto>> ChangePwd([RequiredGuid] Guid id, UserDto dto) => await _service.ChangePwdAsync(id,dto);
    #endregion


}