namespace Compass.Wasm.Shared.IdentityService;

public record UserResponse
{
    public Guid Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Roles { get; set; }
    public DateTime CreationTime { get; set; }
}