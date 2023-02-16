using System.Security.Principal;

namespace Compass.Wpf.Identity;

public class CustomPrincipal : IPrincipal
{

    public bool IsInRole(string role)
    {
        return _identity.Roles.Contains(role);
    }
    IIdentity IPrincipal.Identity => this.Identity;
    //提供了自己的 Identity 属性，以便能够将主体的标识设置为我们的 CustomIdentity 类的实例。
    private CustomIdentity _identity;
    public CustomIdentity Identity
    {
        //在设置属性之前，即只要私有成员变量_identity为 NULL，它就会返回匿名（未经身份验证）标识。
        get => _identity ?? new AnonymousIdentity();
        set => _identity = value;
    }
}