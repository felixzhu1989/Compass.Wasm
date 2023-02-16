using System.Security.Principal;

namespace Compass.Wpf.Identity;
/* IIdentity 接口封装了用户的标识
 * 自定义实现公开了三个属性（名称、电子邮件和角色），这些属性将在创建实例时传递给构造函数。
 *
 */
public class CustomIdentity : IIdentity
{
    public string AuthenticationType => "Custom authentication";
    //IIdentity.IsAuthenticated 属性的实现意味着一旦设置了 token 属性，用户就被视为已通过身份验证。
    public bool IsAuthenticated => !string.IsNullOrEmpty(Token);
    public string? Name { get; }
    public string Email { get;}
    public string? Roles { get; }
    public string? Token { get; set; }//由API返回token

    public CustomIdentity(string name, string email, string roles,string token)
    {
        Name = name;
        Email = email;
        Roles = roles;
        Token = token;
    }
}