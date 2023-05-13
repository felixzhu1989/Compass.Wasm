using Compass.Wasm.Shared.Identities;

namespace Compass.Wasm.Server.Events.Projects;

public record IssueCreatedEvent(List<EmailAddress> Emails, Guid MainPlanId, string Number, string Name, string Reporter, string Content, string Url);
