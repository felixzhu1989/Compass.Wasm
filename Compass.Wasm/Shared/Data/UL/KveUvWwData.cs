namespace Compass.Wasm.Shared.Data.UL;

public class KveUvWwData : ModuleData
{
    public override bool Accept(string model)
    {
        return model.ToLower().Equals("kve-uv-ww");
    }
}