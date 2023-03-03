using MediatR;

namespace Compass.Wasm.Shared.ProjectService.Notification;

public record ModuleDeleteNotification(Guid Id) : INotification;
