using System.Runtime.InteropServices;
using Compass.Wasm.Shared;

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

    /// <summary>
    /// Allocates a new console for current process.
    /// </summary>
    [DllImport("kernel32.dll")]
    public static extern Boolean AllocConsole();

    /// <summary>
    /// Frees the console.
    /// </summary>
    [DllImport("kernel32.dll")]
    public static extern Boolean FreeConsole();
}
