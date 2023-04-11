namespace Compass.Wasm.Shared.DataService.Hoods;

public class UvfHwData : UvfData
{
    #region 灯具距离前端距离
    private double lightToFront;
    public double LightToFront
    {
        get => lightToFront;
        set { lightToFront = value; OnPropertyChanged(); }
    }
    #endregion

    public override bool Accept(string model)
    {
        return model.ToLower().Equals("uvfhw");
    }
}