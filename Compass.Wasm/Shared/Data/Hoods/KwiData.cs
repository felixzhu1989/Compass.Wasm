namespace Compass.Wasm.Shared.Data.Hoods;

public class KwiData : KviData
{
    #region 水洗管入口
    private WaterInlet_e waterInlet = WaterInlet_e.右入水;
    public WaterInlet_e WaterInlet
    {
        get => waterInlet;
        set { waterInlet = value; OnPropertyChanged(); }
    }
    #endregion

    #region Ansul探测器
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
        return model.ToLower().Equals("kwi");
    }
}