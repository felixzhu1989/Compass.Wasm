using Compass.Wasm.Shared.Categories;

namespace Compass.Wasm.Shared.Projects;

public class PackingListDto : BaseDto
{
	//产品类型
	private ProductType_e productType;
    public ProductType_e ProductType
	{
		get => productType;
        set { productType = value; OnPropertyChanged();}
	}
	//第几批,不分批则不填写
	private string? batchName;
    public string? BatchName
	{
		get => batchName;
        set { batchName = value; OnPropertyChanged(); }
	}

	//托盘list
    public List<PalletDto> PalletDtos { get; set; }=new ();
	//配件list
    public List<AccessoryDto> AccessoryDtos { get; set; }=new ();
}