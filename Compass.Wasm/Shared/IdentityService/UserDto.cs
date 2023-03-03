namespace Compass.Wasm.Shared.IdentityService;

public class UserDto : BaseDto
{
    private string userName;
    public string? UserName { get => userName; set { userName = value; OnPropertyChanged(); } }
    private string password;
    public string? Password { get => password; set { password = value; OnPropertyChanged(); } }
    private string password2;
    public string? Password2 { get => password2; set { password2 = value; OnPropertyChanged(); } }
    public string? Email { get; set; }
    public string? Roles { get; set; }
    public string? PhoneNumber { get; set; }
}