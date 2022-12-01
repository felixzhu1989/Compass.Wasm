namespace Compass.Wasm.Shared.ProjectService;

public class AssignDrawingsToUserRequest
{
    public IEnumerable<Guid> DrawingIds { get; set; }
    public Guid UserId  { get; set; }
}