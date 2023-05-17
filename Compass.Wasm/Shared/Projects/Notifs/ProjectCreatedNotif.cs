using MediatR;

namespace Compass.Wasm.Shared.Projects.Notifs;

public record ProjectCreatedNotif(Guid Id, string Name) : INotification;