using MediatR;

namespace Compass.Wasm.Shared.Projects.Notifs;

public record ProjectDeletedNotif(Guid Id):INotification;