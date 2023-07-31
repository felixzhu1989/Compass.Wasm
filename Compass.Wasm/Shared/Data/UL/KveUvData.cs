namespace Compass.Wasm.Shared.Data.UL;

public class KveUvData : ModuleData
{
    public override bool Accept(string model)
    {
        return model.ToLower().Equals("kve-uv");
    }
}