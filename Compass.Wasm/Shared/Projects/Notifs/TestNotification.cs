using MediatR;

namespace Compass.Wasm.Shared.Projects.Notifs;

public record TestNotification(string Name) : INotification;