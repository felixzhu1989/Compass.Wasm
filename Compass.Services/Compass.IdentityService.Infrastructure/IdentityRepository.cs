using Compass.IdentityService.Domain;
using Compass.IdentityService.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text;
using Compass.Wasm.Shared.Identities;

namespace Compass.IdentityService.Infrastructure;

public class IdentityRepository:IIdentityRepository
{
    #region ctor
    private readonly IdentityUserManager _userManager;
    private readonly RoleManager<Role> _roleManager;
    private readonly ILogger<IdentityRepository> _logger;
    public IdentityRepository(IdentityUserManager userManager, RoleManager<Role> roleManager, ILogger<IdentityRepository> logger)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _logger = logger;
    } 
    #endregion

    #region Login需要的方法
    public Task<User> GetUserByIdAsync(Guid id)
    {
       return _userManager.FindByIdAsync(id.ToString());
    }
    #endregion

    #region Service需要的方法

    #region 基本增删改
    /// <summary>
    /// 创建普通用户，并指定角色
    /// </summary>
    public async Task<(IdentityResult, User?, string? password)> AddUserAsync(UserDto dto)
    {
        if (await _userManager.FindByNameAsync(dto.UserName) != null)
            return (ErrorResult($"用户名{dto.UserName}已经存在"), null, null);

        var user = new User(dto.UserName)
        {
            Email = dto.Email,
            EmailConfirmed = true,
            PhoneNumber=dto.PhoneNumber,
            PhoneNumberConfirmed = true
        };
        //var password = GeneratePassword();//随机生成一个密码
        var password = "123";//不随机了，初始密码给123就好了
        var result = await _userManager.CreateAsync(user, password); ;
        if (!result.Succeeded) return (result, null, null);
        result = await AddToRoleAsync(user, dto.Role);
        return !result.Succeeded ? (result, null, null) : (IdentityResult.Success, user, password);
    }

    public async Task UpdateUserAsync(User model, UserDto dto)
    {
        model.UserName = dto.UserName;
        model.Email = dto.Email;
        model.PhoneNumber = dto.PhoneNumber;
        model.PhoneNumberConfirmed = true;
        await _userManager.UpdateAsync(model);
        await AddToRoleAsync(model, dto.Role);
    }

    /// <summary>
    /// 软删除
    /// </summary>
    public async Task<IdentityResult> RemoveUserAsync(Guid id)
    {
        var user = await _userManager.FindByIdAsync(id.ToString());
        var userLoginStore = _userManager.UserLoginStore;
        var noneCT = default(CancellationToken);
        //一定要删除aspnetuserlogins表中的数据，否则再次用这个外部登录登录的话
        //就会报错：The instance of entity type 'IdentityUserLogin<Guid>' cannot be tracked because another instance with the same key value for {'LoginProvider', 'ProviderKey'} is already being tracked.
        //而且要先删除aspnetuserlogins数据，再软删除User
        var logins = await userLoginStore.GetLoginsAsync(user, noneCT);
        foreach (var login in logins)
        {
            await userLoginStore.RemoveLoginAsync(user, login.LoginProvider, login.ProviderKey, noneCT);
        }
        //移除角色
        var roles = await _userManager.GetRolesAsync(user);
        await _userManager.RemoveFromRolesAsync(user, roles);
        user.SoftDelete();
        var result = await _userManager.UpdateAsync(user);
        return result;
    }
    #endregion

    #region Service扩展
    //根据角色返回用户
    public async Task<IEnumerable<User>> GetUsersInRolesAsync(string roleNames)
    {
        IEnumerable<User> users = new List<User>();
        var roles = roleNames.Split(',');
        foreach (var role in roles)
        {
            users = users.Concat(await _userManager.GetUsersInRoleAsync(role));
        }
        return users.OrderBy(x=>x.UserName);
    }
    
    //重置密码
    public async Task<(IdentityResult, User?, string? password)> ResetPasswordAsync(Guid id)
    {
        var user = await _userManager.FindByIdAsync(id.ToString());
        if (user == null) return (ErrorResult("用户没找到"), null, null);
        //string password = GeneratePassword();
        var password = "123";
        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var result = await _userManager.ResetPasswordAsync(user, token, password);
        return !result.Succeeded ? (result, null, null) : (IdentityResult.Success, user, password);
    }

    //修改密码
    public async Task<IdentityResult> ChangePasswordAsync(Guid userId, string password)
    {
        if (password.Length < 2)
        {
            IdentityError err = new IdentityError
            {
                Code = "Password Invalid",
                Description = "密码长度不能小于2"
            };
            return IdentityResult.Failed(err);
        }
        var user = await _userManager.FindByIdAsync(userId.ToString());
        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var resetPwdResult = await _userManager.ResetPasswordAsync(user, token, password);
        return resetPwdResult;
    } 
    #endregion

    #region 私有方法
    //把用户user加入角色role
    private async Task<IdentityResult> AddToRoleAsync(User user, string roleName)
    {
        var roles = await _userManager.GetRolesAsync(user);
        //如果原先角色不存在，那么添加到角色
        if (roles.Count == 0)
        {
            return await UserToRole();
        }
        //如果原先角色存在
        //判断两个角色是否相等，相等则不改变
        if (roles.Contains(roleName)) return IdentityResult.Success;
        //不相等则先移除角色再添加 
        await _userManager.RemoveFromRolesAsync(user, roles);
        return await UserToRole();

        //内嵌方法
        async Task<IdentityResult> UserToRole()
        {
            if (await _roleManager.RoleExistsAsync(roleName)) return await _userManager.AddToRoleAsync(user, roleName);
            //如果不存在就创建该角色
            var role = new Role { Name = roleName };
            var result = await _roleManager.CreateAsync(role);
            if (!result.Succeeded) return result;
            return await _userManager.AddToRoleAsync(user, roleName);
        }
    }
    private static IdentityResult ErrorResult(string msg)
    {
        IdentityError idError = new IdentityError { Description = msg };
        return IdentityResult.Failed(idError);
    }

    private string GeneratePassword()
    {
        var options = _userManager.Options.Password;
        int length = options.RequiredLength;
        bool nonAlphanumeric = options.RequireNonAlphanumeric;
        bool digit = options.RequireDigit;
        bool lowercase = options.RequireLowercase;
        bool uppercase = options.RequireUppercase;
        StringBuilder password = new StringBuilder();
        Random random = new Random();
        while (password.Length<length)
        {
            char c = (char)random.Next(32, 126);
            password.Append(c);
            if (char.IsDigit(c)) digit = false;
            else if (char.IsLower(c)) lowercase = false;
            else if (char.IsUpper(c)) uppercase = false;
            nonAlphanumeric = false;
        }
        if (nonAlphanumeric)
            password.Append((char)random.Next(33, 48));
        if (digit)
            password.Append((char)random.Next(48, 58));
        if (lowercase)
            password.Append((char)random.Next(97, 123));
        if (uppercase)
            password.Append((char)random.Next(65, 91));
        return password.ToString();
    } 
    #endregion
    #endregion

    #region Domain需要的方法
    public Task<IList<string>> GetRolesAsync(User user)
    {
        return _userManager.GetRolesAsync(user);
    }
    public Task<User> FindByNameAsync(string userName)
    {
        return _userManager.FindByNameAsync(userName);
    }
    public Task<User?> FindByPhoneNumberAsync(string phoneNum)
    {
        return _userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == phoneNum);
    }
    /// <summary>
    /// 尝试登录，如果lockoutOnFailure为true，则登录失败还会自动进行lockout计数
    /// </summary>
    public async Task<SignInResult> CheckForSignInAsync(User user, string password, bool lockoutOnFailure)
    {
        if (await _userManager.IsLockedOutAsync(user))
        {
            return SignInResult.LockedOut;
        }
        var success = await _userManager.CheckPasswordAsync(user, password);
        if (success) return SignInResult.Success;
        if (lockoutOnFailure)
        {
            var result = await _userManager.AccessFailedAsync(user);
            if (!result.Succeeded) throw new ApplicationException("AccessFailedAsync return failed");
        }
        return SignInResult.Failed;
    }
    #endregion

    #region 暂时无用的方法
    /// <summary>
    /// 生成重置密码的令牌
    /// </summary>
    private Task<string> GenerateChangePhoneNumberTokenAsync(User user, string phoneNumber)
    {
        return _userManager.GenerateChangePhoneNumberTokenAsync(user, phoneNumber);
    }
    /// <summary>
    /// 检查VCode，然后设置用户手机号为phoneNum
    /// </summary>
    private async Task<SignInResult> ChangePhoneNumberAsync(Guid userId, string phoneNumber, string token)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user==null) throw new ArgumentNullException($"{nameof(user)},{userId}用户不存在");
        var changeResult = await _userManager.ChangePhoneNumberAsync(user, phoneNumber, token);
        if (!changeResult.Succeeded)
        {
            await _userManager.AccessFailedAsync(user);
            string errMsg = changeResult.Errors.SumErrors();
            _logger.LogWarning($"{phoneNumber}，ChangePhoneNumberAsync失败，错误信息：{errMsg}");
            return SignInResult.Failed;
        }
        await ConfirmPhoneNumberAsync(user.Id);//确认手机号
        return SignInResult.Success;
    }
    /// <summary>
    /// 确认手机号
    /// </summary>
    public async Task ConfirmPhoneNumberAsync(Guid id)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id);
        if (user == null) throw new ArgumentException($"找不到用户，id={id}", nameof(id));
        user.PhoneNumberConfirmed = true;
        await _userManager.UpdateAsync(user);
    }
    /// <summary>
    /// 修改手机号
    /// </summary>
    public async Task UpdatePhoneNumberAsync(Guid id, string phoneNumber)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id);
        if (user == null) throw new ArgumentException($"找不到用户，id={id}", nameof(id));
        user.PhoneNumber=phoneNumber;
        await _userManager.UpdateAsync(user);
    }


    #endregion




   

   

    

    

    

    

    
    
    
}