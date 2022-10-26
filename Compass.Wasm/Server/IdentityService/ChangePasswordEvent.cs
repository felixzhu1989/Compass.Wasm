namespace Compass.Wasm.Server.IdentityService;
public record ChangePasswordEvent(Guid Id, string UserName, string Password, string Email);