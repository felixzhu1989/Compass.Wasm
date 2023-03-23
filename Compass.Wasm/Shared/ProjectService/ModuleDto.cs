namespace Compass.Wasm.Shared.ProjectService;

public class ModuleDto : BaseDto
{
    private Guid drawingId;
    public Guid DrawingId
    {
        get => drawingId;
        set
        {
            drawingId = value; OnPropertyChanged();
        }
    }

    private Guid? modelTypeId;
    public Guid? ModelTypeId
    {
        get => modelTypeId; set
        {
            modelTypeId = value; OnPropertyChanged();
        }
    }//标明该分段是属于什么子模型


    private string name;
    public string Name
    {
        get => name;
        set
        {
            name = value;
            OnPropertyChanged();
        }
    }

    private string modelName;
    public string ModelName
    {
        get => modelName;
        set
        {
            modelName = value;
            OnPropertyChanged();
        }
    }

    private string? specialNotes;
    public string? SpecialNotes
    {
        get => specialNotes;
        set
        {
            specialNotes = value;
            OnPropertyChanged();
        }
    }

    private bool isModuleDataOk;
    public bool IsModuleDataOk
    {
        get => isModuleDataOk;
        set { isModuleDataOk = value; OnPropertyChanged(); }
    }
    private bool isCutListOk;
    public bool IsCutListOk
    {
        get => isCutListOk;
        set { isCutListOk = value; OnPropertyChanged(); }
    }




    #region 额外属性


    #region 长宽高
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

    private double height;
    public double Height
    {
        get => height;
        set
        {
            height = value;
            OnPropertyChanged();
        }
    }
    #endregion



    private bool isSelected;
    public bool IsSelected
    {
        get => isSelected;
        set
        {
            isSelected = value;
            OnPropertyChanged();
        }
    }
    private string odpNumber;
    public string OdpNumber
    {
        get => odpNumber;
        set
        {
            odpNumber = value;
            OnPropertyChanged();
        }
    }
    private string projectName;
    public string ProjectName
    {
        get => projectName;
        set
        {
            projectName = value;
            OnPropertyChanged();
        }
    }

    private string itemNumber;
    public string ItemNumber
    {
        get => itemNumber;
        set
        {
            itemNumber = value;
            OnPropertyChanged();
        }
    }
    #endregion
}