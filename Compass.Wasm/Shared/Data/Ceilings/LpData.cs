namespace Compass.Wasm.Shared.Data.Ceilings;

public class LpData:ModuleData
{
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
    private int zPanelNumber;
    public int ZPanelNumber
    {
        get => zPanelNumber;
        set { zPanelNumber = value; OnPropertyChanged(); }
    }
    private bool ledLight;
    public bool LedLight
    {
        get => ledLight;
        set { ledLight = value; OnPropertyChanged(); }
    }
    public override bool Accept(string model)
    {
        return model.ToLower().Equals("lp");
    }
}