using MediatR;

namespace Compass.Wasm.Shared.ProjectService.Notification;

public record ProjectCreatedNotification(Guid Id, string Name) : INotification;