using MediatR;

namespace Compass.Wasm.Shared.ProjectService.Notification;

public record ProjectDeletedNotification(Guid Id):INotification;