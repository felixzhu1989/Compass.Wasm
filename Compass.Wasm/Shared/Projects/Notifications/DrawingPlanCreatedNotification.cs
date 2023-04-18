using MediatR;

namespace Compass.Wasm.Shared.Projects.Notifications;

public record DrawingPlanCreatedNotification(Guid ProjectId):INotification;
