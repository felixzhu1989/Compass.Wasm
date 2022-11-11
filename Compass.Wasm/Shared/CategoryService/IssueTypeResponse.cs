using Compass.Wasm.Shared.ProjectService;

namespace Compass.Wasm.Shared.CategoryService;

public class IssueTypeResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public Stakeholder Stakeholder { get; set; }
}