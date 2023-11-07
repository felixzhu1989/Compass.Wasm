namespace Compass.Wasm.Shared.Data.Ceilings;

public class CjData : ModuleData
{
    #region CJ脖颈参数
    private CjSpigotDirection_e cjSpigotDirection;
    public CjSpigotDirection_e CjSpigotDirection
    {
        get => cjSpigotDirection;
        set { cjSpigotDirection = value; OnPropertyChanged(); }
    }
    private double cjSpigotToRight;
    public double CjSpigotToRight
    {
        get => cjSpigotToRight;
        set { cjSpigotToRight = value; OnPropertyChanged(); }
    }
    #endregion

    #region 连接排风、BCJ、LKS、Gutter位置参数
    private BeamType_e leftBeamType;
    public BeamType_e LeftBeamType
    {
        get => leftBeamType;
        set { leftBeamType = value; OnPropertyChanged(); }
    }
    private double leftDbToRight;
    public double LeftDbToRight
    {
        get => leftDbToRight;
        set { leftDbToRight = value; OnPropertyChanged(); }
    }

    private BeamType_e rightBeamType;
    public BeamType_e RightBeamType
    {
        get => rightBeamType;
        set { rightBeamType = value; OnPropertyChanged(); }
    }
    private double rightDbToLeft;
    public double RightDbToLeft
    {
        get => rightDbToLeft;
        set { rightDbToLeft = value; OnPropertyChanged(); }
    }

    private double leftEndDis;
    public double LeftEndDis
    {
        get => leftEndDis;
        set { leftEndDis = value; OnPropertyChanged(); }
    }
    private double rightEndDis;
    public double RightEndDis
    {
        get => rightEndDis;
        set { rightEndDis = value; OnPropertyChanged(); }
    }
    private BcjSide_e bcjSide;
    public BcjSide_e BcjSide
    {
        get => bcjSide;
        set { bcjSide = value; OnPropertyChanged(); }
    }
    private LksSide_e lksSide;
    public LksSide_e LksSide
    {
        get => lksSide;
        set { lksSide = value; OnPropertyChanged(); }
    }

    private GutterSide_e gutterSide;
    public GutterSide_e GutterSide
    {
        get => gutterSide;
        set { gutterSide = value; OnPropertyChanged(); }
    }
    private double leftGutterWidth;
    public double LeftGutterWidth
    {
        get => leftGutterWidth;
        set { leftGutterWidth = value; OnPropertyChanged(); }
    }
    private double rightGutterWidth;
    public double RightGutterWidth
    {
        get => rightGutterWidth;
        set { rightGutterWidth = value; OnPropertyChanged(); }
    }
    #endregion

    #region 连接NOCJ时的参数
    private NocjSide_e nocjSide;
    public NocjSide_e NocjSide
    {
        get => nocjSide;
        set { nocjSide = value; OnPropertyChanged(); }
    }
    private NocjBackSide_e nocjBackSide;
    public NocjBackSide_e NocjBackSide
    {
        get => nocjBackSide;
        set { nocjBackSide = value; OnPropertyChanged(); }
    }
    //日本NOCJ340连接DP340的情况,(或NOCJ330?)
    private DpSide_e dpSide;
    public DpSide_e DpSide
    {
        get => dpSide;
        set { dpSide = value; OnPropertyChanged(); }
    }
    #endregion


    public override bool Accept(string model)
    {
        return model.ToLower().Equals("cj");
    }
}