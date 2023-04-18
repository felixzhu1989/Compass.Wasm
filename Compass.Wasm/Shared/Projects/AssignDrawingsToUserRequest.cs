namespace Compass.Wasm.Shared.Projects;

public class AssignDrawingsToUserRequest
{
    public IEnumerable<Guid> DrawingIds { get; set; }
    public Guid UserId  { get; set; }
}