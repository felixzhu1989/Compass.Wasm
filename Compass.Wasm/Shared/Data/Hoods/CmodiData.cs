namespace Compass.Wasm.Shared.Data.Hoods;

public class CmodiData : KwiData
{

    public override bool Accept(string model)
    {
        return model.ToLower().Equals("cmodi");
    }
}