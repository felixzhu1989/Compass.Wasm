using Compass.Wasm.Shared.Data;
using MediatR;

namespace Compass.Wasm.Shared.Projects.Notifications;

public record ModuleCreatedNotification(Guid Id, string ModelName,Guid ModelTypeId, double Length, double Width, double Height,SidePanel_e SidePanel) : INotification;