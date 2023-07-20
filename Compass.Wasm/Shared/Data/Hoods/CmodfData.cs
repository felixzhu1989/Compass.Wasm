namespace Compass.Wasm.Shared.Data.Hoods;

public class CmodfData:KwfData
{
    public override bool Accept(string model)
    {
        return model.ToLower().Equals("cmodf");
    }
}