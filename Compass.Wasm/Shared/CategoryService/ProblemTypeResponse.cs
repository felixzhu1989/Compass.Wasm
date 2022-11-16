using Compass.Wasm.Shared.ProjectService;

namespace Compass.Wasm.Shared.CategoryService;

public class ProblemTypeResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public Stakeholder Stakeholder { get; set; }
}