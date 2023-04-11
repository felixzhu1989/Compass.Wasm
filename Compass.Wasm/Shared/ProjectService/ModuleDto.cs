using Compass.Wasm.Shared.DataService;

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
    private SidePanel_e sidePanel;
    public SidePanel_e SidePanel    
    {
        get => sidePanel;
        set { sidePanel = value;OnPropertyChanged(); }
    }

    #endregion


    //是否被选中
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

    //截图是否ok->用于打印JobCard
    private bool isJobCardOk;
    public bool IsJobCardOk
    {
        get => isJobCardOk;
        set { isJobCardOk = value; OnPropertyChanged();}
    }
    private string imageUrl;
    public string ImageUrl
    {
        get => imageUrl;
        set { imageUrl = value; OnPropertyChanged();}
    }



    //本地文件存在吗->用于作图和导出CutList
    private bool isFilesOk;
    public bool IsFilesOk
    {
        get => isFilesOk;
        set
        {
            isFilesOk = value;
            OnPropertyChanged();
        }
    }

    private string packDir;
    public string PackDir
    {
        get => packDir;
        set { packDir = value; OnPropertyChanged(); }
    }


    //用于CutList和JobCard
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
    //项目类型
    private ProjectType_e projectType;
    public ProjectType_e ProjectType
    {
        get => projectType;
        set { projectType = value; OnPropertyChanged();}
    }
    //发货时间
    private DateTime deliveryDate;
    public DateTime DeliveryDate
    {
        get => deliveryDate;
        set { deliveryDate = value; OnPropertyChanged();}
    }
    private string? projectSpecialNotes;
    public string? ProjectSpecialNotes
    {
        get => projectSpecialNotes;
        set
        {
            projectSpecialNotes = value;
            OnPropertyChanged();
        }
    }

    #endregion
}