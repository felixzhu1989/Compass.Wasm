using AutoMapper;
using Compass.Wasm.Server.IdentityService;
using Compass.Wasm.Shared.IdentityService;

namespace Compass.Wasm.Server.Controllers.OtherService;

[Route("api/[controller]")]
[ApiController]
//[Authorize(Roles = "admin")]
public class UserAdminController : ControllerBase
{
    private readonly IdUserManager _userManager;
    private readonly IEventBus _eventBus;
    private readonly IIdRepository _repository;
    private readonly IMapper _mapper;

    public UserAdminController(IdUserManager userManager, IIdRepository repository,IMapper mapper, IEventBus eventBus)
    {
        _userManager = userManager;
        _eventBus = eventBus;
        _repository = repository;
        _mapper = mapper;
    }

    [HttpGet("AllUsers")]
    public async Task<UserResponse[]> FindAllUsers()
    {
        List<UserResponse> responses = new List<UserResponse>();
        var users =_userManager.Users;
        foreach (var user in users)
        {
            var roles = await _repository.GetRolesAsync(user);
            responses.Add(new UserResponse { Id = user.Id,UserName = user.UserName,Email= user.Email,Roles = string.Join(',', roles),CreationTime = user.CreationTime });
        }
        return responses.ToArray();
    }

    //获取所有的设计师的请求"api/UserAdmin/UsersInRoles?roleNames=designer"
    [HttpGet("UsersInRoles")]
    public async Task<UserResponse[]> FindUsersByRoles(string roleNames)
    {
        List<UserResponse> responses = new List<UserResponse>();
        var users =await _repository.FindUsersByRoles(roleNames);
        foreach (var user in users)
        {
            var roles = await _repository.GetRolesAsync(user);
            responses.Add(new UserResponse{Id=user.Id,UserName = user.UserName,Email= user.Email,Roles= string.Join(',', roles),CreationTime = user.CreationTime });

        }
        return responses.ToArray();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<UserResponse>> FindById(Guid id)
    {
        var user = await _userManager.FindByIdAsync(id.ToString());
        if (user == null) return NotFound();
        var roles = await _repository.GetRolesAsync(user);
        return new UserResponse{Id=user.Id,UserName = user.UserName,Email= user.Email,Roles=string.Join(',',roles),CreationTime = user.CreationTime};
        
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
        var userCreatedEvent = new UserCreatedEvent(user!.Id, request.UserName, password!, request.Email);
        //发布集成事件
        _eventBus.Publish("IdentityService.User.Created", userCreatedEvent);
        return Ok();
    }

    [HttpPost("AddUser")]
    public async Task<ActionResult> AddUser(AddUserRequest request)
    {
        var (results, user, password) =
            await _repository.AddUserAsync(request.UserName, request.Email, request.RoleName);
        if (!results.Succeeded) return BadRequest(results.Errors.SumErrors());
        var eventData = new UserCreatedEvent(user!.Id, request.UserName, password!, request.Email);
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
    public async Task<ActionResult> UpdateUser(Guid id, UpdateUserRequest request)
    {
        var user = await _repository.FindByIdAsync(id);
        if (user == null) return NotFound("用户没找到");
        user.UserName = request.UserName;
        user.Email = request.Email;
        await _userManager.UpdateAsync(user);
        return Ok();
    }

    [HttpPut("ResetPwd/{id}")]
    public async Task<ActionResult> ResetAdminUserPassword(Guid id)
    {
        var (result, user, password) = await _repository.ResetPasswordAsync(id);
        if (!result.Succeeded) return BadRequest(result.Errors.SumErrors());
        //生成密码邮件发给对方
        var eventData = new ResetPasswordEvent(user!.Id, user.UserName, password!, user.Email);
        _eventBus.Publish("IdentityService.User.PasswordReset", eventData);
        return Ok();
    }
    [HttpPut("ChangePwd/{id}")]
    public async Task<ActionResult> ChangeUserPassword(Guid id, string password)
    {
        var user = await _repository.FindByIdAsync(id);
        if (user == null) return NotFound("用户没找到");
        await _repository.ChangePasswordAsync(id, password);
        //生成密码邮件发给对方
        var eventData = new ResetPasswordEvent(user.Id, user.UserName, password, user.Email);
        _eventBus.Publish("IdentityService.User.PasswordReset", eventData);
        return Ok();
    }

    [HttpPut("AddToRole/{id}")]
    public async Task<ActionResult> UpdateUserRole(Guid id, string roleName)
    {
        var user = await _repository.FindByIdAsync(id);
        if (user == null) return NotFound("用户没找到");
        await _repository.AddToRoleAsync(user, roleName);
        return Ok();
    }
}