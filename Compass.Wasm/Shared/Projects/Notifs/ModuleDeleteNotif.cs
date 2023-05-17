using MediatR;

namespace Compass.Wasm.Shared.Projects.Notifs;

public record ModuleDeleteNotif(Guid Id) : INotification;
