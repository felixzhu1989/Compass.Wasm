namespace Compass.Wasm.Shared.DataService.Hoods;

public class UviData : KviData
{
    #region UV灯类型
    private UvLightType_e uvLightType;
    public UvLightType_e UvLightType
    {
        get => uvLightType;
        set { uvLightType = value; OnPropertyChanged(); }
    }
    private bool bluetooth;
    public bool Bluetooth
    {
        get => bluetooth;
        set { bluetooth = value; OnPropertyChanged(); }
    }
    #endregion

    public override bool Accept(string model)
    {
        return model.ToLower().Equals("uvi");
    }
}