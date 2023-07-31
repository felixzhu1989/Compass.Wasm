namespace Compass.Wasm.Shared.Data.UL;

public class KvcData : ModuleData
{
    public override bool Accept(string model)
    {
        return model.ToLower().Equals("kvc");
    }
}