namespace Compass.Wasm.Server.ProjectService.ProjectEvent;
public record ProjectCreatedEvent(Guid Id,string Name,DateTime SortDate);