using MediatR;

namespace Compass.Wasm.Shared.ProjectService.Notification;

public record ModuleCreatedNotification(Guid Id, string ModelName,Guid ModelTypeId, double Length, double Width, double Height) : INotification;