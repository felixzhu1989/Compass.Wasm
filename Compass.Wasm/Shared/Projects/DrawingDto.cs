using System.Collections.ObjectModel;

namespace Compass.Wasm.Shared.Projects;

public class DrawingDto:BaseDto,IComparable<DrawingDto>
{
    public Guid ProjectId { get; set; }

    private string itemNumber;
    public string ItemNumber { get=>itemNumber;
        set { itemNumber = value;OnPropertyChanged(); }
    }
    private int batch;
    public int Batch
    {
        get => batch;
        set { batch = value; OnPropertyChanged();}
    }




    private string? drawingUrl;
    public string? DrawingUrl
    {
        get => drawingUrl;
        set { drawingUrl = value; OnPropertyChanged(); }
    }

    private string? imageUrl;
    public string? ImageUrl
    {
        get => imageUrl;
        set { imageUrl = value; OnPropertyChanged(); }
    }




    #region 附加查询属性和方法

    private bool isDrawingOk;
    public bool IsDrawingOk
    {
        get => isDrawingOk;
        set { isDrawingOk = value; OnPropertyChanged(); }
    }

    private ObservableCollection<ModuleDto> moduleDtos = new();
    public ObservableCollection<ModuleDto> ModuleDtos
    {
        get => moduleDtos;
        set { moduleDtos = value; OnPropertyChanged(); }
    }

    public int CompareTo(DrawingDto? other)
    {
        if (other!=null)
        {
            return ItemNumber.CompareTo(other.ItemNumber); //升序
        }
        return 1;//空值比较大，返回1
    } 
    #endregion
}