using ImTools;
using SolidWorks.Interop.sldworks;

namespace Compass.Wpf.SwServices;

public class CeilingService : BaseSwService, ICeilingService
{
    public readonly IExhaustService ExhaustService;
    public CeilingService(IContainerProvider provider) : base(provider)
    {
        ExhaustService = provider.Resolve<IExhaustService>();
    }

    #region CJ腔
    public void Cj300(ModelDoc2 swModelTop, AssemblyDoc swAssyTop, string suffix, string module, CjData data)
    {
        var cjNumber = (int)((data.Length - 40d) / 30d);//天花烟罩马蹄形CJ孔阵列距离为30
        var firstCjDis = (data.Length - 30d * cjNumber) / 2;
        if (firstCjDis < 15d)
        {
            firstCjDis += 15d;
        }

        //CJ脖颈
        CjSpigot(swAssyTop, suffix, data.CjSpigotDirection, "FNCJ0010-1", "5201990413-1");
        //侧板
        CjSidePanel(swAssyTop, suffix, data.SidePanel, "FNCJ0017-1", "FNCJ0019-1", "FNCJ0018-1", "FNCJ0019-2");

        //其余零件
        swAssyTop.ChangePartLength(suffix, "FNCJ0021-1", "Length@SketchBase", data.Length, Aggregator);
        swAssyTop.ChangePartLength(suffix, "FNCJ0016-1", "Length@SketchBase", data.Length-10d, Aggregator);
        FNCJ0022(swAssyTop, suffix, "FNCJ0022-1", data.Length, data.CjSpigotToRight);

        //重命名CJ腔体
        var swCompLevel2 = swAssyTop.RenameComp(suffix, "CJ300", module, "FNCJ0020-1", data.Length, data.Width, Aggregator);
        if (swCompLevel2 != null)
        {
            FNCJ0020(swCompLevel2, data.Length, firstCjDis, data.BcjSide, data.LeftEndDis, data.RightEndDis, data.LeftBeamType, data.LeftDbToRight, data.RightBeamType, data.RightDbToLeft, data.LksSide, data.GutterSide, data.LeftGutterWidth, data.RightGutterWidth);
        }

    }

    public void Cj330(ModelDoc2 swModelTop, AssemblyDoc swAssyTop, string suffix, string module, CjData data)
    {
        var cjNumber = (int)((data.Length - 40d) / 30d);//天花烟罩马蹄形CJ孔阵列距离为30
        var firstCjDis = (data.Length - 30d * cjNumber) / 2;
        if (firstCjDis < 15d)
        {
            firstCjDis += 15d;
        }

        //CJ脖颈
        CjSpigot(swAssyTop, suffix, data.CjSpigotDirection, "FNCJ0010-1", "5201990413-1");
        //侧板
        CjSidePanel(swAssyTop, suffix, data.SidePanel, "FNCJ0025-1", "FNCJ0027-1", "FNCJ0026-1", "FNCJ0027-2");

        //其余零件
        swAssyTop.ChangePartLength(suffix, "FNCJ0024-1", "Length@SketchBase", data.Length, Aggregator);
        swAssyTop.ChangePartLength(suffix, "FNCJ0016-1", "Length@SketchBase", data.Length-10d, Aggregator);
        FNCJ0022(swAssyTop, suffix, "FNCJ0022-1", data.Length, data.CjSpigotToRight);

        //重命名CJ腔体
        var swCompLevel2 = swAssyTop.RenameComp(suffix, "CJ330", module, "FNCJ0023-1", data.Length, data.Width, Aggregator);
        if (swCompLevel2 != null)
        {
            FNCJ0020(swCompLevel2, data.Length, firstCjDis, data.BcjSide, data.LeftEndDis, data.RightEndDis, data.LeftBeamType, data.LeftDbToRight, data.RightBeamType, data.RightDbToLeft, data.LksSide, data.GutterSide, data.LeftGutterWidth, data.RightGutterWidth);
        }
    }

    public void Bcj300(ModelDoc2 swModelTop, AssemblyDoc swAssyTop, string suffix, string module, CjData data)
    {
        var cjNumber = (int)((data.Length - 40d) / 30d);//天花烟罩马蹄形CJ孔阵列距离为30
        var firstCjDis = (data.Length - 30d * cjNumber) / 2;
        if (firstCjDis < 15d)
        {
            firstCjDis += 15d;
        }

        //CJ脖颈
        CjSpigot(swAssyTop, suffix, data.CjSpigotDirection, "FNCJ0010-1", "5201990413-1");
        //侧板
        CjSidePanel(swAssyTop, suffix, data.SidePanel, "FNCJ0003-1", "FNCJ0014-1", "FNCJ0004-1", "FNCJ0014-2");

        //其余零件
        swAssyTop.ChangePartLength(suffix, "FNCJ0016-1", "Length@SketchBase", data.Length-10d, Aggregator);


        //重命名BCJ腔体
        var swCompLevel2 = swAssyTop.RenameComp(suffix, "BCJ300", module, "FNCJ0015-1", data.Length, data.Width, Aggregator);
        if (swCompLevel2 != null)
        {
            FNCJ0015(swCompLevel2, data.Length, firstCjDis, data.CjSpigotToRight);
        }


    }

    public void Bcj330(ModelDoc2 swModelTop, AssemblyDoc swAssyTop, string suffix, string module, CjData data)
    {
        var cjNumber = (int)((data.Length - 40d) / 30d);//天花烟罩马蹄形CJ孔阵列距离为30
        var firstCjDis = (data.Length - 30d * cjNumber) / 2;
        if (firstCjDis < 15d)
        {
            firstCjDis += 15d;
        }

        //CJ脖颈
        CjSpigot(swAssyTop, suffix, data.CjSpigotDirection, "FNCJ0010-1", "5201990413-1");
        //侧板
        CjSidePanel(swAssyTop, suffix, data.SidePanel, "FNCJ0042-1", "FNCJ0044-1", "FNCJ0043-1", "FNCJ0044-2");

        //其余零件
        swAssyTop.ChangePartLength(suffix, "FNCJ0016-1", "Length@SketchBase", data.Length-10d, Aggregator);


        //重命名BCJ腔体
        var swCompLevel2 = swAssyTop.RenameComp(suffix, "BCJ330", module, "FNCJ0045-1", data.Length, data.Width, Aggregator);
        if (swCompLevel2 != null)
        {
            FNCJ0015(swCompLevel2, data.Length, firstCjDis, data.CjSpigotToRight);
        }
    }

    public void Nocj300(ModelDoc2 swModelTop, AssemblyDoc swAssyTop, string suffix, string module, CjData data)
    {
        //侧板
        NocjSidePanel(swAssyTop, suffix, data.SidePanel, data.NocjBackSide, data.Width, "FNCJ0008-1", "FNCJ0007-1", "FNCJ0009-1", "FNCJ0007-2");

        //重命名BCJ腔体
        var swCompLevel2 = swAssyTop.RenameComp(suffix, "NOCJ300", module, "FNCJ0001-1", data.Length, data.Width, Aggregator);
        if (swCompLevel2 != null)
        {
            FNCJ0001(swCompLevel2, data.Length, data.Width, data.BcjSide, data.LeftEndDis, data.RightEndDis, data.LeftBeamType, data.LeftDbToRight, data.RightBeamType, data.RightDbToLeft, data.LksSide, data.GutterSide, data.LeftGutterWidth, data.RightGutterWidth, data.NocjSide, data.NocjBackSide);
        }
    }

    public void Nocj330(ModelDoc2 swModelTop, AssemblyDoc swAssyTop, string suffix, string module, CjData data)
    {
        //侧板
        NocjSidePanel(swAssyTop, suffix, data.SidePanel, data.NocjBackSide, data.Width, "FNCJ0055-1", "FNCJ0054-1", "FNCJ0056-1", "FNCJ0054-2");

        //重命名BCJ腔体
        var swCompLevel2 = swAssyTop.RenameComp(suffix, "NOCJ330", module, "FNCJ0051-1", data.Length, data.Width, Aggregator);
        if (swCompLevel2 != null)
        {
            FNCJ0001(swCompLevel2, data.Length, data.Width, data.BcjSide, data.LeftEndDis, data.RightEndDis, data.LeftBeamType, data.LeftDbToRight, data.RightBeamType, data.RightDbToLeft, data.LksSide, data.GutterSide, data.LeftGutterWidth, data.RightGutterWidth, data.NocjSide, data.NocjBackSide);
        }
    }

    public void Nocj340(ModelDoc2 swModelTop, AssemblyDoc swAssyTop, string suffix, string module, CjData data)
    {
        //侧板
        Nocj340SidePanel(swAssyTop, suffix, data.SidePanel, data.Width, "FNCS0017-1", "FNCS0020-1", "FNCS0018-1", "FNCS0020-2");

        //重命名BCJ腔体
        var swCompLevel2 = swAssyTop.RenameComp(suffix, "NOCJ340", module, "FNCS0016-1", data.Length, data.Width, Aggregator);
        if (swCompLevel2 != null)
        {
            FNCS0016(swCompLevel2, data.Length, data.Width, data.BcjSide, data.LeftEndDis, data.RightEndDis, data.LeftBeamType, data.LeftDbToRight, data.RightBeamType, data.RightDbToLeft, data.LksSide, data.GutterSide, data.LeftGutterWidth, data.RightGutterWidth, data.NocjSide, data.NocjBackSide, data.DpSide);
        }
    }

    #endregion

    #region DP腔体
    public void Dp330(ModelDoc2 swModelTop, AssemblyDoc swAssyTop, string suffix, string module, DpData data)
    {
        //吊装孔
        const double sideDis = 150d;
        const double liftingMinDis = 500d;
        var ligtingNumber = Math.Ceiling((data.Length - 2*sideDis) / liftingMinDis);
        ligtingNumber = ligtingNumber < 2 ? 2 : ligtingNumber;
        var ligtingDis = (data.Length - 2*sideDis)/(ligtingNumber-1);
        swModelTop.ChangeDim("Dis@LocalLPatternLifting", ligtingDis);

        //侧板
        DpSidePanel(swAssyTop, suffix, data.SidePanel, "FNCS0003-1", "FNCS0005-1", "FNCO0001[WPSDP330]-1", "FNCS0004-1", "FNCS0005-2", "FNCO0001[WPSDP330]-2");

        //背板
        FNCS0002(swAssyTop, suffix, "FNCS0002-1", data.Length, data.DpBackSide);

        //重命名DP腔体
        var swCompLevel2 = swAssyTop.RenameComp(suffix, "DP330", module, "FNCS0001-1", data.Length, data.Width, Aggregator);
        if (swCompLevel2 != null)
        {
            FNCS0001(swCompLevel2, data.Length, data.Width, data.BcjSide, data.LeftEndDis, data.RightEndDis, data.LeftBeamType, data.LeftDbToRight, data.RightBeamType, data.RightDbToLeft, data.LksSide, data.GutterSide, data.LeftGutterWidth, data.RightGutterWidth, data.NocjSide, data.NocjBackSide, data.DpSide, data.DpBackSide, data.DpDrainType);
        }
    }

    public void Dp340(ModelDoc2 swModelTop, AssemblyDoc swAssyTop, string suffix, string module, DpData data)
    {
        //侧板
        DpSidePanel(swAssyTop, suffix, data.SidePanel, "FNCS0017-1", "FNCS0019-1", "FNCO0016[WPSDP340]-1", "FNCS0018-1", "FNCS0019-2", "FNCO0016[WPSDP340]-2");

        //重命名DP腔体
        var swCompLevel2 = swAssyTop.RenameComp(suffix, "DP340", module, "FNCS0015-1", data.Length, data.Width, Aggregator);
        if (swCompLevel2 != null)
        {
            FNCS0001(swCompLevel2, data.Length, data.Width, data.BcjSide, data.LeftEndDis, data.RightEndDis, data.LeftBeamType, data.LeftDbToRight, data.RightBeamType, data.RightDbToLeft, data.LksSide, data.GutterSide, data.LeftGutterWidth, data.RightGutterWidth, data.NocjSide, data.NocjBackSide, data.DpSide, data.DpBackSide, data.DpDrainType);
        }
    }

    public void DpCj330(ModelDoc2 swModelTop, AssemblyDoc swAssyTop, string suffix, string module, DpData data)
    {
        #region DP部分
        var swAssyDp = swAssyTop.GetSubAssemblyDoc(out var swModelDp, suffix, "DP_CJ_Inner330-1", Aggregator);
        //吊装孔
        const double sideDis = 150d;
        const double liftingMinDis = 500d;
        var ligtingNumber = Math.Ceiling((data.Length - 2*sideDis) / liftingMinDis);
        ligtingNumber = ligtingNumber < 2 ? 2 : ligtingNumber;
        var ligtingDis = (data.Length - 2*sideDis)/(ligtingNumber-1);
        swModelDp.ChangeDim("Dis@LocalLPatternLifting", ligtingDis);

        //侧板
        DpSidePanel(swAssyDp, suffix, data.SidePanel, "FNCS0011-1", "FNCS0013-1", "FNCO0002[WPSDP200]-1", "FNCS0012-1", "FNCS0013-2", "FNCO0002[WPSDP200]-2");

        //重命名DP腔体
        var swCompDp = swAssyDp.RenameComp(suffix, "DPCJ330", module, "FNCS0001-1", data.Length, data.Width, Aggregator);
        if (swCompDp != null)
        {
            FNCS0001(swCompDp, data.Length, data.Width, data.BcjSide, data.LeftEndDis, data.RightEndDis, data.LeftBeamType, data.LeftDbToRight, data.RightBeamType, data.RightDbToLeft, data.LksSide, data.GutterSide, data.LeftGutterWidth, data.RightGutterWidth, data.NocjSide, data.NocjBackSide, data.DpSide, data.DpBackSide, data.DpDrainType);
        }
        #endregion

        #region CJ部分
        var swAssyCj = swAssyTop.GetSubAssemblyDoc(out var swModelCj, suffix, "CJ_330-1", Aggregator);
        var cjNumber = (int)((data.Length - 40d) / 30d);//天花烟罩马蹄形CJ孔阵列距离为30
        var firstCjDis = (data.Length - 30d * cjNumber) / 2;
        if (firstCjDis < 15d)
        {
            firstCjDis += 15d;
        }

        //CJ脖颈
        CjSpigot(swAssyCj, suffix, data.CjSpigotDirection, "FNCJ0010-1", "5201990413-1");
        //侧板
        CjSidePanel(swAssyCj, suffix, data.SidePanel, "FNCJ0025-1", "FNCJ0027-1", "FNCJ0026-1", "FNCJ0027-2");

        //其余零件
        swAssyCj.ChangePartLength(suffix, "FNCJ0024-1", "Length@SketchBase", data.Length, Aggregator);
        swAssyCj.ChangePartLength(suffix, "FNCJ0016-1", "Length@SketchBase", data.Length-10d, Aggregator);
        FNCJ0022(swAssyCj, suffix, "FNCJ0022-1", data.Length, data.CjSpigotToRight);

        //不需要重命名CJ腔体
        var swCompCj = swAssyCj.UnSuppress(suffix, "FNCJ0023-1", Aggregator);
        FNCJ0020(swCompCj, data.Length, firstCjDis, BcjSide_e.NA, 0, 0, BeamType_e.NA, 0, BeamType_e.NA, 0, LksSide_e.NA, GutterSide_e.NA, 0, 0);

        #endregion
    }
    #endregion

    #region LFU
    public void LfuSa(ModelDoc2 swModelTop, AssemblyDoc swAssyTop, string suffix, string module, LfuData data)
    {
        //重命名LFU腔体
        var swCompLevel2 = swAssyTop.RenameComp(suffix, "LFUSA", module, "FNCA0001-1", data.Length, data.Width, Aggregator);
        if (swCompLevel2 != null)
        {
            FNCA0001(swCompLevel2, data.Length, data.Width, data.SidePanel, data.SupplySpigotNumber, data.SupplySpigotDis, data.SupplySpigotDia, data.Japan);
        }

        #region LFU侧板
        LfuSaSsSidePanel(swAssyTop, suffix, data.SidePanel, data.Width-2d, "FNCA0002-1", "FNCA0003-1", "FNCA0002-2", "FNCA0003-2");
        #endregion

        #region 铝网孔板
        if (data.SidePanel is SidePanel_e.左 or SidePanel_e.双)
        {
            var swAssyLevel1 = swAssyTop.GetSubAssemblyDoc(suffix, "LFU_P-1", Aggregator);
            LfuSaPanel(swAssyLevel1, suffix, module, data.TotalLength, data.Width);
        }
        else
        {
            swAssyTop.Suppress(suffix, "LFU_P-1");
        }
        #endregion

        #region 日本项目
        if (data.Japan)
        {
            swAssyTop.Suppress(suffix, "FNCE0070-1");
            swAssyTop.Suppress("LocalLPatternLifting");
        }
        else
        {
            swAssyTop.UnSuppress(suffix, "FNCE0070-1", Aggregator);
            swAssyTop.UnSuppress("LocalLPatternLifting");
        }
        #endregion
    }

    public void LfuSc(ModelDoc2 swModelTop, AssemblyDoc swAssyTop, string suffix, string module, LfuData data)
    {
        #region 腔体
        //国内散流器长度原样给，日本按照网孔板长度加17
        var netLength = data.Japan ? data.Length +17d : data.Length;
        //重命名LFU腔体
        var swCompLevel2 = swAssyTop.RenameComp(suffix, "LFUSC", module, "FNCA0012-1", netLength, data.Width, Aggregator);
        if (swCompLevel2 != null)
        {
            FNCA0012(swCompLevel2, netLength, data.SupplySpigotNumber, data.SupplySpigotDis, data.SupplySpigotDia);
        }
        #endregion


        #region 铝网孔板
        //国内网孔板长度-10，日本按照原样给
        var panelLength = data.Japan ? data.Length : data.Length-10d;
        var swCompSidePanel = swAssyTop.RenameComp(suffix, "LFUP", module, "FNCA0014-1", panelLength, 500d, Aggregator);
        if (swCompSidePanel != null)
        {
            var swModelStdPanel = (ModelDoc2)swCompSidePanel.GetModelDoc2();
            swModelStdPanel.ChangeDim("Length@SketchBase", panelLength);
        }
        #endregion
    }

    public void LfuSs(ModelDoc2 swModelTop, AssemblyDoc swAssyTop, string suffix, string module, LfuData data)
    {
        //重命名LFU腔体
        var swCompLevel2 = swAssyTop.RenameComp(suffix, "LFUSS", module, "FNCA0016-1", data.Length, data.Width, Aggregator);
        if (swCompLevel2 != null)
        {
            FNCA0016(swCompLevel2, data.Length, data.Width, data.SupplySpigotNumber, data.SupplySpigotDis, data.SupplySpigotDia, data.Japan);
        }

        #region LFU侧板
        LfuSaSsSidePanel(swAssyTop, suffix, data.SidePanel, data.Width-3d, "FNCA0017-1", "FNCA0028-1", "FNCA0017-2", "FNCA0028-2");
        #endregion

        #region 型材
        swAssyTop.ChangePartLength(suffix, "2900600019-1", "Length@Boss-Extrude", data.Length-7d, Aggregator);
        #endregion

        #region 铝网孔板
        var panelLength = data.SidePanel is SidePanel_e.双 ? data.Length-7d : data.SidePanel is SidePanel_e.左 or SidePanel_e.右 ? data.Length-4d : data.Length -1d;
        var panelWidth = data.Width - 38d;
        var swCompSidePanel = swAssyTop.RenameComp(suffix, "LFUP", module, "FNCA0018-1", panelLength, panelWidth, Aggregator);
        if (swCompSidePanel != null)
        {
            var swModelStdPanel = (ModelDoc2)swCompSidePanel.GetModelDoc2();
            swModelStdPanel.ChangeDim("Length@SketchBase", panelLength);
            swModelStdPanel.ChangeDim("Width@SketchBase", panelWidth);
        }
        #endregion
    }

    #endregion

    #region An(Gutter)

    public void An135(ModelDoc2 swModelTop, AssemblyDoc swAssyTop, string suffix, string module, AnData data)
    {
        //IR后盖
        if (data.Marvel)
        {
            swAssyTop.UnSuppress(suffix, "FNCB0001-1", Aggregator);
        }
        else
        {
            swAssyTop.Suppress(suffix, "FNCB0001-1");
        }

        //Ansul探测器后盖
        if (data.AnsulDetectorNumber > 0)
        {
            swAssyTop.UnSuppress(suffix, "5201990405-1", Aggregator);
        }
        else
        {
            swAssyTop.Suppress(suffix, "5201990405-1");
        }

        FNCE0001(swAssyTop, suffix, "FNCE0001-1", data.Width, data.Ansul);

        //重命名LFU腔体
        var swCompLevel2 = swAssyTop.RenameComp(suffix, "AN", module, "FNCE0025-1", data.Length, data.Width, Aggregator);
        if (swCompLevel2 != null)
        {
            FNCE0025(swCompLevel2, data.Length, data.Width, data.Marvel,data.Ansul,data.AnsulDropNumber,data.AnsulDropToFront,data.AnsulDropDis1,data.AnsulDropDis2,data.AnsulDropDis3,data.AnsulDropDis4,data.AnsulDropDis5,data.AnsulDetectorNumber,data.AnsulDetectorEnd,data.AnsulDetectorDis1,data.AnsulDetectorDis2,data.AnsulDetectorDis3,data.AnsulDetectorDis4,data.AnsulDetectorDis5);
        }
    }


    #endregion

    #region SSP
    public void SspFlat(ModelDoc2 swModelTop, AssemblyDoc swAssyTop, string suffix, string module, SspData data)
    {
        //边缘两块W板之间的间距
        var disW = (data.MPanelNumber * 2 - 1) * 500d;
        swAssyTop.ChangeDim("Length@DistanceLength", disW);

        //边缘两块W板(重命名边缘W板),有Z板时宽度500
        var leftWidthW =data.LeftType is PanelType_e.Z ?  500d:data.LeftWidth;
        var swCompLeftW= swAssyTop.RenameComp(suffix, "SSPFW", $"{module}.1", "FNCM0017-1", data.Length - 10d, leftWidthW, Aggregator);
        if (swCompLeftW != null)
        {
            SspFlatPanel(swCompLeftW, data.Length - 10d, leftWidthW, data.LedLight);
        }

        var rightWidthW = data.RightType is PanelType_e.Z ? 500d : data.RightWidth;
        var swCompRightW = swAssyTop.RenameComp(suffix, "SSPFW", $"{module}.2", "FNCM0018-1", data.Length - 10d, rightWidthW, Aggregator);
        if (swCompRightW != null)
        {
            SspFlatPanel(swCompRightW, data.Length - 10d, rightWidthW, data.LedLight);
        }
        

        //如果那边有Z板就解压Z板(重命名边缘Z板)
        if (data.LeftType is PanelType_e.Z)
        {
            var swCompLeftZ = swAssyTop.RenameComp(suffix, "SSPFZ", $"{module}.3", "FNCM0019-1", data.Length - 10d, data.LeftWidth, Aggregator);
            if (swCompLeftZ != null)
            {
                SspFlatPanel(swCompLeftZ, data.Length - 10d, data.LeftWidth, data.LedLight);
            }
        }
        else
        {
            swAssyTop.ForceSuppress(suffix, "FNCM0019-1");
        }

        if (data.RightType is PanelType_e.Z)
        {
            var swCompRightZ = swAssyTop.RenameComp(suffix, "SSPFZ", $"{module}.4", "FNCM0020-1", data.Length - 10d, data.RightWidth, Aggregator);
            if (swCompRightZ != null)
            {
                SspFlatPanel(swCompRightZ, data.Length - 10d, data.RightWidth, data.LedLight);
            }
        }
        else
        {
            swAssyTop.ForceSuppress(suffix, "FNCM0020-1");
        }

        //标准M板(有一个驻场，重命名标准M板)
        var swCompStdM = swAssyTop.RenameComp(suffix, "SSPFM", module, "FNCM0005-1", data.Length - 10d, 500d, Aggregator);
        if (swCompStdM != null)
        {
            SspFlatPanel(swCompStdM, data.Length - 10d, 500d, data.LedLight);
        }

        //(M板数量>1，解压阵列种子，M和W，重命名标准W板)
        if (data.MPanelNumber > 1)
        {
            swAssyTop.ForceUnSuppress(suffix, "FNCM0005-2", Aggregator);

            var swCompStdW = swAssyTop.RenameComp(suffix, "SSPFW", module, "FNCM0004-1", data.Length - 10d, 500d, Aggregator);
            if (swCompStdW != null)
            {
                SspFlatPanel(swCompStdW, data.Length - 10d, 500d, data.LedLight);
            }
        }
        else
        {
            swAssyTop.ForceSuppress(suffix, "FNCM0004-1");
            swAssyTop.ForceSuppress(suffix, "FNCM0005-2");
        }

        //(M板数量>2，解压阵列)
        if (data.MPanelNumber > 2)
        {
            swAssyTop.UnSuppress("LocalLPatternStd");
            swModelTop.ChangeDim("Number@LocalLPatternStd",data.MPanelNumber-1);
        }
        else
        {
            swAssyTop.Suppress("LocalLPatternStd");
        }
    }

    public void SspDome(ModelDoc2 swModelTop, AssemblyDoc swAssyTop, string suffix, string module, SspData data)
    {
        //边缘两块W板之间的间距
        var disW = (data.MPanelNumber * 2 - 1) * 500d;
        swAssyTop.ChangeDim("Length@DistanceLength", disW);
        //用于计算焊接支撑板数量
        var panelNumber = data.MPanelNumber * 2 + 1;


        //边缘两块W板(重命名边缘W板),有Z板时宽度500
        var leftWidthW = data.LeftType is PanelType_e.Z ? 500d : data.LeftWidth;
        var swCompLeftW = swAssyTop.RenameComp(suffix, "SSPDW", $"{module}.1", "FNCM0021-1", data.Length - 10d, leftWidthW, Aggregator);
        if (swCompLeftW != null)
        {
            SspDomePanel(swCompLeftW, data.Length - 810d, leftWidthW, data.LedLight);
        }

        var rightWidthW = data.RightType is PanelType_e.Z ? 500d : data.RightWidth;
        var swCompRightW = swAssyTop.RenameComp(suffix, "SSPDW", $"{module}.2", "FNCM0022-1", data.Length - 10d, rightWidthW, Aggregator);
        if (swCompRightW != null)
        {
            SspDomePanel(swCompRightW, data.Length - 810d, rightWidthW, data.LedLight);
        }


        //如果那边有Z板就解压Z板(重命名边缘Z板)
        if (data.LeftType is PanelType_e.Z)
        {
            panelNumber++;
            var swCompLeftZ = swAssyTop.RenameComp(suffix, "SSPDZ", $"{module}.3", "FNCM0023-1", data.Length - 10d, data.LeftWidth, Aggregator);
            if (swCompLeftZ != null)
            {
                SspDomePanel(swCompLeftZ, data.Length - 810d, data.LeftWidth, data.LedLight);
            }
        }
        else
        {
            swAssyTop.ForceSuppress(suffix, "FNCM0023-1");
        }

        if (data.RightType is PanelType_e.Z)
        {
            panelNumber++;
            var swCompRightZ = swAssyTop.RenameComp(suffix, "SSPDZ", $"{module}.4", "FNCM0024-1", data.Length - 10d, data.RightWidth, Aggregator);
            if (swCompRightZ != null)
            {
                SspDomePanel(swCompRightZ, data.Length - 810d, data.RightWidth, data.LedLight);
            }
        }
        else
        {
            swAssyTop.ForceSuppress(suffix, "FNCM0024-1");
        }

        //标准M板(有一个驻场，重命名标准M板)
        var swCompStdM = swAssyTop.RenameComp(suffix, "SSPDM", module, "FNCM0002-1", data.Length - 10d, 500d, Aggregator);
        if (swCompStdM != null)
        {
            SspDomePanel(swCompStdM, data.Length - 810d, 500d, data.LedLight);
        }

        //(M板数量>1，解压阵列种子，M和W，重命名标准W板)
        if (data.MPanelNumber > 1)
        {
            swAssyTop.ForceUnSuppress(suffix, "FNCM0002-2", Aggregator);

            var swCompStdW = swAssyTop.RenameComp(suffix, "SSPDW", module, "FNCM0001-1", data.Length - 10d, 500d, Aggregator);
            if (swCompStdW != null)
            {
                SspDomePanel(swCompStdW, data.Length - 10, 500d, data.LedLight);
            }
        }
        else
        {
            swAssyTop.ForceSuppress(suffix, "FNCM0001-1");
            swAssyTop.ForceSuppress(suffix, "FNCM0002-2");
        }

        //(M板数量>2，解压阵列)
        if (data.MPanelNumber > 2)
        {
            swAssyTop.UnSuppress("LocalLPatternStd");
            swModelTop.ChangeDim("Number@LocalLPatternStd", data.MPanelNumber-1);
        }
        else
        {
            swAssyTop.Suppress("LocalLPatternStd");
        }

        //灯板焊接支撑阵列
        swModelTop.ChangeDim("Number@LocalLPatternSupport", panelNumber*2);
    }

    #endregion

    #region 通用方法

    #region CJ
    private void CjSpigot(AssemblyDoc swAssyLevel1, string suffix, CjSpigotDirection_e cjSpigotDirection, string upPart, string frontPart)
    {
        if (cjSpigotDirection is CjSpigotDirection_e.CJ脖颈朝上)
        {
            swAssyLevel1.UnSuppress(suffix, upPart, Aggregator);
            swAssyLevel1.Suppress(suffix, frontPart);
        }
        else
        {
            swAssyLevel1.UnSuppress(suffix, frontPart, Aggregator);
            swAssyLevel1.Suppress(suffix, upPart);
        }
    }

    private void CjSidePanel(AssemblyDoc swAssyLevel1, string suffix, SidePanel_e sidePanel, string leftBlindPart, string leftHolePart, string rightBlindPart, string rightHolePart)
    {
        switch (sidePanel)
        {
            case SidePanel_e.左:
                swAssyLevel1.UnSuppress(suffix, leftBlindPart, Aggregator);
                swAssyLevel1.UnSuppress(suffix, rightHolePart, Aggregator);
                swAssyLevel1.Suppress(suffix, leftHolePart);
                swAssyLevel1.Suppress(suffix, rightBlindPart);
                break;
            case SidePanel_e.右:
                swAssyLevel1.UnSuppress(suffix, leftHolePart, Aggregator);
                swAssyLevel1.UnSuppress(suffix, rightBlindPart, Aggregator);
                swAssyLevel1.Suppress(suffix, leftBlindPart);
                swAssyLevel1.Suppress(suffix, rightHolePart);
                break;
            case SidePanel_e.双:
                swAssyLevel1.UnSuppress(suffix, leftBlindPart, Aggregator);
                swAssyLevel1.UnSuppress(suffix, rightBlindPart, Aggregator);
                swAssyLevel1.Suppress(suffix, leftHolePart);
                swAssyLevel1.Suppress(suffix, rightHolePart);
                break;

            case SidePanel_e.中:
            case SidePanel_e.NA:
            default:
                swAssyLevel1.UnSuppress(suffix, leftHolePart, Aggregator);
                swAssyLevel1.UnSuppress(suffix, rightHolePart, Aggregator);
                swAssyLevel1.Suppress(suffix, leftBlindPart);
                swAssyLevel1.Suppress(suffix, rightBlindPart);
                break;
        }
    }

    private void NocjSidePanel(AssemblyDoc swAssyLevel1, string suffix, SidePanel_e sidePanel, NocjBackSide_e nocjBackSide, double width, string leftBlindPart, string leftHolePart, string rightBlindPart, string rightHolePart)
    {
        switch (sidePanel)
        {
            case SidePanel_e.左:
                {
                    var leftBack = nocjBackSide is NocjBackSide_e.两背面NOCJ腔 or NocjBackSide_e.左背面NOCJ腔;
                    BlindPanel(leftBack, leftBlindPart);
                    HolePanel(rightHolePart);

                    swAssyLevel1.Suppress(suffix, leftHolePart);
                    swAssyLevel1.Suppress(suffix, rightBlindPart);
                    break;
                }

            case SidePanel_e.右:
                {
                    var rightBack = nocjBackSide is NocjBackSide_e.两背面NOCJ腔 or NocjBackSide_e.右背面NOCJ腔;
                    BlindPanel(rightBack, rightBlindPart);
                    HolePanel(leftHolePart);

                    swAssyLevel1.Suppress(suffix, leftBlindPart);
                    swAssyLevel1.Suppress(suffix, rightHolePart);
                    break;
                }
            case SidePanel_e.双:
                {
                    var leftBack = nocjBackSide is NocjBackSide_e.两背面NOCJ腔 or NocjBackSide_e.左背面NOCJ腔;
                    BlindPanel(leftBack, leftBlindPart);

                    var rightBack = nocjBackSide is NocjBackSide_e.两背面NOCJ腔 or NocjBackSide_e.右背面NOCJ腔;
                    BlindPanel(rightBack, rightBlindPart);

                    swAssyLevel1.Suppress(suffix, leftHolePart);
                    swAssyLevel1.Suppress(suffix, rightHolePart);
                    break;
                }

            case SidePanel_e.中:
            case SidePanel_e.NA:
            default:
                {
                    HolePanel(leftHolePart);
                    swAssyLevel1.UnSuppress(suffix, rightHolePart, Aggregator);

                    swAssyLevel1.Suppress(suffix, leftBlindPart);
                    swAssyLevel1.Suppress(suffix, rightBlindPart);
                    break;
                }
        }

        void BlindPanel(bool back, string compName)
        {
            var swCompLevel2 = swAssyLevel1.UnSuppress(out var swModelLevel2, suffix, compName, Aggregator);
            swModelLevel2.ChangeDim("Width@SketchBase", width-2d);
            if (back)
            {
                swCompLevel2.Suppress("FilletNoBack");

                swCompLevel2.UnSuppress("FlangeBack");
                swCompLevel2.UnSuppress("FlangeBack");
                swCompLevel2.UnSuppress("FlangeBack");
                swModelLevel2.ChangeDim("Width@FlangeBack", width);
            }
            else
            {
                swCompLevel2.Suppress("FlangeBack");
                swCompLevel2.Suppress("FlangeBack");
                swCompLevel2.Suppress("FlangeBack");

                swCompLevel2.UnSuppress("FilletNoBack");
            }
        }

        void HolePanel(string compName)
        {
            var swCompLevel2 = swAssyLevel1.UnSuppress(out var swModelLevel2, suffix, compName, Aggregator);
            swModelLevel2.ChangeDim("Width@SketchBase", width-2d);
        }
    }

    private void Nocj340SidePanel(AssemblyDoc swAssyLevel1, string suffix, SidePanel_e sidePanel, double width, string leftBlindPart, string leftHolePart, string rightBlindPart, string rightHolePart)
    {
        switch (sidePanel)
        {
            case SidePanel_e.左:
                {
                    BlindPanel(leftBlindPart);
                    HolePanel(rightHolePart);

                    swAssyLevel1.Suppress(suffix, leftHolePart);
                    swAssyLevel1.Suppress(suffix, rightBlindPart);
                    break;
                }

            case SidePanel_e.右:
                {
                    BlindPanel(rightBlindPart);
                    HolePanel(leftHolePart);

                    swAssyLevel1.Suppress(suffix, leftBlindPart);
                    swAssyLevel1.Suppress(suffix, rightHolePart);
                    break;
                }
            case SidePanel_e.双:
                {
                    BlindPanel(leftBlindPart);

                    BlindPanel(rightBlindPart);

                    swAssyLevel1.Suppress(suffix, leftHolePart);
                    swAssyLevel1.Suppress(suffix, rightHolePart);
                    break;
                }

            case SidePanel_e.中:
            case SidePanel_e.NA:
            default:
                {
                    HolePanel(leftHolePart);
                    swAssyLevel1.UnSuppress(suffix, rightHolePart, Aggregator);

                    swAssyLevel1.Suppress(suffix, leftBlindPart);
                    swAssyLevel1.Suppress(suffix, rightBlindPart);
                    break;
                }
        }

        void BlindPanel(string compName)
        {
            var swCompLevel2 = swAssyLevel1.UnSuppress(out var swModelLevel2, suffix, compName, Aggregator);
            swModelLevel2.ChangeDim("Width@SketchBase", width-2d);
        }

        void HolePanel(string compName)
        {
            var swCompLevel2 = swAssyLevel1.UnSuppress(out var swModelLevel2, suffix, compName, Aggregator);
            if (width<90d)
            {
                //宽度45时，解压
                swCompLevel2.UnSuppress("DeleteFace45");
                swCompLevel2.UnSuppress("MoveFace45");
            }
            else
            {
                //宽度90时，压缩
                swCompLevel2.Suppress("MoveFace45");
                swCompLevel2.Suppress("DeleteFace45");
            }
        }
    }

    private void BcjSide(Component2 swCompLevel2, ModelDoc2 swModelLevel2, BcjSide_e bcjSide, ref double leftSbDis, ref double rightSbDis)
    {
        switch (bcjSide)
        {
            case BcjSide_e.左BCJ腔:
                swCompLevel2.UnSuppress("BcjLeft");
                swModelLevel2.ChangeDim("Dis@SketchBcjLeft", leftSbDis);
                leftSbDis += 90d;
                swCompLevel2.Suppress("BcjRight");
                break;
            case BcjSide_e.右BCJ腔:
                swCompLevel2.UnSuppress("BcjRight");
                swModelLevel2.ChangeDim("Dis@SketchBcjRight", rightSbDis);
                rightSbDis += 90d;
                swCompLevel2.Suppress("BcjLeft");
                break;
            case BcjSide_e.两BCJ腔:
                swCompLevel2.UnSuppress("BcjLeft");
                swModelLevel2.ChangeDim("Dis@SketchBcjLeft", leftSbDis);
                leftSbDis += 90d;
                swCompLevel2.UnSuppress("BcjRight");
                swModelLevel2.ChangeDim("Dis@SketchBcjRight", rightSbDis);
                rightSbDis += 90d;
                break;

            case BcjSide_e.无BCJ腔:
            case BcjSide_e.NA:
            default:
                swCompLevel2.Suppress("BcjLeft");
                swCompLevel2.Suppress("BcjRight");
                break;
        }
    }

    private void LeftBeamType(Component2 swCompLevel2, ModelDoc2 swModelLevel2, BeamType_e leftBeamType, LksSide_e lksSide, double length, double leftDbToRight, double leftSbDis, ref double leftLksDis, ref double leftGutterDis)
    {
        swCompLevel2.Suppress("KUcjDb800Left");
        swCompLevel2.Suppress("KUcjSb535Left");
        swCompLevel2.Suppress("KcjSb290Left");
        swCompLevel2.Suppress("KcjSb265Left");
        swCompLevel2.Suppress("UcjSb385Left");
        swCompLevel2.Suppress("KUcwDb800Left");
        swCompLevel2.Suppress("KUcwSb535Left");
        swCompLevel2.Suppress("KcwSb265Left");
        switch (leftBeamType)
        {
            case BeamType_e.KUCJDB800:
                swCompLevel2.Suppress("BcjLeft");//确保没有左BCJ
                swCompLevel2.UnSuppress("KUcjDb800Left");
                swModelLevel2.ChangeDim("Dis@SketchKUcjDb800Left", leftDbToRight+1d);
                //自带灯腔，因此不考虑灯腔的问题
                leftGutterDis = length - leftDbToRight + 1d;
                break;
            case BeamType_e.KUCJSB535:
                swCompLevel2.UnSuppress("KUcjSb535Left");
                swModelLevel2.ChangeDim("Dis@SketchKUcjSb535Left", leftSbDis);
                //自带灯腔，因此不考虑灯腔的问题
                leftGutterDis = leftSbDis + 535d;
                break;
            case BeamType_e.KCJSB290:
                swCompLevel2.UnSuppress("KcjSb290Left");
                swModelLevel2.ChangeDim("Dis@SketchKcjSb290Left", leftSbDis);
                leftLksDis = leftSbDis + 290d;
                leftGutterDis =leftLksDis+ (lksSide is LksSide_e.左LK灯腔 or LksSide_e.两LK灯腔 ? 270d : 0d);
                break;
            case BeamType_e.KCJSB265:
                swCompLevel2.UnSuppress("KcjSb265Left");
                swModelLevel2.ChangeDim("Dis@SketchKcjSb265Left", leftSbDis);
                leftLksDis = leftSbDis + 265d;
                leftGutterDis =leftLksDis+ (lksSide is LksSide_e.左LK灯腔 or LksSide_e.两LK灯腔 ? 270d : 0d);
                break;
            case BeamType_e.UCJSB385:
                swCompLevel2.UnSuppress("UcjSb385Left");
                swModelLevel2.ChangeDim("Dis@SketchUcjSb385Left", leftSbDis);
                leftLksDis = leftSbDis + 385d;
                leftGutterDis =leftLksDis+ (lksSide is LksSide_e.左LK灯腔 or LksSide_e.两LK灯腔 ? 270d : 0d);
                break;
            case BeamType_e.KUCWDB800:
                swCompLevel2.Suppress("BcjLeft");//确保没有左BCJ
                swCompLevel2.UnSuppress("KUcwDb800Left");
                swModelLevel2.ChangeDim("Dis@SketchKUcwDb800Left", leftDbToRight+1d);
                //自带灯腔，因此不考虑灯腔的问题
                leftGutterDis = length - leftDbToRight + 1d;
                break;
            case BeamType_e.KUCWSB535:
                swCompLevel2.UnSuppress("KUcwSb535Left");
                swModelLevel2.ChangeDim("Dis@SketchKUcwSb535Left", leftSbDis);
                //自带灯腔，因此不考虑灯腔的问题
                leftGutterDis = leftSbDis + 535d;
                break;
            case BeamType_e.KCWSB265:
                swCompLevel2.UnSuppress("KcwSb265Left");
                swModelLevel2.ChangeDim("Dis@SketchKcwSb265Left", leftSbDis);
                leftLksDis = leftSbDis + 265d;
                leftGutterDis =leftLksDis+ (lksSide is LksSide_e.左LK灯腔 or LksSide_e.两LK灯腔 ? 270d : 0d);
                break;
            case BeamType_e.NA:
            default:

                break;
        }
    }

    private void RightBeamType(Component2 swCompLevel2, ModelDoc2 swModelLevel2, BeamType_e rightBeamType, LksSide_e lksSide, double length, double rightDbToLeft, double rightSbDis, ref double rightLksDis, ref double rightGutterDis)
    {
        swCompLevel2.Suppress("KUcjDb800Right");
        swCompLevel2.Suppress("KUcjSb535Right");
        swCompLevel2.Suppress("KcjSb290Right");
        swCompLevel2.Suppress("KcjSb265Right");
        swCompLevel2.Suppress("UcjSb385Right");
        swCompLevel2.Suppress("KUcwDb800Right");
        swCompLevel2.Suppress("KUcwSb535Right");
        swCompLevel2.Suppress("KcwSb265Right");
        switch (rightBeamType)
        {
            case BeamType_e.KUCJDB800:
                swCompLevel2.Suppress("BcjRight");//确保没有左BCJ
                swCompLevel2.UnSuppress("KUcjDb800Right");
                swModelLevel2.ChangeDim("Dis@SketchKUcjDb800Right", rightDbToLeft+1d);
                //自带灯腔，因此不考虑灯腔的问题
                rightGutterDis = length - rightDbToLeft + 1d;
                break;
            case BeamType_e.KUCJSB535:
                swCompLevel2.UnSuppress("KUcjSb535right");
                swModelLevel2.ChangeDim("Dis@SketchKUcjSb535right", rightSbDis);
                //自带灯腔，因此不考虑灯腔的问题
                rightGutterDis = rightSbDis + 535d;
                break;
            case BeamType_e.KCJSB290:
                swCompLevel2.UnSuppress("KcjSb290Right");
                swModelLevel2.ChangeDim("Dis@SketchKcjSb290Right", rightSbDis);
                rightLksDis = rightSbDis + 290d;
                rightGutterDis =rightLksDis+ (lksSide is LksSide_e.左LK灯腔 or LksSide_e.两LK灯腔 ? 270d : 0d);
                break;
            case BeamType_e.KCJSB265:
                swCompLevel2.UnSuppress("KcjSb265Right");
                swModelLevel2.ChangeDim("Dis@SketchKcjSb265Right", rightSbDis);
                rightLksDis = rightSbDis + 265d;
                rightGutterDis =rightLksDis+ (lksSide is LksSide_e.左LK灯腔 or LksSide_e.两LK灯腔 ? 270d : 0d);
                break;
            case BeamType_e.UCJSB385:
                swCompLevel2.UnSuppress("UcjSb385Right");
                swModelLevel2.ChangeDim("Dis@SketchUcjSb385Right", rightSbDis);
                rightLksDis = rightSbDis + 385d;
                rightGutterDis =rightLksDis+ (lksSide is LksSide_e.左LK灯腔 or LksSide_e.两LK灯腔 ? 270d : 0d);
                break;
            case BeamType_e.KUCWDB800:
                swCompLevel2.Suppress("Bcjright");//确保没有左BCJ
                swCompLevel2.UnSuppress("KUcwDb800Right");
                swModelLevel2.ChangeDim("Dis@SketchKUcwDb800Right", rightDbToLeft+1d);
                //自带灯腔，因此不考虑灯腔的问题
                rightGutterDis = length - rightDbToLeft + 1d;
                break;
            case BeamType_e.KUCWSB535:
                swCompLevel2.UnSuppress("KUcwSb535Right");
                swModelLevel2.ChangeDim("Dis@SketchKUcwSb535Right", rightSbDis);
                //自带灯腔，因此不考虑灯腔的问题
                rightGutterDis = rightSbDis + 535d;
                break;
            case BeamType_e.KCWSB265:
                swCompLevel2.UnSuppress("KcwSb265Right");
                swModelLevel2.ChangeDim("Dis@SketchKcwSb265Right", rightSbDis);
                rightLksDis = rightSbDis + 265d;
                rightGutterDis =rightLksDis+ (lksSide is LksSide_e.左LK灯腔 or LksSide_e.两LK灯腔 ? 270d : 0d);
                break;
            case BeamType_e.NA:
            default:

                break;
        }
    }

    private void LksSide(Component2 swCompLevel2, ModelDoc2 swModelLevel2, LksSide_e lksSide, double leftLksDis, double rightLksDis)
    {
        switch (lksSide)
        {
            case LksSide_e.左LK灯腔:
                swCompLevel2.UnSuppress("Lks270Left");
                swModelLevel2.ChangeDim("Dis@SketchLks270Left", leftLksDis);
                swCompLevel2.Suppress("Lks270Right");
                break;
            case LksSide_e.右LK灯腔:
                swCompLevel2.UnSuppress("Lks270Right");
                swModelLevel2.ChangeDim("Dis@SketchLks270Right", rightLksDis);
                swCompLevel2.Suppress("Lks270Left");
                break;
            case LksSide_e.两LK灯腔:
                swCompLevel2.UnSuppress("Lks270Left");
                swModelLevel2.ChangeDim("Dis@SketchLks270Left", leftLksDis);
                swCompLevel2.UnSuppress("Lks270Right");
                swModelLevel2.ChangeDim("Dis@SketchLks270Right", rightLksDis);
                break;
            case LksSide_e.无LK灯腔:
            case LksSide_e.NA:
            default:
                swCompLevel2.Suppress("Lks270Left");
                swCompLevel2.Suppress("Lks270Right");
                break;
        }
    }

    private void GutterSide(Component2 swCompLevel2, ModelDoc2 swModelLevel2, GutterSide_e gutterSide, double leftGutterDis, double leftGutterWidth, double rightGutterDis, double rightGutterWidth)
    {
        switch (gutterSide)
        {
            case GutterSide_e.左Ansul腔:
                swCompLevel2.UnSuppress("GutterLeft");
                swModelLevel2.ChangeDim("Dis@SketchGutterLeft", leftGutterDis);
                swModelLevel2.ChangeDim("Width@SketchGutterLeft", leftGutterWidth-2d);
                swModelLevel2.ChangeDim("Pin@SketchGutterLeft", leftGutterWidth-62d);
                swCompLevel2.Suppress("GutterRight");
                break;
            case GutterSide_e.右Ansul腔:
                swCompLevel2.UnSuppress("GutterRight");
                swModelLevel2.ChangeDim("Dis@SketchGutterRight", rightGutterDis);
                swModelLevel2.ChangeDim("Width@SketchGutterRight", rightGutterWidth-2d);
                swModelLevel2.ChangeDim("Pin@SketchGutterRight", rightGutterWidth-62d);
                swCompLevel2.Suppress("GutterLeft");
                break;
            case GutterSide_e.两Ansul腔:
                swCompLevel2.UnSuppress("GutterLeft");
                swModelLevel2.ChangeDim("Dis@SketchGutterLeft", leftGutterDis);
                swModelLevel2.ChangeDim("Width@SketchGutterLeft", leftGutterWidth-2d);
                swModelLevel2.ChangeDim("Pin@SketchGutterLeft", leftGutterWidth-62d);
                swCompLevel2.UnSuppress("GutterRight");
                swModelLevel2.ChangeDim("Dis@SketchGutterRight", rightGutterDis);
                swModelLevel2.ChangeDim("Width@SketchGutterRight", rightGutterWidth-2d);
                swModelLevel2.ChangeDim("Pin@SketchGutterRight", rightGutterWidth-62d);
                break;
            case GutterSide_e.无Ansul腔:
            case GutterSide_e.NA:
            default:
                swCompLevel2.Suppress("GutterLeft");
                swCompLevel2.Suppress("GutterRight");
                break;
        }
    }

    private void NocjSide(Component2 swCompLevel2, ModelDoc2 swModelLevel2, NocjSide_e nocjSide, double width, ref double leftSbDis, ref double rightSbDis)
    {
        switch (nocjSide)
        {
            case NocjSide_e.左NOCJ腔:
                swCompLevel2.UnSuppress("NocjLeft");
                swModelLevel2.ChangeDim("Dis@SketchNocjLeft", leftSbDis);
                swModelLevel2.ChangeDim("Width@SketchNocjLeft", width-2d);
                leftSbDis += width;
                swCompLevel2.Suppress("NocjRight");
                break;
            case NocjSide_e.右NOCJ腔:
                swCompLevel2.UnSuppress("NocjRight");
                swModelLevel2.ChangeDim("Dis@SketchNocjRight", rightSbDis);
                swModelLevel2.ChangeDim("Width@SketchNocjRight", width-2d);
                rightSbDis += width;
                swCompLevel2.Suppress("NocjLeft");
                break;
            case NocjSide_e.两NOCJ腔:
                swCompLevel2.UnSuppress("NocjLeft");
                swModelLevel2.ChangeDim("Dis@SketchNocjLeft", leftSbDis);
                swModelLevel2.ChangeDim("Width@SketchNocjLeft", width-2d);
                leftSbDis += width;
                swCompLevel2.UnSuppress("NocjRight");
                swModelLevel2.ChangeDim("Dis@SketchNocjRight", rightSbDis);
                swModelLevel2.ChangeDim("Width@SketchNocjRight", width-2d);
                rightSbDis += width;
                break;

            case NocjSide_e.无NOCJ腔:
            case NocjSide_e.NA:
            default:
                swCompLevel2.Suppress("NocjLeft");
                swCompLevel2.Suppress("NocjRight");
                break;
        }
    }

    private void NocjBackSide(Component2 swCompLevel2, ModelDoc2 swModelLevel2, NocjBackSide_e nocjBackSide, double width)
    {
        switch (nocjBackSide)
        {
            case NocjBackSide_e.左背面NOCJ腔:
                swCompLevel2.UnSuppress("NocjBackLeft");
                swModelLevel2.ChangeDim("Width@SketchNocjBackLeft", width-2d);

                swCompLevel2.Suppress("NocjBackRight");
                break;
            case NocjBackSide_e.右背面NOCJ腔:
                swCompLevel2.UnSuppress("NocjBackRight");
                swModelLevel2.ChangeDim("Width@SketchNocjBackRight", width-2d);

                swCompLevel2.Suppress("NocjBackLeft");
                break;
            case NocjBackSide_e.两背面NOCJ腔:
                swCompLevel2.UnSuppress("NocjBackLeft");
                swModelLevel2.ChangeDim("Width@SketchNocjBackLeft", width-2d);

                swCompLevel2.UnSuppress("NocjBackRight");
                swModelLevel2.ChangeDim("Width@SketchNocjBackRight", width-2d);

                break;

            case NocjBackSide_e.无背面NOCJ腔:
            case NocjBackSide_e.NA:
            default:
                swCompLevel2.Suppress("NocjBackLeft");
                swCompLevel2.Suppress("NocjBackRight");
                break;
        }
    }
    #endregion

    #region DP
    private void DpSidePanel(AssemblyDoc swAssyLevel1, string suffix, SidePanel_e sidePanel, string leftBlindPart, string leftHolePart, string leftSealingPart, string rightBlindPart, string rightHolePart, string rightSealingPart)
    {
        switch (sidePanel)
        {
            case SidePanel_e.左:
                swAssyLevel1.UnSuppress(suffix, leftBlindPart, Aggregator);
                swAssyLevel1.UnSuppress(suffix, rightHolePart, Aggregator);
                swAssyLevel1.Suppress(suffix, leftHolePart);
                swAssyLevel1.Suppress(suffix, rightBlindPart);
                break;
            case SidePanel_e.右:
                swAssyLevel1.UnSuppress(suffix, leftHolePart, Aggregator);
                swAssyLevel1.UnSuppress(suffix, rightBlindPart, Aggregator);
                swAssyLevel1.Suppress(suffix, leftBlindPart);
                swAssyLevel1.Suppress(suffix, rightHolePart);
                break;
            case SidePanel_e.双:
                swAssyLevel1.UnSuppress(suffix, leftBlindPart, Aggregator);
                swAssyLevel1.UnSuppress(suffix, rightBlindPart, Aggregator);
                swAssyLevel1.Suppress(suffix, leftHolePart);
                swAssyLevel1.Suppress(suffix, rightHolePart);
                break;

            case SidePanel_e.中:
            case SidePanel_e.NA:
            default:
                swAssyLevel1.UnSuppress(suffix, leftHolePart, Aggregator);
                swAssyLevel1.UnSuppress(suffix, rightHolePart, Aggregator);
                swAssyLevel1.Suppress(suffix, leftBlindPart);
                swAssyLevel1.Suppress(suffix, rightBlindPart);
                break;
        }

        WaterproofSealing(swAssyLevel1, suffix, sidePanel, leftSealingPart, rightSealingPart);
    }

    private void CutDpSide(Component2 swCompLevel2, DpSide_e dpSide)
    {
        switch (dpSide)
        {
            case DpSide_e.左DP腔:
                swCompLevel2.UnSuppress("CutDpLeft");
                swCompLevel2.Suppress("CutDpRight");
                break;
            case DpSide_e.右DP腔:
                swCompLevel2.UnSuppress("CutDpRight");
                swCompLevel2.Suppress("CutDpLeft");
                break;
            case DpSide_e.两DP腔:
                swCompLevel2.UnSuppress("CutDpLeft");
                swCompLevel2.UnSuppress("CutDpRight");
                break;
            case DpSide_e.无DP腔:
            case DpSide_e.NA:
            default:
                swCompLevel2.Suppress("CutDpLeft");
                swCompLevel2.Suppress("CutDpRight");
                break;
        }
    }

    private void DpSide(Component2 swCompLevel2, ModelDoc2 swModelLevel2, DpSide_e dpSide, ref double leftSbDis, ref double rightSbDis)
    {
        switch (dpSide)
        {
            case DpSide_e.左DP腔:
                swCompLevel2.UnSuppress("DpLeft");
                swModelLevel2.ChangeDim("Dis@SketchDpLeft", leftSbDis);

                leftSbDis += 90;
                swCompLevel2.Suppress("DpRight");
                break;
            case DpSide_e.右DP腔:
                swCompLevel2.UnSuppress("DpRight");
                swModelLevel2.ChangeDim("Dis@SketchDpRight", rightSbDis);

                rightSbDis += 90;
                swCompLevel2.Suppress("DpLeft");
                break;
            case DpSide_e.两DP腔:
                swCompLevel2.UnSuppress("DpLeft");
                swModelLevel2.ChangeDim("Dis@SketchDpLeft", leftSbDis);

                leftSbDis += 90;
                swCompLevel2.UnSuppress("DpRight");
                swModelLevel2.ChangeDim("Dis@SketchDpRight", rightSbDis);

                rightSbDis += 90;
                break;

            case DpSide_e.无DP腔:
            case DpSide_e.NA:
            default:
                swCompLevel2.Suppress("DpLeft");
                swCompLevel2.Suppress("DpRight");
                break;
        }
    }

    private void DpBackSide(Component2 swCompLevel2, DpBackSide_e dpBackSide)
    {
        switch (dpBackSide)
        {
            case DpBackSide_e.左背面DP腔:
                swCompLevel2.UnSuppress("DpBackLeft");
                swCompLevel2.Suppress("DpBackRight");
                break;
            case DpBackSide_e.右背面DP腔:
                swCompLevel2.UnSuppress("DpBackRight");
                swCompLevel2.Suppress("DpBackLeft");
                break;
            case DpBackSide_e.两背面DP腔:
                swCompLevel2.UnSuppress("DpBackLeft");
                swCompLevel2.UnSuppress("DpBackRight");
                break;

            case DpBackSide_e.无背面DP腔:
            case DpBackSide_e.NA:
            default:
                swCompLevel2.Suppress("DpBackLeft");
                swCompLevel2.Suppress("DpBackRight");
                break;
        }
    }

    private void DpDrainType(Component2 swCompLevel2, DpDrainType_e dpDrainType)
    {
        switch (dpDrainType)
        {
            case DpDrainType_e.左排水管:
                swCompLevel2.UnSuppress("DrainLeft");
                swCompLevel2.Suppress("DrainRight");
                break;
            case DpDrainType_e.右排水管:
                swCompLevel2.UnSuppress("DrainRight");
                swCompLevel2.Suppress("DrainLeft");
                break;
            case DpDrainType_e.无排水管:
            case DpDrainType_e.NA:
            default:
                swCompLevel2.Suppress("DrainLeft");
                swCompLevel2.Suppress("DrainRight");
                break;
        }
    }

    /// <summary>
    /// 防水棉
    /// </summary>
    private void WaterproofSealing(AssemblyDoc swAssyLevel1, string suffix, SidePanel_e sidePanel, string leftPart, string rightPart)
    {
        switch (sidePanel)
        {
            case SidePanel_e.左:
            case SidePanel_e.双:
                swAssyLevel1.UnSuppress(suffix, leftPart, Aggregator);
                swAssyLevel1.UnSuppress(suffix, rightPart, Aggregator);
                break;
            case SidePanel_e.右:
            case SidePanel_e.NA:
            case SidePanel_e.中:
            default:
                swAssyLevel1.Suppress(suffix, leftPart);
                swAssyLevel1.UnSuppress(suffix, rightPart, Aggregator);
                break;
        }
    }
    #endregion

    #region LFU
    private void LfuSaSsSidePanel(AssemblyDoc swAssyLevel1, string suffix, SidePanel_e sidePanel, double width, string leftBlindPart, string leftHolePart, string rightBlindPart, string rightHolePart)
    {
        switch (sidePanel)
        {
            case SidePanel_e.左:
                {
                    ChangeWidth(leftBlindPart);
                    ChangeWidth(rightHolePart);

                    swAssyLevel1.Suppress(suffix, leftHolePart);
                    swAssyLevel1.Suppress(suffix, rightBlindPart);
                    break;
                }
            case SidePanel_e.右:
                {
                    ChangeWidth(leftHolePart);
                    ChangeWidth(rightBlindPart);

                    swAssyLevel1.Suppress(suffix, leftBlindPart);
                    swAssyLevel1.Suppress(suffix, rightHolePart);
                    break;
                }
            case SidePanel_e.双:
                {
                    ChangeWidth(leftBlindPart);
                    ChangeWidth(rightBlindPart);

                    swAssyLevel1.Suppress(suffix, leftHolePart);
                    swAssyLevel1.Suppress(suffix, rightHolePart);
                    break;
                }

            case SidePanel_e.中:
            case SidePanel_e.NA:
            default:
                {
                    ChangeWidth(leftHolePart);
                    ChangeWidth(rightHolePart);

                    swAssyLevel1.Suppress(suffix, leftBlindPart);
                    swAssyLevel1.Suppress(suffix, rightBlindPart);
                    break;
                }
        }
        void ChangeWidth(string compName)
        {
            var swCompLevel2 = swAssyLevel1.UnSuppress(out var swModelLevel2, suffix, compName, Aggregator);
            swModelLevel2.ChangeDim("Width@SketchBase", width);
        }
    }

    private void LfuSaPanel(AssemblyDoc swAssyLevel1, string suffix, string module, double totalLength, double width)
    {
        var swModelLevel1 = (ModelDoc2)swAssyLevel1;
        var stdPanelNumber = (int)((totalLength - 500.5d) / 1000d);//1500直接做成一块
        var sideLength = totalLength - stdPanelNumber * 1000d-100d;//100是两边侧板宽度

        var swCompSidePanel = swAssyLevel1.RenameComp(suffix, "LFUP", module, "FNCA0005-1", sideLength, width-81d, Aggregator);
        if (swCompSidePanel != null)
        {
            var swModelStdPanel = (ModelDoc2)swCompSidePanel.GetModelDoc2();
            swModelStdPanel.ChangeDim("Length@SketchBase", sideLength);
            swModelStdPanel.ChangeDim("Width@SketchBase", width-81d);
        }

        if (stdPanelNumber > 0)
        {
            var swCompStdPanel = swAssyLevel1.RenameComp(suffix, "LFUP", $"{module}.Std", "FNCA0004-1", 1000d, width-81d, Aggregator);
            if (swCompStdPanel != null)
            {
                var swModelStdPanel = (ModelDoc2)swCompStdPanel.GetModelDoc2();
                swModelStdPanel.ChangeDim("Width@SketchBase", width-81d);
            }
        }
        else
        {
            swAssyLevel1.ForceSuppress(suffix, "FNCA0004-1");
        }

        if (stdPanelNumber > 1)
        {
            swAssyLevel1.UnSuppress("LocalLPatternStdPanel");
            swModelLevel1.ChangeDim("Number@LocalLPatternStdPanel", stdPanelNumber);
        }
        else
        {
            swAssyLevel1.Suppress("LocalLPatternStdPanel");
        }

    }

    #endregion

    #region SSP
    private void SspFlatPanel(Component2 swCompLevel1, double length, double width, bool ledLight)
    {
        var swModelLevel1 = (ModelDoc2)swCompLevel1.GetModelDoc2();
        swModelLevel1.ChangeDim("Length@SketchBase", length);
        swModelLevel1.ChangeDim("Width@SketchBase", width);
        if (ledLight)
        {
            swCompLevel1.UnSuppress("LedLight");
        }
        else
        {
            swCompLevel1.Suppress("LedLight");
        }
    }

    private void SspDomePanel(Component2 swCompLevel1, double length, double width, bool ledLight)
    {
        var swModelLevel1 = (ModelDoc2)swCompLevel1.GetModelDoc2();
        swModelLevel1.ChangeDim("Length@SketchBase", length);
        swModelLevel1.ChangeDim("Width@SketchBase", width);
        if (ledLight)
        {
            swCompLevel1.UnSuppress("LedLight");
        }
        else
        {
            swCompLevel1.Suppress("LedLight");
        }
    }

    #endregion




    #endregion

    #region 零件方法

    #region CJ
    private void FNCJ0022(AssemblyDoc swAssyLevel1, string suffix, string partName, double length, double cjSpigotToRight)
    {
        var swCompLevel2 = swAssyLevel1.UnSuppress(out ModelDoc2 swModelLevel2, suffix, partName, Aggregator);
        swModelLevel2.ChangeDim("Length@SketchBase", length);
        swModelLevel2.ChangeDim("Dis@SketchCjSpigot", cjSpigotToRight);
    }

    private void FNCJ0020(Component2 swCompLevel2, double length, double firstCjDis, BcjSide_e bcjSide, double leftEndDis, double rightEndDis, BeamType_e leftBeamType, double leftDbToRight, BeamType_e rightBeamType, double rightDbToLeft, LksSide_e lksSide, GutterSide_e gutterSide, double leftGutterWidth, double rightGutterWidth)
    {
        var swModelLevel2 = (ModelDoc2)swCompLevel2.GetModelDoc2();
        swModelLevel2.ChangeDim("Length@SketchBase", length);
        swModelLevel2.ChangeDim("Dis@SketchCjHoleC", firstCjDis);

        var leftSbDis = leftEndDis+1d;
        var rightSbDis = rightEndDis+1d;
        var leftLksDis = leftSbDis + 265d;
        var rightLksDis = rightSbDis + 265d;
        var leftGutterDis = leftSbDis + 535d;
        var rightGutterDis = rightSbDis + 535d;

        #region BCJ腔
        BcjSide(swCompLevel2, swModelLevel2, bcjSide, ref leftSbDis, ref rightSbDis);
        #endregion

        #region 左侧腔体
        LeftBeamType(swCompLevel2, swModelLevel2, leftBeamType, lksSide, length, leftDbToRight, leftSbDis, ref leftLksDis, ref leftGutterDis);
        #endregion

        #region 右侧腔体right
        RightBeamType(swCompLevel2, swModelLevel2, rightBeamType, lksSide, length, rightDbToLeft, rightSbDis, ref rightLksDis, ref rightGutterDis);
        #endregion

        #region LKS灯腔
        LksSide(swCompLevel2, swModelLevel2, lksSide, leftLksDis, rightLksDis);
        #endregion

        #region Ansul腔(Gutter)
        GutterSide(swCompLevel2, swModelLevel2, gutterSide, leftGutterDis, leftGutterWidth, rightGutterDis, rightGutterWidth);
        #endregion
    }

    private void FNCJ0015(Component2 swCompLevel2, double length, double firstCjDis, double cjSpigotToRight)
    {
        var swModelLevel2 = (ModelDoc2)swCompLevel2.GetModelDoc2();
        swModelLevel2.ChangeDim("Length@SketchBase", length);
        swModelLevel2.ChangeDim("Dis@SketchCjHoleC", firstCjDis);
        swModelLevel2.ChangeDim("Dis@SketchCjSpigot", cjSpigotToRight);
    }


    private void FNCJ0001(Component2 swCompLevel2, double length, double width, BcjSide_e bcjSide, double leftEndDis, double rightEndDis, BeamType_e leftBeamType, double leftDbToRight, BeamType_e rightBeamType, double rightDbToLeft, LksSide_e lksSide, GutterSide_e gutterSide, double leftGutterWidth, double rightGutterWidth, NocjSide_e nocjSide, NocjBackSide_e nocjBackSide)
    {
        var swModelLevel2 = (ModelDoc2)swCompLevel2.GetModelDoc2();
        swModelLevel2.ChangeDim("Length@SketchBase", length);
        swModelLevel2.ChangeDim("Width@Edge-FlangeWidth", width);


        var leftSbDis = leftEndDis+1d;
        var rightSbDis = rightEndDis+1d;
        var leftLksDis = leftSbDis + 265d;
        var rightLksDis = rightSbDis + 265d;
        var leftGutterDis = leftSbDis + 535d;
        var rightGutterDis = rightSbDis + 535d;

        #region BCJ腔
        BcjSide(swCompLevel2, swModelLevel2, bcjSide, ref leftSbDis, ref rightSbDis);
        #endregion

        #region NOCJ腔
        NocjSide(swCompLevel2, swModelLevel2, nocjSide, width, ref leftSbDis, ref rightSbDis);
        #endregion

        #region NOCJ腔Back
        NocjBackSide(swCompLevel2, swModelLevel2, nocjBackSide, width);
        #endregion

        #region 左侧腔体
        LeftBeamType(swCompLevel2, swModelLevel2, leftBeamType, lksSide, length, leftDbToRight, leftSbDis, ref leftLksDis, ref leftGutterDis);
        #endregion

        #region 右侧腔体right
        RightBeamType(swCompLevel2, swModelLevel2, rightBeamType, lksSide, length, rightDbToLeft, rightSbDis, ref rightLksDis, ref rightGutterDis);
        #endregion

        #region LKS灯腔
        LksSide(swCompLevel2, swModelLevel2, lksSide, leftLksDis, rightLksDis);
        #endregion

        #region Ansul腔(Gutter)
        GutterSide(swCompLevel2, swModelLevel2, gutterSide, leftGutterDis, leftGutterWidth, rightGutterDis, rightGutterWidth);
        #endregion
    }

    //日本NOCJ340
    private void FNCS0016(Component2 swCompLevel2, double length, double width, BcjSide_e bcjSide, double leftEndDis, double rightEndDis, BeamType_e leftBeamType, double leftDbToRight, BeamType_e rightBeamType, double rightDbToLeft, LksSide_e lksSide, GutterSide_e gutterSide, double leftGutterWidth, double rightGutterWidth, NocjSide_e nocjSide, NocjBackSide_e nocjBackSide, DpSide_e dpSide)
    {
        var swModelLevel2 = (ModelDoc2)swCompLevel2.GetModelDoc2();
        swModelLevel2.ChangeDim("Length@SketchBase", length);
        swModelLevel2.ChangeDim("Width@Edge-FlangeWidth", width);


        var leftSbDis = leftEndDis+1d;
        var rightSbDis = rightEndDis+1d;
        var leftLksDis = leftSbDis + 265d;
        var rightLksDis = rightSbDis + 265d;
        var leftGutterDis = leftSbDis + 535d;
        var rightGutterDis = rightSbDis + 535d;

        #region BCJ腔
        BcjSide(swCompLevel2, swModelLevel2, bcjSide, ref leftSbDis, ref rightSbDis);
        #endregion

        #region NOCJ腔
        NocjSide(swCompLevel2, swModelLevel2, nocjSide, width, ref leftSbDis, ref rightSbDis);
        #endregion

        #region NOCJ腔Back
        NocjBackSide(swCompLevel2, swModelLevel2, nocjBackSide, width);
        #endregion

        #region DP腔(NOCJ340特有)，DP和NOCJ不需要拧螺丝
        CutDpSide(swCompLevel2, dpSide);
        #endregion

        #region 左侧腔体
        LeftBeamType(swCompLevel2, swModelLevel2, leftBeamType, lksSide, length, leftDbToRight, leftSbDis, ref leftLksDis, ref leftGutterDis);
        #endregion

        #region 右侧腔体right
        RightBeamType(swCompLevel2, swModelLevel2, rightBeamType, lksSide, length, rightDbToLeft, rightSbDis, ref rightLksDis, ref rightGutterDis);
        #endregion

        #region LKS灯腔
        LksSide(swCompLevel2, swModelLevel2, lksSide, leftLksDis, rightLksDis);
        #endregion

        #region Ansul腔(Gutter)
        GutterSide(swCompLevel2, swModelLevel2, gutterSide, leftGutterDis, leftGutterWidth, rightGutterDis, rightGutterWidth);
        #endregion
    }




    #endregion

    #region DP
    private void FNCS0002(AssemblyDoc swAssyLevel1, string suffix, string partName, double length, DpBackSide_e dpBackSide)
    {
        var swCompLevel2 = swAssyLevel1.UnSuppress(out ModelDoc2 swModelLevel2, suffix, partName, Aggregator);
        swModelLevel2.ChangeDim("Length@SketchBase", length);

        DpBackSide(swCompLevel2, dpBackSide);
    }
    private void FNCS0001(Component2 swCompLevel2, double length, double width, BcjSide_e bcjSide, double leftEndDis, double rightEndDis, BeamType_e leftBeamType, double leftDbToRight, BeamType_e rightBeamType, double rightDbToLeft, LksSide_e lksSide, GutterSide_e gutterSide, double leftGutterWidth, double rightGutterWidth, NocjSide_e nocjSide, NocjBackSide_e nocjBackSide, DpSide_e dpSide, DpBackSide_e dpBackSide, DpDrainType_e dpDrainType)
    {
        var swModelLevel2 = (ModelDoc2)swCompLevel2.GetModelDoc2();
        swModelLevel2.ChangeDim("Length@SketchBase", length);

        var leftSbDis = leftEndDis+1d;
        var rightSbDis = rightEndDis+1d;
        var leftLksDis = leftSbDis + 265d;
        var rightLksDis = rightSbDis + 265d;
        var leftGutterDis = leftSbDis + 535d;
        var rightGutterDis = rightSbDis + 535d;

        #region BCJ腔
        BcjSide(swCompLevel2, swModelLevel2, bcjSide, ref leftSbDis, ref rightSbDis);
        #endregion

        #region NOCJ腔
        NocjSide(swCompLevel2, swModelLevel2, nocjSide, width, ref leftSbDis, ref rightSbDis);
        #endregion

        #region NOCJ腔Back
        NocjBackSide(swCompLevel2, swModelLevel2, nocjBackSide, width);
        #endregion

        #region DP腔体
        DpSide(swCompLevel2, swModelLevel2, dpSide, ref leftSbDis, ref rightSbDis);
        #endregion

        #region DP腔体切除顶部
        CutDpSide(swCompLevel2, dpSide);
        #endregion

        #region DP腔体Back
        DpBackSide(swCompLevel2, dpBackSide);
        #endregion

        #region 排水管位置
        DpDrainType(swCompLevel2, dpDrainType);
        #endregion

        #region 左侧腔体
        LeftBeamType(swCompLevel2, swModelLevel2, leftBeamType, lksSide, length, leftDbToRight, leftSbDis, ref leftLksDis, ref leftGutterDis);
        #endregion

        #region 右侧腔体right
        RightBeamType(swCompLevel2, swModelLevel2, rightBeamType, lksSide, length, rightDbToLeft, rightSbDis, ref rightLksDis, ref rightGutterDis);
        #endregion

        #region LKS灯腔
        LksSide(swCompLevel2, swModelLevel2, lksSide, leftLksDis, rightLksDis);
        #endregion

        #region Ansul腔(Gutter)
        GutterSide(swCompLevel2, swModelLevel2, gutterSide, leftGutterDis, leftGutterWidth, rightGutterDis, rightGutterWidth);
        #endregion
    }

    #endregion

    #region LFU
    private void FNCA0001(Component2 swCompLevel2, double length, double width, SidePanel_e sidePanel, int supplySpigotNumber, double supplySpigotDis, double supplySpigotDia, bool japan)
    {
        var spigotToMiddle = supplySpigotDis * (supplySpigotNumber/2-1)+supplySpigotDis/2d;//脖颈口距离中间位置

        var swModelLevel2 = (ModelDoc2)swCompLevel2.GetModelDoc2();
        swModelLevel2.ChangeDim("Length@Base-Flange", length);
        swModelLevel2.ChangeDim("Width@SketchBase", width-2d);

        #region 均流桶
        if (supplySpigotNumber == 1)
        {
            swCompLevel2.Suppress("LPatternSpigot");
            swModelLevel2.ChangeDim("Dia@SketchSpigot", supplySpigotDia);
            swModelLevel2.ChangeDim("ToMiddle@SketchSpigot", 0);
        }
        else
        {
            swCompLevel2.UnSuppress("LPatternSpigot");
            swModelLevel2.ChangeDim("Dia@SketchSpigot", supplySpigotDia);
            swModelLevel2.ChangeDim("ToMiddle@SketchSpigot", spigotToMiddle);
            swModelLevel2.ChangeDim("Number@LPatternSpigot", supplySpigotNumber);
            swModelLevel2.ChangeDim("Dis@LPatternSpigot", supplySpigotDis);
        }
        #endregion

        #region 联接侧边切除
        switch (sidePanel)
        {
            case SidePanel_e.左:
                swCompLevel2.UnSuppress("CutLeft");
                swCompLevel2.Suppress("CutRight");
                break;
            case SidePanel_e.右:
                swCompLevel2.UnSuppress("CutRight");
                swCompLevel2.Suppress("CutLeft");
                break;
            case SidePanel_e.双:
                swCompLevel2.UnSuppress("CutLeft");
                swCompLevel2.UnSuppress("CutRight");
                break;
            case SidePanel_e.NA:
            case SidePanel_e.中:
            default:
                swCompLevel2.Suppress("CutLeft");
                swCompLevel2.Suppress("CutRight");
                break;
        }
        #endregion

        #region 日本不要吊装孔
        if (japan)
        {
            swCompLevel2.Suppress("LiftingHoles");
        }
        else
        {
            swCompLevel2.UnSuppress("LiftingHoles");
        }
        #endregion
    }

    private void FNCA0012(Component2 swCompLevel2, double length, int supplySpigotNumber, double supplySpigotDis, double supplySpigotDia)
    {
        var spigotToMiddle = supplySpigotDis * (supplySpigotNumber/2-1)+supplySpigotDis/2d;//脖颈口距离中间位置

        var swModelLevel2 = (ModelDoc2)swCompLevel2.GetModelDoc2();
        swModelLevel2.ChangeDim("Length@Base-Flange", length);

        #region 均流桶
        if (supplySpigotNumber == 1)
        {
            swCompLevel2.Suppress("LPatternSpigot");
            swModelLevel2.ChangeDim("Dia@SketchSpigot", supplySpigotDia);
            swModelLevel2.ChangeDim("ToMiddle@SketchSpigot", 0);
        }
        else
        {
            swCompLevel2.UnSuppress("LPatternSpigot");
            swModelLevel2.ChangeDim("Dia@SketchSpigot", supplySpigotDia);
            swModelLevel2.ChangeDim("ToMiddle@SketchSpigot", spigotToMiddle);
            swModelLevel2.ChangeDim("Number@LPatternSpigot", supplySpigotNumber);
            swModelLevel2.ChangeDim("Dis@LPatternSpigot", supplySpigotDis);
        }
        #endregion

    }

    private void FNCA0016(Component2 swCompLevel2, double length, double width, int supplySpigotNumber, double supplySpigotDis, double supplySpigotDia, bool japan)
    {
        var spigotToMiddle = supplySpigotDis * (supplySpigotNumber/2-1)+supplySpigotDis/2d;//脖颈口距离中间位置

        var swModelLevel2 = (ModelDoc2)swCompLevel2.GetModelDoc2();
        swModelLevel2.ChangeDim("Length@Base-Flange", length);
        swModelLevel2.ChangeDim("Width@SketchBase", width);

        #region 均流桶
        if (supplySpigotNumber == 1)
        {
            swCompLevel2.Suppress("LPatternSpigot");
            swModelLevel2.ChangeDim("Dia@SketchSpigot", supplySpigotDia);
            swModelLevel2.ChangeDim("ToMiddle@SketchSpigot", 0);
        }
        else
        {
            swCompLevel2.UnSuppress("LPatternSpigot");
            swModelLevel2.ChangeDim("Dia@SketchSpigot", supplySpigotDia);
            swModelLevel2.ChangeDim("ToMiddle@SketchSpigot", spigotToMiddle);
            swModelLevel2.ChangeDim("Number@LPatternSpigot", supplySpigotNumber);
            swModelLevel2.ChangeDim("Dis@LPatternSpigot", supplySpigotDis);
        }
        #endregion

        #region 日本不要吊装孔
        if (japan)
        {
            swCompLevel2.Suppress("LiftingHoles");
        }
        else
        {
            swCompLevel2.UnSuppress("LiftingHoles");
        }
        #endregion
    }


    #endregion

    #region An(Gutter)

    private void FNCE0025(Component2 swCompLevel2, double length, double width, bool marvel, bool ansul, int ansulDropNumber, double ansulDropToFront, double ansulDropDis1, double ansulDropDis2, double ansulDropDis3, double ansulDropDis4, double ansulDropDis5, int ansulDetectorNumber, AnsulDetectorEnd_e ansulDetectorEnd, double ansulDetectorDis1, double ansulDetectorDis2, double ansulDetectorDis3, double ansulDetectorDis4, double ansulDetectorDis5)
    {
        var swModelLevel2 = (ModelDoc2)swCompLevel2.GetModelDoc2();
        swModelLevel2.ChangeDim("Length@Base-Flange", length);
        swModelLevel2.ChangeDim("Width@SketchBase", width);


        #region Marvel
        if (marvel)
        {
            swCompLevel2.UnSuppress("Marvel");
        }
        else
        {
            swCompLevel2.Suppress("Marvel");
        }
        #endregion

        #region ANSUL选项
        swCompLevel2.Suppress("AnsulDrop1");
        swCompLevel2.Suppress("AnsulDrop2");
        swCompLevel2.Suppress("AnsulDrop3");
        swCompLevel2.Suppress("AnsulDrop4");
        swCompLevel2.Suppress("AnsulDrop5");
        //UW/KW HOOD 水洗烟罩探测器在midRoof上，非水洗烟罩压缩
        swCompLevel2.Suppress("AnsulDetector1");
        swCompLevel2.Suppress("AnsulDetector2");
        swCompLevel2.Suppress("AnsulDetector3");
        swCompLevel2.Suppress("AnsulDetector4");
        swCompLevel2.Suppress("AnsulDetector5");
        if (ansul)
        {
            #region Ansul下喷
            if (ansulDropNumber > 0)
            {
                swCompLevel2.UnSuppress("AnsulDrop1");
                swModelLevel2.ChangeDim("Dis@SketchAnsulDrop1", ansulDropDis1);
                swModelLevel2.ChangeDim("DisY@SketchAnsulDrop1", ansulDropToFront);
            }
            if (ansulDropNumber > 1)
            {
                swCompLevel2.UnSuppress("AnsulDrop2");
                swModelLevel2.ChangeDim("Dis@SketchAnsulDrop2", ansulDropDis2);
            }
            if (ansulDropNumber > 2)
            {
                swCompLevel2.UnSuppress("AnsulDrop3");
                swModelLevel2.ChangeDim("Dis@SketchAnsulDrop3", ansulDropDis3);
            }
            if (ansulDropNumber > 3)
            {
                swCompLevel2.UnSuppress("AnsulDrop4");
                swModelLevel2.ChangeDim("Dis@SketchAnsulDrop4", ansulDropDis4);
            }
            if (ansulDropNumber > 4)
            {
                swCompLevel2.UnSuppress("AnsulDrop5");
                swModelLevel2.ChangeDim("Dis@SketchAnsulDrop5", ansulDropDis5);
            }
            #endregion

            #region Ansul探测器，水洗烟罩需要探测器安装在MidRoof上

            //探测器
            if (ansulDetectorNumber> 0)
            {
                swCompLevel2.UnSuppress("AnsulDetector1");
                swModelLevel2.ChangeDim("Dis@SketchAnsulDetector1", ansulDetectorDis1);
                if (ansulDetectorEnd == AnsulDetectorEnd_e.左末端探测器|| (ansulDetectorEnd == AnsulDetectorEnd_e.右末端探测器 && ansulDetectorNumber == 1))
                    swModelLevel2.ChangeDim("Length@SketchAnsulDetector1", 195d);
                else swModelLevel2.ChangeDim("Length@SketchAnsulDetector1", 175d);
            }
            if (ansulDetectorNumber > 1)
            {
                swCompLevel2.UnSuppress("AnsulDetector2");
                swModelLevel2.ChangeDim("Dis@SketchAnsulDetector2", ansulDetectorDis2);
                if (ansulDetectorEnd == AnsulDetectorEnd_e.右末端探测器 && ansulDetectorNumber == 2)
                    swModelLevel2.ChangeDim("Length@SketchAnsulDetector2", 195d);
                else swModelLevel2.ChangeDim("Length@SketchAnsulDetector2", 175d);
            }
            if (ansulDetectorNumber > 2)
            {
                swCompLevel2.UnSuppress("AnsulDetector3");
                swModelLevel2.ChangeDim("Dis@SketchAnsulDetector3", ansulDetectorDis3);
                if (ansulDetectorEnd == AnsulDetectorEnd_e.右末端探测器 && ansulDetectorNumber == 3)
                    swModelLevel2.ChangeDim("Length@SketchAnsulDetector3", 195d);
                else swModelLevel2.ChangeDim("Length@SketchAnsulDetector3", 175d);
            }
            if (ansulDetectorNumber > 3)
            {
                swCompLevel2.UnSuppress("AnsulDetector4");
                swModelLevel2.ChangeDim("Dis@SketchAnsulDetector4", ansulDetectorDis4);
                if (ansulDetectorEnd == AnsulDetectorEnd_e.右末端探测器 && ansulDetectorNumber == 4)
                    swModelLevel2.ChangeDim("Length@SketchAnsulDetector4", 195d);
                else swModelLevel2.ChangeDim("Length@SketchAnsulDetector4", 175d);
            }
            if (ansulDetectorNumber > 4)
            {
                swCompLevel2.UnSuppress("AnsulDetector5");
                swModelLevel2.ChangeDim("Dis@SketchAnsulDetector5", ansulDetectorDis5);
                if (ansulDetectorEnd == AnsulDetectorEnd_e.右末端探测器 && ansulDetectorNumber == 5)
                    swModelLevel2.ChangeDim("Length@SketchAnsulDetector5", 195d);
                else swModelLevel2.ChangeDim("Length@SketchAnsulDetector5", 175d);
            }
            #endregion
        }
        #endregion

    }


    private void FNCE0001(AssemblyDoc swAssyLevel1, string suffix, string partName, double width, bool ansul)
    {
        var swCompLevel2 = swAssyLevel1.UnSuppress(out ModelDoc2 swModelLevel2, suffix, partName, Aggregator);
        swModelLevel2.ChangeDim("Width@SketchBase", width);
        #region Marvel
        if (ansul)
        {
            swCompLevel2.UnSuppress("AnsulDetector");
        }
        else
        {
            swCompLevel2.Suppress("AnsulDetector");
        }
        #endregion
    }
    #endregion

    #region SSP



    #endregion

    #endregion
}