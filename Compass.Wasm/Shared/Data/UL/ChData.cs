using Compass.Wasm.Shared.Data.Hoods;

namespace Compass.Wasm.Shared.Data.UL;

public class ChData:KvvData
{
    public override bool Accept(string model)
    {
        return model.ToLower().Equals("ch");
    }
}