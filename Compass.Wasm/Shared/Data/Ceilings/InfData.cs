namespace Compass.Wasm.Shared.Data.Ceilings;

public class InfData : ModuleData
{

    public override bool Accept(string model)
    {
        return model.ToLower().Equals("inf");
    }
}