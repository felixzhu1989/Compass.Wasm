using Compass.Wasm.Shared.ProjectService;

namespace Compass.Wasm.Client.CategoryService;

public class AddProblemTypeRequest
{
    public string Name { get; set; }
    public Stakeholder Stakeholder { get; set; }
}