namespace Compass.Wasm.Shared.Data.Ceilings;

public class KcwData:ModuleData
{
    //水洗排风腔需要SidePanel参数

    #region DP排水腔参数
    //油网位置
    private DpSide_e dpSide;
    public DpSide_e DpSide
    {
        get => dpSide;
        set { dpSide = value; OnPropertyChanged(); }
    }
    #endregion

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

    #region 过滤器参数
    //油网位置
    private FilterSide_e filterSide;
    public FilterSide_e FilterSide
    {
        get => filterSide;
        set { filterSide = value; OnPropertyChanged(); }
    }
    //左油网长度
    private double filterLeft;
    public double FilterLeft
    {
        get => filterLeft;
        set { filterLeft = value; OnPropertyChanged(); }
    }
    //右油网长度
    private double filterRight;
    public double FilterRight
    {
        get => filterRight;
        set { filterRight = value; OnPropertyChanged(); }
    }
    //油网盲板数量
    private int filterBlindNumber;
    public int FilterBlindNumber
    {
        get => filterBlindNumber;
        set { filterBlindNumber = value; OnPropertyChanged(); }
    }

    #endregion

    #region 灯具，判断KCW265没有的参数
    private CeilingLightType_e ceilingLightType;
    public CeilingLightType_e CeilingLightType
    {
        get => ceilingLightType;
        set { ceilingLightType = value; OnPropertyChanged(); }
    }
    //出线孔走DP腔
    //private LightCable_e lightCable;
    //public LightCable_e LightCable
    //{
    //    get => lightCable;
    //    set { lightCable = value; OnPropertyChanged(); }
    //}
    //HCL时，需要侧板
    private HclSide_e hclSide;
    public HclSide_e HclSide
    {
        get => hclSide;
        set { hclSide = value; OnPropertyChanged(); }
    }
    //左侧板长度
    private double hclLeft;
    public double HclLeft
    {
        get => hclLeft;
        set { hclLeft = value; OnPropertyChanged(); }
    }
    //右侧板长度
    private double hclRight;
    public double HclRight
    {
        get => hclRight;
        set { hclRight = value; OnPropertyChanged(); }
    }
    #endregion

    #region 水洗管入口,KCW265时才需要
    private CeilingWaterInlet_e ceilingWaterInlet;
    public CeilingWaterInlet_e CeilingWaterInlet
    {
        get => ceilingWaterInlet;
        set { ceilingWaterInlet = value; OnPropertyChanged(); }
    }
    #endregion

    #region 其他选项
    private bool domeSsp;
    public bool DomeSsp
    {
        get => domeSsp;
        set { domeSsp = value; OnPropertyChanged(); }
    }
    private bool gutter;
    public bool Gutter
    {
        get => gutter;
        set { gutter = value; OnPropertyChanged(); }
    }
    private double gutterWidth;
    public double GutterWidth
    {
        get => gutterWidth;
        set { gutterWidth = value; OnPropertyChanged(); }
    }
    private bool marvel;
    public bool Marvel
    {
        get => marvel;
        set { marvel = value; OnPropertyChanged(); }
    }

    private bool japan;
    public bool Japan
    {
        get => japan;
        set { japan = value; OnPropertyChanged(); }
    }

    #endregion

    #region Ansul
    private bool ansul;
    public bool Ansul
    {
        get => ansul;
        set { ansul = value; OnPropertyChanged(); }
    }

    private AnsulSide_e ansulSide;
    public AnsulSide_e AnsulSide
    {
        get => ansulSide;
        set { ansulSide = value; OnPropertyChanged(); }
    }
    #endregion

    public override bool Accept(string model)
    {
        return model.ToLower().Equals("kcw");
    }
}