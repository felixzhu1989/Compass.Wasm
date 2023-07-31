namespace Compass.Wasm.Shared.Data.UL;

public class KvrData : ModuleData
{
    public override bool Accept(string model)
    {
        return model.ToLower().Equals("kvr");
    }
}