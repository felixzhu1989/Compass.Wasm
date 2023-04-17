using Compass.Wasm.Shared.DataService;

namespace Compass.Wasm.Shared.ProjectService;

public class ModuleDto : BaseDto
{
    #region 本身的属性
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
    #endregion
    
    #region 额外属性
    
    #region 长宽高侧板
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

    #region 查看工程图
    private bool isDrawingOk;
    public bool IsDrawingOk
    {
        get => isDrawingOk;
        set { isDrawingOk = value; OnPropertyChanged(); }
    }
    private string? drawingUrl;
    public string? DrawingUrl
    {
        get => drawingUrl;
        set { drawingUrl = value; OnPropertyChanged(); }
    }
    #endregion

    #region 作图和导出CutList
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
    #endregion

    #region 用于CutList和JobCard
    //JobCard是否ok，与制图参数和截图一起判断，与->用于打印JobCard
    private bool isJobCardOk;
    public bool IsJobCardOk
    {
        get => isJobCardOk;
        set { isJobCardOk = value; OnPropertyChanged(); }
    }
    //图片Url
    private string? imageUrl;
    public string? ImageUrl
    {
        get => imageUrl;
        set { imageUrl = value; OnPropertyChanged(); }
    }
    //ODP号
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
    //项目名称
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
    //图纸Item编号
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
        set { projectType = value; OnPropertyChanged(); }
    }
    //发货时间
    private DateTime deliveryDate;
    public DateTime DeliveryDate
    {
        get => deliveryDate;
        set { deliveryDate = value; OnPropertyChanged(); }
    }
    //项目特殊要求
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

    #endregion

}