namespace Compass.Wasm.Shared.Data.Hoods;

public class UvimData : ModuleData
{
    public override bool Accept(string model)
    {
        return model.ToLower().Equals("uvim");
    }
}