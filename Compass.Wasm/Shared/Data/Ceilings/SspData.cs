namespace Compass.Wasm.Shared.Data.Ceilings;

public class SspData:ModuleData
{

    private PanelType_e leftType;
    public PanelType_e LeftType
    {
        get => leftType;
        set { leftType = value; OnPropertyChanged(); }
    }
    private PanelType_e rightType;
    public PanelType_e RightType
    {
        get => rightType;
        set { rightType = value; OnPropertyChanged(); }
    }
    private double leftWidth;
    public double LeftWidth
    {
        get => leftWidth;
        set { leftWidth = value; OnPropertyChanged(); }
    }
    private double rightWidth;
    public double RightWidth
    {
        get => rightWidth;
        set { rightWidth = value; OnPropertyChanged(); }
    }
    private int mPanelNumber;
    public int MPanelNumber
    {
        get => mPanelNumber;
        set { mPanelNumber = value; OnPropertyChanged(); }
    }
    private bool ledLight;
    public bool LedLight
    {
        get => ledLight;
        set { ledLight = value; OnPropertyChanged(); }
    }
    

    public override bool Accept(string model)
    {
        return model.ToLower().Equals("ssp");
    }
}