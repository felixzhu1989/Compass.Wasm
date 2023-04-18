using MediatR;

namespace Compass.Wasm.Shared.Projects.Notifications;

public record ProjectCreatedNotification(Guid Id, string Name) : INotification;