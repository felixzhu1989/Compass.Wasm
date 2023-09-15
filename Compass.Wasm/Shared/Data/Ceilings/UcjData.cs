namespace Compass.Wasm.Shared.Data.Ceilings;

public class UcjData:KcjData
{
    #region UV灯类型
    private UvLightType_e uvLightType;
    public UvLightType_e UvLightType
    {
        get => uvLightType;
        set { uvLightType = value; OnPropertyChanged(); }
    }
    #endregion

    public override bool Accept(string model)
    {
        return model.ToLower().Equals("ucj");
    }
}