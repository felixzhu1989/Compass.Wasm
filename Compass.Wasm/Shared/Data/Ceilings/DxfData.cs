namespace Compass.Wasm.Shared.Data.Ceilings;

public class DxfData : ModuleData
{
    public string AssyPath { get; set; }

    public override bool Accept(string model)
    {
        return model.ToLower().Equals("dxf");
    }
}