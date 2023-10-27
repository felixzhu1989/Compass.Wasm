namespace Compass.Wasm.Shared.Data.Hoods;

public class KviData : ModuleData
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
    }//长灯, 短灯, 筒灯60, 筒灯140

    private int spotLightNumber;
    public int SpotLightNumber
    {
        get => spotLightNumber;
        set { spotLightNumber = value; OnPropertyChanged(); }
    }//前端界面input设置为number，min=1
    private double spotLightDistance;
    public double SpotLightDistance
    {
        get => spotLightDistance;
        set { spotLightDistance = value; OnPropertyChanged(); }
    } //默认为400 
    private double lightToFront;
    public double LightToFront
    {
        get => lightToFront;
        set { lightToFront = value; OnPropertyChanged(); }
    }//灯具距离前端距离,默认0为居中
    #endregion

    #region 其他配置
    private DrainType_e drainType;
    public DrainType_e DrainType
    {
        get => drainType;
        set { drainType = value; OnPropertyChanged(); }
    }
    private bool waterCollection;
    public bool WaterCollection
    {
        get => waterCollection;
        set { waterCollection = value; OnPropertyChanged(); }
    }

    private bool ledLogo;
    public bool LedLogo
    {
        get => ledLogo;
        set { ledLogo = value; OnPropertyChanged(); }
    }

    private bool backToBack;
    public bool BackToBack
    {
        get => backToBack;
        set { backToBack = value; OnPropertyChanged(); }
    }
    private bool backCj;
    public bool BackCj
    {
        get => backCj;
        set { backCj = value; OnPropertyChanged(); }
    }
    private double cjSpigotToRight;
    public double CjSpigotToRight
    {
        get => cjSpigotToRight;
        set { cjSpigotToRight = value; OnPropertyChanged(); }
    }

    private bool coverBoard;
    public bool CoverBoard
    {
        get => coverBoard;
        set { coverBoard = value; OnPropertyChanged(); }
    }

    private bool marvel;
    public bool Marvel
    {
        get => marvel;
        set { marvel = value; OnPropertyChanged(); }
    }
    #endregion

    #region Ansul基本参数
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

    #region Ansul下喷
    private int ansulDropNumber;
    public int AnsulDropNumber
    {
        get => ansulDropNumber;
        set { ansulDropNumber = value; OnPropertyChanged(); }
    }//界面input设置为number，min=0 

    //下喷距离前端距离
    private double ansulDropToFront;
    public double AnsulDropToFront
    {
        get => ansulDropToFront;
        set { ansulDropToFront = value; OnPropertyChanged(); }
    }

    private double ansulDropDis1;
    public double AnsulDropDis1
    {
        get => ansulDropDis1;
        set { ansulDropDis1 = value; OnPropertyChanged(); }
    }
    private double ansulDropDis2;
    public double AnsulDropDis2
    {
        get => ansulDropDis2;
        set { ansulDropDis2 = value; OnPropertyChanged(); }
    }
    private double ansulDropDis3;
    public double AnsulDropDis3
    {
        get => ansulDropDis3;
        set { ansulDropDis3 = value; OnPropertyChanged(); }
    }
    private double ansulDropDis4;
    public double AnsulDropDis4
    {
        get => ansulDropDis4;
        set { ansulDropDis4 = value; OnPropertyChanged(); }
    }
    private double ansulDropDis5;
    public double AnsulDropDis5
    {
        get => ansulDropDis5;
        set { ansulDropDis5 = value; OnPropertyChanged(); }
    }
    #endregion

    public override bool Accept(string model)
    {
        return model.ToLower().Equals("kvi");
    }
}