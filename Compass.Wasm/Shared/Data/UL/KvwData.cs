namespace Compass.Wasm.Shared.Data.UL;

public class KvwData : ModuleData
{
    public override bool Accept(string model)
    {
        return model.ToLower().Equals("kvw");
    }
}