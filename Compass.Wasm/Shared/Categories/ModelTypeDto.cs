namespace Compass.Wasm.Shared.Categories;

public class ModelTypeDto:BaseDto
{
    public int SequenceNumber { get; set; }

    private Guid modelId;
    public Guid ModelId { get=>modelId;
        set { modelId = value;OnPropertyChanged(); }
    }

    private string name;
    public string Name
    {
        get => name;
        set { name = value; OnPropertyChanged(); }
    }

    //增补，显示模型名称
    private string modelName;
    public string ModelName
    {
        get => modelName;
        set { modelName = value; OnPropertyChanged(); }
    }

    private string description;
    public string Description { get=>description;
        set { description = value;OnPropertyChanged(); }
    }

    private double length;
    public double Length { get=>length;
        set { length = value; OnPropertyChanged();}
    }

    private double width;
    public double Width { get=>width;
        set { width = value;OnPropertyChanged(); }
    }

    private double height;
    public double Height { get=>height;
        set { height = value;OnPropertyChanged(); }
    }


    #region 附加属性
    private string product;
    public string Product
    {
        get => product;
        set { product = value; OnPropertyChanged(); }
    }
    private Guid productId;
    public Guid ProductId
    {
        get => productId;
        set { productId = value; OnPropertyChanged(); }
    }
    private string model;
    public string Model
    {
        get => model;
        set { model = value; OnPropertyChanged(); }
    }
    private int modelSeqNumber;
    public int ModelSeqNumber
    {
        get => modelSeqNumber;
        set { modelSeqNumber = value; OnPropertyChanged(); }
    }
    #endregion
}