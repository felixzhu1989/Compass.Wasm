namespace Compass.Wasm.Shared.Categories;

public class ProductDto:BaseDto
{
    public int SequenceNumber { get;set; }

    private string name;
    public string Name { get=>name;
        set { name = value;OnPropertyChanged(); }
    }

    private Sbu_e sbu;
    public Sbu_e Sbu { get=>sbu;
        set { sbu = value;OnPropertyChanged(); }
    }

	private List<ModelDto> modelDtos=new List<ModelDto>();
    public List<ModelDto> ModelDtos
	{
		get => modelDtos;
        set { modelDtos = value;OnPropertyChanged(); }
	}


}