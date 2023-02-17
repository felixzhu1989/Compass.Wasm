using System;

namespace Compass.Wpf.Common;
/// <summary>
/// 保存用户登录信息
/// </summary>
public class AppSession
{
    public static string? UserName { get; set; }
    public static string? Roles { get; set; }
    public static Guid? Id{ get; set; }
    public static string? Token { get; set; }
}
