using MediatR;

namespace Compass.Wasm.Shared.ProjectService.Notification;

public record TestNotification(string Name) : INotification;