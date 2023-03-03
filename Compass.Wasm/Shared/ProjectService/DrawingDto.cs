using System.Collections.ObjectModel;

namespace Compass.Wasm.Shared.ProjectService;

public class DrawingDto:BaseDto,IComparable<DrawingDto>
{
    public Guid ProjectId { get; set; }

    private string itemNumber;
    public string ItemNumber { get=>itemNumber;
        set { itemNumber = value;OnPropertyChanged(); }
    }

    public string? DrawingUrl { get; set; }


    private ObservableCollection<ModuleDto> moduleDtos=new();
    public ObservableCollection<ModuleDto> ModuleDtos
    {
        get => moduleDtos;
        set { moduleDtos = value; OnPropertyChanged();}
    }



    
    
    public int CompareTo(DrawingDto? other)
    {
        if (other!=null)
        {
            return ItemNumber.CompareTo(other.ItemNumber); //升序
        }
        return 1;//空值比较大，返回1
    }
}