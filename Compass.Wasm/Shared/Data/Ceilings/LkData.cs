namespace Compass.Wasm.Shared.Data.Ceilings;

public class LkData:ModuleData
{

    //与水洗烟罩相连（Tab口）
    private bool waterWash;
    public bool WaterWash
    {
        get => waterWash;
        set { waterWash = value; OnPropertyChanged(); }
    }

    //一整条LK的总长
    private double totalLength;
    public double TotalLength
    {
        get => totalLength;
        set { totalLength = value; OnPropertyChanged(); }
    }

    //灯具类型
    private CeilingLightType_e ceilingLightType;
    public CeilingLightType_e CeilingLightType
    {
        get => ceilingLightType;
        set { ceilingLightType = value; OnPropertyChanged(); }
    }

    //玻璃灯板
    private int longGlassNumber;
    public int LongGlassNumber
    {
        get => longGlassNumber;
        set { longGlassNumber = value; OnPropertyChanged(); }
    }
    private int shortGlassNumber;
    public int ShortGlassNumber
    {
        get => shortGlassNumber;
        set { shortGlassNumber = value; OnPropertyChanged(); }
    }
    
    
    #region 日本项目
    private bool japan;
    public bool Japan
    {
        get => japan;
        set { japan = value; OnPropertyChanged(); }
    }
    private double leftLength;
    public double LeftLength
    {
        get => leftLength;
        set { leftLength = value; OnPropertyChanged(); }
    }
    private double rightLength;
    public double RightLength
    {
        get => rightLength;
        set { rightLength = value; OnPropertyChanged(); }
    }
    private double middleLength;
    public double MiddleLength
    {
        get => middleLength;
        set { middleLength = value; OnPropertyChanged(); }
    } 
    #endregion


    public override bool Accept(string model)
    {
        return model.ToLower().Equals("lk");
    }
}