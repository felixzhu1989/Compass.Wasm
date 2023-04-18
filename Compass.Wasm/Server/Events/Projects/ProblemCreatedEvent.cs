using Compass.Wasm.Shared.Identities;

namespace Compass.Wasm.Server.Events.Projects;

public record ProblemCreatedEvent(List<EmailAddress> Emails, Guid ProjectId, string OdoNumber, string ProjectName, string Reporter, string ProblemDesc, string Url);
