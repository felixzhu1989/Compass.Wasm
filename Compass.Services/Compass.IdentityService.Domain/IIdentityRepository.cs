using Compass.IdentityService.Domain.Entities;
using Compass.Wasm.Shared.Identities;
using Microsoft.AspNetCore.Identity;

namespace Compass.IdentityService.Domain;

public interface IIdentityRepository
{
    #region Login需要的方法
    Task<User> GetUserByIdAsync(Guid id);
    #endregion

    #region Service需要的方法
    /// <summary>
    /// 添加用户
    /// </summary>
    /// <returns>返回值(元组)第三个是生成的密码</returns>
    public Task<(IdentityResult, User?, string? password)> AddUserAsync(UserDto dto);
    /// <summary>
    /// 更新用户
    /// </summary>
    public Task UpdateUserAsync(User model,UserDto dto);
    /// <summary>
    /// 删除用户
    /// </summary>
    public Task<IdentityResult> RemoveUserAsync(Guid id);
    /// <summary>
    /// 根据角色获取用户
    /// </summary>
    Task<IEnumerable<User>> GetUsersInRolesAsync(string roleNames);//获取多个角色的人员，角色以,分隔
    /// <summary>
    /// 重置密码。
    /// </summary>
    /// <returns>返回值(元组)第三个是生成的密码</returns>
    public Task<(IdentityResult, User?, string? password)> ResetPasswordAsync(Guid id);
    /// <summary>
    /// 修改密码
    /// </summary>
    Task<IdentityResult> ChangePasswordAsync(Guid userId, string password);
    #endregion

    #region Domain需要的方法
    /// <summary>
    /// 获取用户角色
    /// </summary>
    Task<IList<string>> GetRolesAsync(User user);
    Task<User> FindByNameAsync(string userName);//根据用户名获取用户
    Task<User?> FindByPhoneNumberAsync(string phoneNum);//根据手机号获取用户
    /// <summary>
    /// 为了登录而检查用户名、密码是否正确
    /// </summary>
    public Task<SignInResult> CheckForSignInAsync(User user, string password, bool lockoutOnFailure);

    #endregion
}