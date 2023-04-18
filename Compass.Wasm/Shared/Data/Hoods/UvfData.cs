namespace Compass.Wasm.Shared.Data.Hoods;

public class UvfData : UviData
{

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

    public override bool Accept(string model)
    {
        return model.ToLower().Equals("uvf");
    }
}