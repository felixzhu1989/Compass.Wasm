namespace Compass.Wasm.Shared.Data.UL;

public class KveWwData : ModuleData
{
    public override bool Accept(string model)
    {
        return model.ToLower().Equals("kve-ww");
    }
}