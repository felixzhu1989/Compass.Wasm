namespace Compass.Wasm.Shared.Data.Ceilings;

public class UcwData:KcwData
{
    #region UV灯参数
    private UvLightType_e uvLightType;
    public UvLightType_e UvLightType
    {
        get => uvLightType;
        set { uvLightType = value; OnPropertyChanged(); }
    }
    //水洗挡板感应器数量
    private int baffleSensorNumber;
    public int BaffleSensorNumber
    {
        get => baffleSensorNumber;
        set { baffleSensorNumber = value; OnPropertyChanged(); }
    }
    private double baffleSensorDis1;
    public double BaffleSensorDis1
    {
        get => baffleSensorDis1;
        set { baffleSensorDis1 = value; OnPropertyChanged(); }
    }
    private double baffleSensorDis2;
    public double BaffleSensorDis2
    {
        get => baffleSensorDis2;
        set { baffleSensorDis2 = value; OnPropertyChanged(); }
    }
    #endregion

    public override bool Accept(string model)
    {
        return model.ToLower().Equals("ucw");
    }
}