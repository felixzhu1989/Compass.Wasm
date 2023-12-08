﻿namespace Compass.Wasm.Shared.Data.Ceilings;

public class KcjData : ModuleData
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

    #region 过滤器参数
    //油网类型,FC,KSA
    private FilterType_e filterType;
    public FilterType_e FilterType
    {
        get => filterType;
        set { filterType = value; OnPropertyChanged(); }
    }
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

    #region 灯具，判断KCJ265和290没有的参数
    private CeilingLightType_e ceilingLightType;
    public CeilingLightType_e CeilingLightType
    {
        get => ceilingLightType;
        set { ceilingLightType = value;OnPropertyChanged(); }
    }
    private LightCable_e lightCable;
    public LightCable_e LightCable
    {
        get => lightCable;
        set { lightCable = value; OnPropertyChanged(); }
    }
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

    #region 灯腔侧板参数
    //一整条LK的总长
    private double totalLength;
    public double TotalLength
    {
        get => totalLength;
        set { totalLength = value; OnPropertyChanged(); }
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

    #region 其他选项
    private bool domeSsp;
    public bool DomeSsp
    {
        get => domeSsp;
        set { domeSsp = value;OnPropertyChanged(); }
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

    private AnsulDetector_e ansulDetector;
    public AnsulDetector_e AnsulDetector
    {
        get => ansulDetector;
        set { ansulDetector = value; OnPropertyChanged(); }
    }
    #endregion

    #region Ansul探测器，判断双排风时需要探测器参数
    private AnsulDetectorEnd_e ansulDetectorEnd;
    public AnsulDetectorEnd_e AnsulDetectorEnd
    {
        get => ansulDetectorEnd;
        set { ansulDetectorEnd = value; OnPropertyChanged(); }
    }//末端探测器位置
    private int ansulDetectorNumber;
    public int AnsulDetectorNumber
    {
        get => ansulDetectorNumber;
        set { ansulDetectorNumber = value; OnPropertyChanged(); }
    }//探测器数量
    private double ansulDetectorDis1;
    public double AnsulDetectorDis1
    {
        get => ansulDetectorDis1;
        set { ansulDetectorDis1 = value; OnPropertyChanged(); }
    }
    private double ansulDetectorDis2;
    public double AnsulDetectorDis2
    {
        get => ansulDetectorDis2;
        set { ansulDetectorDis2 = value; OnPropertyChanged(); }
    }
    private double ansulDetectorDis3;
    public double AnsulDetectorDis3
    {
        get => ansulDetectorDis3;
        set { ansulDetectorDis3 = value; OnPropertyChanged(); }
    }
    private double ansulDetectorDis4;
    public double AnsulDetectorDis4
    {
        get => ansulDetectorDis4;
        set { ansulDetectorDis4 = value; OnPropertyChanged(); }
    }
    private double ansulDetectorDis5;
    public double AnsulDetectorDis5
    {
        get => ansulDetectorDis5;
        set { ansulDetectorDis5 = value; OnPropertyChanged(); }
    }
    #endregion

    public override bool Accept(string model)
    {
        return model.ToLower().Equals("kcj");
    }
}