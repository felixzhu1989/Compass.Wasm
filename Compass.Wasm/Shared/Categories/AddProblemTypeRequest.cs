using Compass.Wasm.Shared.Projects;

namespace Compass.Wasm.Shared.Categories;

public class AddProblemTypeRequest
{
    public string Name { get; set; }
    public Stakeholder_e Stakeholder { get; set; }
}