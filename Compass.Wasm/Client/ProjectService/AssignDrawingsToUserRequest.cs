namespace Compass.Wasm.Client.ProjectService;

public class AssignDrawingsToUserRequest
{
    public IEnumerable<Guid> DrawingIds { get; set; }
    public Guid UserId  { get; set; }
}