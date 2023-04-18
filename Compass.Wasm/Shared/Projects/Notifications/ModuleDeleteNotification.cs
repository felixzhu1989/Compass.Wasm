using MediatR;

namespace Compass.Wasm.Shared.Projects.Notifications;

public record ModuleDeleteNotification(Guid Id) : INotification;
