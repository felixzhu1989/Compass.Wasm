using AutoMapper;
using Compass.Wasm.Server.Services.Identities;
using Compass.Wasm.Shared.Identities;

namespace Compass.Wasm.Server.Controllers.Others;

[Route("api/[controller]")]
[ApiController]
//[Authorize(Roles = "admin")]
public class UserAdminController : ControllerBase
{
    private readonly IdentityUserManager _userManager;
    private readonly IEventBus _eventBus;
    private readonly IIdentityRepository _repository;
    private readonly IMapper _mapper;

    public UserAdminController(IdentityUserManager userManager, IIdentityRepository repository,IMapper mapper, IEventBus eventBus)
    {
        _userManager = userManager;
        _eventBus = eventBus;
        _repository = repository;
        _mapper = mapper;
    }

    [HttpGet("AllUsers")]
    public async Task<List<UserDto>> FindAllUsers()
    {
        List<UserDto> dtos = new List<UserDto>();
        var models =_userManager.Users;
        foreach (var model in models)
        {
            var roles = await _repository.GetRolesAsync(model);
            dtos.Add(new UserDto { Id = model.Id,UserName = model.UserName,Email= model.Email,Roles = string.Join(',', roles),CreationTime = model.CreationTime });
        }
        return dtos.ToList();
    }

    //获取所有的设计师的请求"api/UserAdmin/UsersInRoles?roleNames=designer"
    [HttpGet("UsersInRoles")]
    public async Task<List<UserDto>> FindUsersByRoles(string roleNames)
    {
        List<UserDto> dtos = new List<UserDto>();
        var models =await _repository.FindUsersByRoles(roleNames);
        foreach (var model in models)
        {
            var roles = await _repository.GetRolesAsync(model);
            dtos.Add(new UserDto{Id=model.Id,UserName = model.UserName,Email= model.Email,Roles= string.Join(',', roles),CreationTime = model.CreationTime });

        }
        return dtos.ToList();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<UserDto>> FindById(Guid id)
    {
        var user = await _userManager.FindByIdAsync(id.ToString());
        if (user == null) return NotFound();
        var roles = await _repository.GetRolesAsync(user);
        return new UserDto{Id=user.Id,UserName = user.UserName,Email= user.Email,Roles=string.Join(',',roles),CreationTime = user.CreationTime};
        
    }

    [HttpGet("ByName/{userName}")]
    public async Task<ActionResult<UserDto>> FindByUserName(string userName)
    {
        var user = await _userManager.FindByNameAsync(userName);
        if (user == null) return NotFound();
        var roles = await _repository.GetRolesAsync(user);
        return new UserDto { Id=user.Id, UserName = user.UserName, Email= user.Email, Roles=string.Join(',', roles), CreationTime = user.CreationTime };
    }
    [HttpGet("IdByName/{userName}")]
    public async Task<ActionResult<Guid>> GetIdByName(string userName)
    {
        var user = await _userManager.FindByNameAsync(userName);
        if (user == null) return NotFound();
        return user.Id;
    }
    [HttpGet("NameById/{id}")]
    public async Task<ActionResult<string>> GetIdByName(Guid id)
    {
        var user = await _userManager.FindByIdAsync(id.ToString());
        if (user == null) return NotFound();
        return user.UserName;
    }
    [HttpPost("AddAdmin")]
    //[AllowAnonymous]//创建第一个管理员时使用
    public async Task<ActionResult> AddAdminUser(UserDto dto)
    {
        var (results, user, password) =
            await _repository.AddAdminUserAsync(dto.UserName, dto.Email);
        if (!results.Succeeded) return BadRequest(results.Errors.SumErrors());
        //生成的密码短信发给对方
        //可以同时或者选择性的把新增用户的密码短信/邮件/打印给用户(EventHandler中指定操作)
        //体现了领域事件对于代码“高内聚、低耦合”的追求
        var userCreatedEvent = new UserCreatedEvent(user!.Id, dto.UserName, password!, dto.Email);
        //发布集成事件
        _eventBus.Publish("IdentityService.User.Created", userCreatedEvent);
        return Ok();
    }

    [HttpPost("AddUser")]
    public async Task<ActionResult> AddUser(UserDto dto)
    {
        var (results, user, password) =
            await _repository.AddUserAsync(dto.UserName, dto.Email, dto.Roles);
        if (!results.Succeeded) return BadRequest(results.Errors.SumErrors());
        var eventData = new UserCreatedEvent(user!.Id, dto.UserName, password!, dto.Email);
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
    public async Task<ActionResult> UpdateUser(Guid id, UserDto dto)
    {
        var model = await _repository.FindByIdAsync(id);
        if (model == null) return NotFound("用户没找到");
        model.UserName = dto.UserName;
        model.Email = dto.Email;
        await _userManager.UpdateAsync(model);
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
    public async Task<ActionResult> ChangeUserPassword(Guid id, UserDto dto)
    {
        var model = await _repository.FindByIdAsync(id);
        if (model == null) return NotFound("用户没找到");
        await _repository.ChangePasswordAsync(id, dto.Password);
        //生成密码邮件发给对方
        var eventData = new ResetPasswordEvent(model.Id, model.UserName, dto.Password, model.Email);
        _eventBus.Publish("IdentityService.User.PasswordReset", eventData);
        return Ok();
    }

    [HttpPut("AddToRole/{id}")]
    public async Task<ActionResult> UpdateUserRole(Guid id, UserDto dto)
    {
        var model = await _repository.FindByIdAsync(id);
        if (model == null) return NotFound("用户没找到");
        await _repository.AddToRoleAsync(model, dto.Roles);
        return Ok();
    }
}