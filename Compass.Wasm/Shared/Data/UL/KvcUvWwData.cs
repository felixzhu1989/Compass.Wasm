﻿namespace Compass.Wasm.Shared.Data.UL;

public class KvcUvWwData : ModuleData
{
    public override bool Accept(string model)
    {
        return model.ToLower().Equals("kvc-uv-ww");
    }
}