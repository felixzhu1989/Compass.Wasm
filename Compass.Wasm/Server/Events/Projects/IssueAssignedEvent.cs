namespace Compass.Wasm.Server.Events.Projects;

public record IssueAssignedEvent(string Responder, string Email, string Number, string Name, string Content, DateTime Deadline, string Url);
