namespace Compass.Wasm.Shared.Data.Ceilings;

public class DxfData : ModuleData
{
    
    private string? accNumber;//accessories Number(FCCOMBI-5,UCWUVRACK4L-5)
    public string? AccNumber
    {
        get => accNumber;
        set { accNumber = value; OnPropertyChanged(); }
    }

    public override bool Accept(string model)
    {
        return model.ToLower().Equals("dxf");
    }
}