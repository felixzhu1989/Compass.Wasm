using Compass.Wasm.Shared.DataService;

namespace Compass.DataService.Domain.Entities;

public record UvfData:ModuleData
{
    public SidePanel SidePanel { get; set; }//大侧板：左, 右, 中, 双
    //排风口参数
    public double MiddleToRight { get; private set; }//中心距离右端
    public double ExhaustSpigotLength { get; private set; }
    public double ExhaustSpigotWidth { get; private set; }
    public double ExhaustSpigotHeight { get; private set; }
    public int ExhaustSpigotNumber { get; private set; }//前端界面input设置为number，min=1,max=2
    //新风口参数
    public double SupplySpigotNumber { get; private set; }//前端界面input设置为number，min=1,max=2
    public double SupplySpigotDistance { get; private set; }

    //灯具类型
    public LightType LightType { get; private set; }//长灯, 短灯, 筒灯60, 筒灯140
    public int SpotLightNumber { get; private set; }//前端界面input设置为number，min=1
    public double SpotLightDistance { get; private set; } = 400d;//默认为400
    //UV灯类型
    public UvLightType UvLightType { get; private set; }
    public bool BlueTooth { get; private set; }
    //其他配置
    public bool LedLogo { get; private set; }
    public DrainType DrainType { get; private set; }
    public bool WaterCollection { get; private set; }
    public bool BackToBack { get; private set; }
    public bool BackCj { get; private set; }
    public bool CoverBoard { get; private set; }

    //Ansul
    public bool Ansul { get; private set; }
    public AnsulSide AnsulSide { get; private set; }
    public AnsulDetector AnsulDetector { get; private set; }
    public int AnsulDrop { get; private set; }//前端界面input设置为number，min=0
    //Marvel
    public bool Marvel { get; private set; }
}