namespace Compass.Wasm.Server.Services.Identities;
public record ResetPasswordEvent(Guid Id, string UserName, string Password, string Email);