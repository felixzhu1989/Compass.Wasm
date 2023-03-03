namespace Compass.Wasm.Shared.CategoryService;
public class ModelDto:BaseDto
{
    public int SequenceNumber { get; set; }

    private Guid productId;
    public Guid ProductId { get=>productId;
        set { productId = value;OnPropertyChanged(); }
    }

    private string name;
    public string Name
    {
        get => name;
        set { name = value; OnPropertyChanged(); }
    }

    private double workload;
    public double Workload { get=>workload;
        set { workload = value;OnPropertyChanged(); }
    }

	private List<ModelTypeDto> modelTypeDtos=new();
    public List<ModelTypeDto> ModelTypeDtos
	{
		get => modelTypeDtos;
        set { modelTypeDtos = value;OnPropertyChanged(); }
	}

}