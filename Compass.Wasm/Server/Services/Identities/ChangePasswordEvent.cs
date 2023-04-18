namespace Compass.Wasm.Server.Services.Identities;
public record ChangePasswordEvent(Guid Id, string UserName, string Password, string Email);