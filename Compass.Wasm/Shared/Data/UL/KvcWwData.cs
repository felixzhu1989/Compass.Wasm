namespace Compass.Wasm.Shared.Data.UL;

public class KvcWwData : ModuleData
{
    public override bool Accept(string model)
    {
        return model.ToLower().Equals("kvc-ww");
    }
}