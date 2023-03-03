namespace Compass.Wasm.Shared.DataService;

public class ModuleData : BaseDataEntity
{

    //Id直接使用ModuleId
    //产品基本属性有长宽高，注意和以前的作图程序不同，这里是总长，
    private double length;
    public double Length
    {
        get => length; set { length = value; OnPropertyChanged(); }
    }

    private double width;
    public double Width
    {
        get => width;
        set { width = value; OnPropertyChanged(); }
    }
    private double height;
    public double Height
    {
        get => height;
        set { height = value; OnPropertyChanged(); }
    }

    public virtual bool Accept(string model)
    {
        return false;
    }
    public ModuleData ChangeId(Guid id)
    {
        Id = id;
        return this;
    }
}