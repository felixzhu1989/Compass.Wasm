namespace Compass.Wasm.Server.ProjectService.ModuleEvent;
public record ModuleCreatedEvent(Guid Id, string ModelName,Guid ModelTypeId, double Length, double Width, double Height);