namespace Compass.Wasm.Server.ProjectService.ModuleEvent;

public record ModuleUpdatedEvent(Guid Id, string ModelName, Guid ModelTypeId,Guid OldModelTypeId,double Length, double Width, double Height);