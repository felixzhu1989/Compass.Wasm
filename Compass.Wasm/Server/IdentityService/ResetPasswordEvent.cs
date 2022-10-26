namespace Compass.Wasm.Server.IdentityService;
public record ResetPasswordEvent(Guid Id, string UserName, string Password, string Email);