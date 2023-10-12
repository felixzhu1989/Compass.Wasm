namespace Compass.Wasm.Shared.Data.Hoods;

public class KvvData : ModuleData
{
    #region 排风口参数
    //中心距离右端
    private double middleToRight;
    public double MiddleToRight
    {
        get => middleToRight;
        set { middleToRight = value; OnPropertyChanged(); }
    }
    private double exhaustSpigotLength;
    public double ExhaustSpigotLength
    {
        get => exhaustSpigotLength;
        set { exhaustSpigotLength = value; OnPropertyChanged(); }
    }

    private double exhaustSpigotWidth;
    public double ExhaustSpigotWidth
    {
        get => exhaustSpigotWidth;
        set { exhaustSpigotWidth = value; OnPropertyChanged(); }
    }
    private double exhaustSpigotHeight;
    public double ExhaustSpigotHeight
    {
        get => exhaustSpigotHeight;
        set { exhaustSpigotHeight = value; OnPropertyChanged(); }
    }

    private int exhaustSpigotNumber = 1;
    public int ExhaustSpigotNumber
    {
        get => exhaustSpigotNumber;
        set { exhaustSpigotNumber = value; OnPropertyChanged(); }
    } //前端界面input设置为number，min=1,max=2 

    private double exhaustSpigotDis;
    public double ExhaustSpigotDis
    {
        get => exhaustSpigotDis;
        set { exhaustSpigotDis = value; OnPropertyChanged(); }
    }
    #endregion

    #region 灯具类型
    private LightType_e lightType;
    public LightType_e LightType
    {
        get => lightType;
        set { lightType = value; OnPropertyChanged(); }
    }//长灯, 短灯 
    #endregion


    public override bool Accept(string model)
    {
        return model.ToLower().Equals("kvv");
    }
}