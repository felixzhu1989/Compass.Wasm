namespace Compass.Wasm.Shared.ProjectService;

public class ProjectSummaryDto:BaseDto
{
    //订单总数
    private int sum;
    public int Sum
	{
		get => sum;
        set { sum = value; OnPropertyChanged();}
	}
    //计划,制图,生产,入库,发货,结束
    private int planCount;
    public int PlanCount
    {
        get => planCount;
        set { planCount = value; OnPropertyChanged(); }
    }
    private int drawingCount;
    public int DrawingCount
    {
        get => drawingCount;
        set { drawingCount = value; OnPropertyChanged(); }
    }
    private int productionCount;
    public int ProductionCount
    {
        get => productionCount;
        set { productionCount = value; OnPropertyChanged(); }
    }
    private int warehousingCount;
    public int WarehousingCount
    {
        get => warehousingCount;
        set { warehousingCount = value; OnPropertyChanged(); }
    }
    private int shippingCount;
    public int ShippingCount
    {
        get => shippingCount;
        set { shippingCount = value; OnPropertyChanged(); }
    }
    private int completedCount;
    public int CompletedCount
    {
        get => completedCount;
        set { completedCount = value; OnPropertyChanged(); }
    }
}