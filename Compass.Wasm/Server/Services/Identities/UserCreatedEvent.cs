namespace Compass.Wasm.Server.Services.Identities;
public record UserCreatedEvent(Guid Id, string UserName, string Password, string Email);