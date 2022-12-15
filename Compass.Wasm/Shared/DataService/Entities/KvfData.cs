namespace Compass.Wasm.Shared.DataService.Entities;

public record KvfData:ModuleData
{
    public SidePanel SidePanel { get; set; }//大侧板：左, 右, 中, 双
    //排风口参数
    public double MiddleToRight { get;  set; }//中心距离右端
    public double ExhaustSpigotLength { get; set; }
    public double ExhaustSpigotWidth { get;  set; }
    public double ExhaustSpigotHeight { get;  set; }
    public int ExhaustSpigotNumber { get;  set; }//前端界面input设置为number，min=1,max=2
    //新风口参数
    public double SupplySpigotNumber { get;  set; }//前端界面input设置为number，min=1,max=2
    public double SupplySpigotDistance { get;  set; }
    //灯具类型
    public LightType LightType { get;  set; }//长灯, 短灯, 筒灯60, 筒灯140
    public int SpotLightNumber { get;  set; }//前端界面input设置为number，min=1
    public double SpotLightDistance { get;  set; } = 400d;//默认为400
    
    //其他配置
    public bool LedLogo { get;  set; }
    public DrainType DrainType { get;  set; }
    public bool WaterCollection { get;  set; }
    public bool BackToBack { get;  set; }
    public bool BackCj { get;  set; }
    public bool CoverBoard { get;  set; }

    //Ansul
    public bool Ansul { get;  set; }
    public AnsulSide AnsulSide { get;  set; }
    public AnsulDetector AnsulDetector { get;  set; }
    public int AnsulDrop { get;  set; }//前端界面input设置为number，min=0
    //Marvel
    public bool Marvel { get;  set; }
    
    public override bool Accept(string model)
    {
        return model.ToLower().Equals("kvf");
    }
}