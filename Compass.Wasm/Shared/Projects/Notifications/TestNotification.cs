using MediatR;

namespace Compass.Wasm.Shared.Projects.Notifications;

public record TestNotification(string Name) : INotification;