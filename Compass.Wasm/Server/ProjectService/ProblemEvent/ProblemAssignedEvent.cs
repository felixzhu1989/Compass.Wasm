namespace Compass.Wasm.Server.ProjectService.ProblemEvent;

public record ProblemAssignedEvent(string UserName, string Email,string OdoNumber,string ProjectName,string ProblemDesc, DateTime Deadline, string Url);
