namespace Compass.Wasm.Shared.Data.Ceilings;

public class AnData : ModuleData
{
    private bool ansul;
    public bool Ansul
    {
        get => ansul;
        set { ansul = value; OnPropertyChanged(); }
    }

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
        return model.ToLower().Equals("an");
    }
}