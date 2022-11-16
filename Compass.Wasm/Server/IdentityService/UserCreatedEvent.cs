namespace Compass.Wasm.Server.IdentityService;
public record UserCreatedEvent(Guid Id, string UserName, string Password, string Email);