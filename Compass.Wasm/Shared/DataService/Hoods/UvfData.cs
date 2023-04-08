namespace Compass.Wasm.Shared.DataService.Hoods;

public class UvfData : ModuleData
{
    public SidePanel_e SidePanel { get; set; }//大侧板：左, 右, 中, 双
    //排风口参数
    public double MiddleToRight { get; private set; }//中心距离右端
    public double ExhaustSpigotLength { get; private set; }
    public double ExhaustSpigotWidth { get; private set; }
    public double ExhaustSpigotHeight { get; private set; }
    public int ExhaustSpigotNumber { get; private set; }//前端界面input设置为number，min=1,max=2
    private double exhaustSpigotDis;
    public double ExhaustSpigotDis
    {
        get => exhaustSpigotDis;
        set { exhaustSpigotDis = value; OnPropertyChanged(); }
    }
    #region 新风口参数
    private int supplySpigotNumber = 2;
    public int SupplySpigotNumber
    {
        get => supplySpigotNumber;
        set { supplySpigotNumber = value; OnPropertyChanged(); }
    }//前端界面input设置为number，min=1,max=2
    private double supplySpigotDis = 800d;
    public double SupplySpigotDis
    {
        get => supplySpigotDis;
        set { supplySpigotDis = value; OnPropertyChanged(); }
    }
    #endregion

    //灯具类型
    public LightType_e LightType { get; private set; }//长灯, 短灯, 筒灯60, 筒灯140
    public int SpotLightNumber { get; private set; }//前端界面input设置为number，min=1
    public double SpotLightDistance { get; private set; } = 400d;//默认为400
    //UV灯类型
    public UvLightType_e UvLightType { get; private set; }
    public bool BlueTooth { get; private set; }
    //其他配置
    public bool LedLogo { get; private set; }
    public DrainType_e DrainType { get; private set; }
    public bool WaterCollection { get; private set; }
    public bool BackToBack { get; private set; }
    public bool BackCj { get; private set; }
    public bool CoverBoard { get; private set; }

    //Ansul
    public bool Ansul { get; private set; }
    public AnsulSide_e AnsulSide { get; private set; }
    public AnsulDetector_e AnsulDetector { get; private set; }
    private int ansulDropNumber;
    public int AnsulDropNumber
    {
        get => ansulDropNumber;
        set { ansulDropNumber = value; OnPropertyChanged(); }
    }//界面input设置为number，min=0 
    //Marvel
    public bool Marvel { get; private set; }


    public override bool Accept(string model)
    {
        return model.ToLower().Equals("uvf");
    }
}