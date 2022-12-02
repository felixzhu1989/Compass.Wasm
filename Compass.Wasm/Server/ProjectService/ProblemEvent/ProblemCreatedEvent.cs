using Compass.Wasm.Shared.IdentityService;

namespace Compass.Wasm.Server.ProjectService.ProblemEvent;

public record ProblemCreatedEvent(List<EmailAddress> Emails,Guid ProjectId, string OdoNumber, string ProjectName,string Reporter, string ProblemDesc, string Url);
