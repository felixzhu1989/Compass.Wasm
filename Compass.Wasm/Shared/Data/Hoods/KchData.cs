namespace Compass.Wasm.Shared.Data.Hoods;

public class KchData:KvfData
{
    public override bool Accept(string model)
    {
        return model.ToLower().Equals("kch");
    }
}