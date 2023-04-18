using MediatR;

namespace Compass.Wasm.Shared.Projects.Notifications;

public record ProjectDeletedNotification(Guid Id):INotification;