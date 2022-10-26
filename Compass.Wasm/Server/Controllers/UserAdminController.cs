using Compass.IdentityService.Domain;
using Compass.IdentityService.Infrastructure;
using Compass.Wasm.Server.IdentityService;
using Compass.Wasm.Shared.IdentityService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Zack.EventBus;

namespace Compass.Wasm.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
//[Authorize(Roles = "admin")]
public class UserAdminController : ControllerBase
{
    private readonly IdUserManager _userManager;
    private readonly IEventBus _eventBus;
    private readonly IIdRepository _repository;
    public UserAdminController(IdUserManager userManager, IEventBus eventBus, IIdRepository repository)
    {
        _userManager = userManager;
        _eventBus = eventBus;
        _repository = repository;
    }

    [HttpGet("AllUsers")]
    public Task<UserDto[]> FindAllUsers()
    {
       return _userManager.Users.Select(x => UserDto.Create(x)).ToArrayAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<UserDto>> FindById(Guid id)
    {
        var user = await _userManager.FindByIdAsync(id.ToString());
        if (user == null) return NotFound();
        return UserDto.Create(user);
    }

    [HttpPost("AddAdmin")]
    //[AllowAnonymous]//创建第一个管理员时使用
    public async Task<ActionResult> AddAdminUser(AddAdminRequest request)
    {
        var (results, user, password) =
            await _repository.AddAdminUserAsync(request.UserName, request.Email);
        if (!results.Succeeded) return BadRequest(results.Errors.SumErrors());
        //生成的密码短信发给对方
        //可以同时或者选择性的把新增用户的密码短信/邮件/打印给用户(EventHandler中指定操作)
        //体现了领域事件对于代码“高内聚、低耦合”的追求
        var UserCreatedEvent = new UserCreatedEvent(user.Id, request.UserName, password, request.Email);
        //发布集成事件
        _eventBus.Publish("IdentityService.User.Created", UserCreatedEvent);
        return Ok();
    }

    [HttpPost("AddUser")]
    public async Task<ActionResult> AddUser(AddUserRequest request)
    {
        var (results, user, password) =
            await _repository.AddUserAsync(request.UserName, request.Email, request.RoleName);
        if (!results.Succeeded) return BadRequest(results.Errors.SumErrors());
        var eventData = new UserCreatedEvent(user.Id, request.UserName, password, request.Email);
        //发布集成事件
        _eventBus.Publish("IdentityService.User.Created", eventData);
        return Ok();
    }

    [HttpDelete("Delete/{id}")]
    public async Task<ActionResult> DeleteAdminUser(Guid id)
    {
        await _repository.RemoveUserAsync(id);
        return Ok();
    }

    [HttpPut("Update/{id}")]
    public async Task<ActionResult> UpdateAdminUser(Guid id, UpdateAdminRequest request)
    {
        var user = await _repository.FindByIdAsync(id);
        if (user == null) return NotFound("用户没找到");
        user.UserName = request.UserName;
        user.Email=request.Email;
        await _userManager.UpdateAsync(user);
        return Ok();
    }

    [HttpPut("ResetPwd/{id}")]
    public async Task<ActionResult> ResetAdminUserPassword(Guid id)
    {
        var (result, user, password) = await _repository.ResetPasswordAsync(id);
        if (!result.Succeeded) return BadRequest(result.Errors.SumErrors());
        //生成密码邮件发给对方
        var eventData = new ResetPasswordEvent(user.Id, user.UserName, password, user.Email);
        _eventBus.Publish("IdentityService.User.PasswordReset", eventData);
        return Ok();
    }
}