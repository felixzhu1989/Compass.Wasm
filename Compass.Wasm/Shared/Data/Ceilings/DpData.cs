namespace Compass.Wasm.Shared.Data.Ceilings;

public class DpData:CjData
{
    #region 排水管位置
    private DpDrainType_e dpDrainType;
    public DpDrainType_e DpDrainType
    {
        get => dpDrainType;
        set { dpDrainType = value; OnPropertyChanged(); }
    }
    #endregion

    #region 日本DP340，背面连接DP的情况
    private DpBackSide_e dpBackSide;
    public DpBackSide_e DpBackSide
    {
        get => dpBackSide;
        set { dpBackSide = value; OnPropertyChanged(); }
    }
    #endregion


    public override bool Accept(string model)
    {
        return model.ToLower().Equals("dp");
    }
}