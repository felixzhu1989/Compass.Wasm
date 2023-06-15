using Compass.Wasm.Server.Services.Todos;
using Compass.Wasm.Shared.Todos;

namespace Compass.Wasm.Server.Events;

public record ProjectCreatedEvent(string OdpNumber, string ProjectName, Guid? Designer);

[EventName("ProjectService.Project.Created")]
public class ProjectCreatedEventHandler : JsonIntegrationEventHandler<ProjectCreatedEvent>
{
    private readonly ITodoService _todoService;
    public ProjectCreatedEventHandler(ITodoService todoService)
    {
        _todoService = todoService;
    }

    public override async Task HandleJson(string eventName, ProjectCreatedEvent? eventData)
    {
        if(eventData.Designer==null || eventData.Designer==Guid.Empty) return;
        var dto = new TodoDto
        {
            Title = eventData.OdpNumber,
            Content = eventData.ProjectName,
            Status = 0,
            UserId = eventData.Designer.Value
        };
        await _todoService.UserAddAsync(dto,eventData.Designer.Value);
    }
}