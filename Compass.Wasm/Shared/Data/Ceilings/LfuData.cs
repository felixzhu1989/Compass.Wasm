namespace Compass.Wasm.Shared.Data.Ceilings;

public class LfuData:ModuleData 
{
    #region 新风口参数
    private int supplySpigotNumber = 2;
    public int SupplySpigotNumber
    {
        get => supplySpigotNumber;
        set { supplySpigotNumber = value; OnPropertyChanged(); }
    }
    private double supplySpigotDis;
    public double SupplySpigotDis
    {
        get => supplySpigotDis;
        set { supplySpigotDis = value; OnPropertyChanged(); }
    }
    private double supplySpigotDia;
    public double SupplySpigotDia
    {
        get => supplySpigotDia;
        set { supplySpigotDia = value; OnPropertyChanged(); }
    }
    #endregion

    //一整条LFU的总长
    private double totalLength;
    public double TotalLength
    {
        get => totalLength;
        set { totalLength = value; OnPropertyChanged(); }
    }

    private bool japan;
    public bool Japan
    {
        get => japan;
        set { japan = value; OnPropertyChanged(); }
    }

    public override bool Accept(string model)
    {
        return model.ToLower().Equals("lfu");
    }
}