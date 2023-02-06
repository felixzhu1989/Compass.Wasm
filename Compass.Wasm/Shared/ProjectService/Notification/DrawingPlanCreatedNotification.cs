using MediatR;
namespace Compass.Wasm.Shared.ProjectService.Notification;

public record DrawingPlanCreatedNotification(Guid ProjectId):INotification;
