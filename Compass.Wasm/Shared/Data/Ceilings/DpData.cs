namespace Compass.Wasm.Shared.Data.Ceilings;

public class DpData:ModuleData
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
}