using System.Collections.ObjectModel;

namespace Compass.Wasm.Shared.Plans;

public class PackingListDto:BaseDto
{
    #region 基本信息
    private Guid? mainPlanId;
    public Guid? MainPlanId
    {
        get => mainPlanId;
        set { mainPlanId = value; OnPropertyChanged(); }
    }//关联主计划Id

    private Product_e product;
    public Product_e Product
    {
        get => product;
        set { product = value; OnPropertyChanged(); }
    }//装箱清单产品类型

    private string packingType;
    public string PackingType
    {
        get => packingType;
        set { packingType = value; OnPropertyChanged(); }
    }//包装形式

    private string deliveryType;
    public string DeliveryType
    {
        get => deliveryType;
        set { deliveryType = value; OnPropertyChanged(); }
    }//发货形式 

    private string? assyPath;
    public string? AssyPath
    {
        get=>assyPath;
        set { assyPath = value;OnPropertyChanged(); }
    }//天花烟罩时，需要填写总装地址
    #endregion

    #region 扩展查询
    private int? batch;
    public int? Batch
    {
        get => batch;
        set { batch = value;OnPropertyChanged(); }
    }//分批
    private Guid? projectId;
    public Guid? ProjectId
    {
        get => projectId;
        set { projectId = value; OnPropertyChanged(); }
    }//项目Id

    private string? projectName;
    public string? ProjectName
    {
        get => projectName;
        set { projectName = value;OnPropertyChanged(); }
    }//根据计划查询得出

    private DateTime finishTime;
    public DateTime FinishTime 
    { 
        get=>finishTime; 
        set { finishTime = value;OnPropertyChanged(); }
    }


    private ObservableCollection<PackingItemDto> packingItemDtos=new ();
    public ObservableCollection<PackingItemDto> PackingItemDtos
    {
        get => packingItemDtos ;
        set { packingItemDtos = value; OnPropertyChanged();}
    }//查询获取PackingItem列表


    #endregion
}