namespace Compass.Wasm.Shared.Data.Hoods;

public class CmodmData:ModuleData
{
    public override bool Accept(string model)
    {
        return model.ToLower().Equals("cmodm");
    }
}