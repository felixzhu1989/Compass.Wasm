namespace Compass.Wpf.Identity;

public class AnonymousIdentity : CustomIdentity
{
    //表示未经身份验证的用户，即具有空名称的用户。
    public AnonymousIdentity() : base(string.Empty, string.Empty, string.Empty, string.Empty)
    {
    }
}