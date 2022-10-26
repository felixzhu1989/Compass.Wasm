using Compass.IdentityService.Domain;
using Compass.Wasm.Server.IdentityService;
using Compass.Wasm.Shared.IdentityService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;
using Zack.EventBus;

namespace Compass.Wasm.Server.Controllers;

//eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjRmMjcwZjcxLWEzNWYtNGU4Yi1hNDAwLTIzNjMwNTNmNDhiZCIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWUiOiJhZG1pbiIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6ImFkbWluIiwiZXhwIjoxNjY3MzA0NjUyLCJpc3MiOiJteUlzc3VlciIsImF1ZCI6Im15QXVkaWVuY2UifQ.TKBKEQyrhOQyK8SmgrpculOedqfeAMHzkXDB4qqQTrs

[Route("api/[controller]")]
[ApiController]
public class LoginController : ControllerBase
{
    private readonly IdDomainService _idService;
    private readonly IIdRepository _repository;
    private readonly IEventBus _eventBus;

    public LoginController(IdDomainService idService, IIdRepository repository, IEventBus eventBus)
    {
        _idService = idService;
        _repository = repository;
        _eventBus = eventBus;
    }

    [HttpPost("Name")]
    public async Task<ActionResult<string?>> LoginByUserName(LoginByNameRequest request)
    {
        var (checkResult, token) = await _idService.LoginByUserNameAndPwdAsync(request.UserName, request.Password);
        if (checkResult.Succeeded) return token;
        else if (checkResult.IsLockedOut)
            return StatusCode((int)HttpStatusCode.Locked, "账号已锁定，请稍后再试");
        else return StatusCode((int)HttpStatusCode.BadRequest, "登录失败");

    }

    [HttpPost("Phone")]
    public async Task<ActionResult<string?>> LoginByPhone(LoginByPhoneRequest request)
    {
        //todo：要通过行为验证码、图形验证码等形式来防止暴力破解
        var (checkResult, token) = await _idService.LoginByPhoneAndPwdAsync(request.PhoneNumber, request.Password);
        if (checkResult.Succeeded) return token;
        else if (checkResult.IsLockedOut)
            return StatusCode((int)HttpStatusCode.Locked, "账号已锁定，请稍后再试");
        else return StatusCode((int)HttpStatusCode.BadRequest, "登录失败");
    }


    [HttpGet("Info")]
    //[Authorize]
    public async Task<ActionResult<UserResponse>> GetUserInfo()
    {
        //返回我的信息（当前登录用户）
        Guid userId = Guid.Parse(this.User.FindFirstValue(ClaimTypes.NameIdentifier));
        var user = await _repository.FindByIdAsync(userId);
        if (user == null) return NotFound();//可能用户注销了
        //出于安全考虑，不要机密信息传递到客户端
        //除非确认没问题，否则尽量不要直接把实体类对象返回给前端
        return new UserResponse(user.Id, user.UserName, user.Email, user.CreationTime);
    }
    [HttpPost("ChangePwd")]
    //[Authorize]
    public async Task<ActionResult> ChangePassword(ChangePwdRequest request)
    {
        Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var resetPwdResult = await _repository.ChangePasswordAsync(userId, request.Password);
        if (!resetPwdResult.Succeeded) return BadRequest(resetPwdResult.Errors.SumErrors());

        var user = await _repository.FindByIdAsync(userId);
        //发布集成事件
        var eventData = new ChangePasswordEvent(userId, user.UserName, request.Password, user.Email);
        _eventBus.Publish("IdentityService.User.PasswordChange", eventData);
        return Ok();
    }
}