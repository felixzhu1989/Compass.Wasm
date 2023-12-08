namespace Compass.Wasm.Shared.Categories;

public class AccCutListDto : BaseDto
{
    private AccType_e accType;
    public AccType_e AccType
    {
        get => accType;
        set { accType = value; OnPropertyChanged(); }
    }

    private string? partDescription;
    public string? PartDescription
    {
        get => partDescription;
        set { partDescription = value; OnPropertyChanged(); }
    }

    private double length;
    public double Length
    {
        get => length;
        set { length = value; OnPropertyChanged(); }
    }
    private double width;
    public double Width
    {
        get => width;
        set { width = value; OnPropertyChanged(); }
    }
    private double thickness;
    public double Thickness
    {
        get => thickness;
        set { thickness = value; OnPropertyChanged(); }
    }
    private int quantity = 1;
    public int Quantity
    {
        get => quantity;
        set { quantity = value; OnPropertyChanged(); }
    }
    private string? material;
    public string? Material
    {
        get => material;
        set { material = value; OnPropertyChanged(); }
    }
    private string? partNo;
    public string? PartNo
    {
        get => partNo;
        set { partNo = value; OnPropertyChanged(); }
    }
    //从文件中读取的折弯属性
    private string? bendingMark;
    public string? BendingMark
    {
        get => bendingMark;
        set { bendingMark = value; OnPropertyChanged(); }
    }
}