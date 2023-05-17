using Compass.IdentityService.Domain.Entities;
using Compass.Wasm.Shared;
using Compass.Wasm.Shared.Identities;

namespace Compass.Wasm.Server.Services.Identities;

public interface IUserService : IBaseService<UserDto>
{
    Task<ApiResponse<List<UserDto>>> GetUsersInRolesAsync(string roles);
    Task<ApiResponse<UserDto>> ResetPwdAsync(Guid id, UserDto dto);
    Task<ApiResponse<UserDto>> ChangePwdAsync(Guid id,UserDto dto);
}
public class UserService : IUserService
{
    #region ctor
    private readonly IdentityUserManager _userManager;
    private readonly IEventBus _eventBus;
    private readonly IIdentityRepository _repository;
    public UserService(IdentityUserManager userManager, IIdentityRepository repository, IEventBus eventBus)
    {
        _userManager = userManager;
        _eventBus = eventBus;
        _repository = repository;
    }
    #endregion

    #region 基本增删改查
    public async Task<ApiResponse<List<UserDto>>> GetAllAsync()
    {
        try
        {
            var dtos = new List<UserDto>();
            var models = _userManager.Users.OrderBy(x => x.UserName);
            foreach (var model in models)
            {
                var roleNames = await _userManager.GetRolesAsync(model);
                dtos.Add(new UserDto { Id = model.Id, UserName = model.UserName, Email= model.Email,PhoneNumber = model.PhoneNumber, Role = string.Join(',', roleNames), CreationTime = model.CreationTime });
            }
            return new ApiResponse<List<UserDto>> { Status = true, Result = dtos };
        }
        catch (Exception e)
        {
            return new ApiResponse<List<UserDto>> { Status = false, Message = e.Message };
        }
    }
    public async Task<ApiResponse<UserDto>> GetSingleAsync(Guid id)
    {
        try
        {
            var model = await _userManager.FindByIdAsync(id.ToString());
            if (model == null) return new ApiResponse<UserDto> { Status = false, Message = "查询数据失败" };
            var roles = await _userManager.GetRolesAsync(model);
            var dto = new UserDto { Id=model.Id, UserName = model.UserName, Email= model.Email,PhoneNumber =model.PhoneNumber, Role=string.Join(',', roles), CreationTime = model.CreationTime };

            return new ApiResponse<UserDto> { Status = true, Result = dto };
        }
        catch (Exception e)
        {
            return new ApiResponse<UserDto> { Status = false, Message = e.Message };
        }
    }
    public async Task<ApiResponse<UserDto>> AddAsync(UserDto dto)
    {
        try
        {
            var (results, user, password) =
                await _repository.AddUserAsync(dto);
            if (!results.Succeeded) return new ApiResponse<UserDto> { Status = false, Message = results.Errors.SumErrors() };
            dto.Id = user.Id;

            //发布集成事件
            //生成的密码短信发给对方
            //可以同时或者选择性的把新增用户的密码短信/邮件/打印给用户(EventHandler中指定操作)
            //体现了领域事件对于代码“高内聚、低耦合”的追求
            var eventData = new UserCreatedEvent(user.Id, dto.UserName, password!, dto.Email);
            _eventBus.Publish("IdentityService.User.Created", eventData);

            return new ApiResponse<UserDto> { Status = true, Result = dto };
        }
        catch (Exception e)
        {
            return new ApiResponse<UserDto> { Status = false, Message = e.Message };
        }
    }
    public async Task<ApiResponse<UserDto>> UpdateAsync(Guid id, UserDto dto)
    {
        try
        {
            var model = await _userManager.FindByIdAsync(id.ToString());
            if (model == null) return new ApiResponse<UserDto> { Status = false, Message = "更新数据失败" };
            await  _repository.UpdateUserAsync(model, dto);
            return new ApiResponse<UserDto> { Status = true, Result = dto };
        }
        catch (Exception e)
        {
            return new ApiResponse<UserDto> { Status = false, Message = e.Message };
        }
    }
    public async Task<ApiResponse<UserDto>> DeleteAsync(Guid id)
    {
        try
        {
            await _repository.RemoveUserAsync(id);
            return new ApiResponse<UserDto> { Status = true };
        }
        catch (Exception e)
        {
            return new ApiResponse<UserDto> { Status = false, Message = e.Message };
        }
    }
    #endregion

    #region Blazor扩展
    public async Task<ApiResponse<List<UserDto>>> GetUsersInRolesAsync(string roles)
    {
        try
        {
            var dtos = new List<UserDto>();
            var models = await _repository.GetUsersInRolesAsync(roles);
            foreach (var model in models)
            {
                var roleNames = await _userManager.GetRolesAsync(model);
                dtos.Add(new UserDto { Id = model.Id, UserName = model.UserName, Email= model.Email, PhoneNumber = model.PhoneNumber, Role = string.Join(',', roleNames), CreationTime = model.CreationTime });
            }
            return new ApiResponse<List<UserDto>> { Status = true, Result = dtos };
        }
        catch (Exception e)
        {
            return new ApiResponse<List<UserDto>> { Status = false, Message = e.Message };
        }
    }

    public async Task<ApiResponse<UserDto>> ResetPwdAsync(Guid id, UserDto dto)
    {
        try
        {
           var (result, user, password) = await _repository.ResetPasswordAsync(id);

           if (!result.Succeeded) return new ApiResponse<UserDto> { Status = false, Message = result.Errors.SumErrors() };
           //生成密码邮件发给对方
           var eventData = new ResetPasswordEvent(user!.Id, user.UserName, password!, user.Email);
           _eventBus.Publish("IdentityService.User.PasswordReset", eventData);
           return new ApiResponse<UserDto> { Status = true,Result = dto};
        }
        catch (Exception e)
        {
            return new ApiResponse<UserDto> { Status = false, Message = e.Message };
        }
    }

    public async Task<ApiResponse<UserDto>> ChangePwdAsync(Guid id, UserDto dto)
    {
        try
        {
            var result= await _repository.ChangePasswordAsync(id,dto.Password);

            if (!result.Succeeded) return new ApiResponse<UserDto> { Status = false, Message = result.Errors.SumErrors() };
            //生成密码邮件发给对方
            var eventData = new ResetPasswordEvent(id, dto.UserName, dto.Password, dto.Email);
            _eventBus.Publish("IdentityService.User.PasswordReset", eventData);
            return new ApiResponse<UserDto> { Status = true, Result = dto };
        }
        catch (Exception e)
        {
            return new ApiResponse<UserDto> { Status = false, Message = e.Message };
        }
    }
    #endregion

}