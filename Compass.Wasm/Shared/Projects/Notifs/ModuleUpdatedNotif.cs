using Compass.Wasm.Shared.Data;
using MediatR;

namespace Compass.Wasm.Shared.Projects.Notifs;

public record ModuleUpdatedNotif(Guid Id, string ModelName, Guid ModelTypeId,double Length, double Width, double Height,SidePanel_e SidePanel,bool Marvel):INotification;