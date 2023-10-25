namespace Compass.Wasm.Shared.Projects;

public class CutListDto : BaseDto
{
	private Guid moduleId;
    public Guid ModuleId
	{
		get => moduleId;
        set { moduleId = value;OnPropertyChanged(); }
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

    #region 额外属性
    private int index;
    public int Index
    {
        get => index;
        set { index = value; OnPropertyChanged(); }
    }
    //文件地址
    private string filePath;
    public string FilePath
    {
        get => filePath;
        set { filePath = value; OnPropertyChanged(); }
    }
    #endregion
}