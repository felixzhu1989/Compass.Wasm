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
            cjNumber--;
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
        var swCompLevel2 = swAssyTop.RenameComp(suffix, "CJ300", module, "FNCJ0020", 1, data.Length, data.Width, Aggregator);
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
            cjNumber--;
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
        var swCompLevel2 = swAssyTop.RenameComp(suffix, "CJ330", module, "FNCJ0023", 1, data.Length, data.Width, Aggregator);
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
            cjNumber--;
            firstCjDis += 15d;
        }

        //CJ脖颈
        CjSpigot(swAssyTop, suffix, data.CjSpigotDirection, "FNCJ0010-1", "5201990413-1");
        //侧板
        CjSidePanel(swAssyTop, suffix, data.SidePanel, "FNCJ0003-1", "FNCJ0014-1", "FNCJ0004-1", "FNCJ0014-2");

        //其余零件
        swAssyTop.ChangePartLength(suffix, "FNCJ0016-1", "Length@SketchBase", data.Length-10d, Aggregator);


        //重命名BCJ腔体
        var swCompLevel2 = swAssyTop.RenameComp(suffix, "BCJ300", module, "FNCJ0015", 1, data.Length, data.Width, Aggregator);
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
            cjNumber--;
            firstCjDis += 15d;
        }

        //CJ脖颈
        CjSpigot(swAssyTop, suffix, data.CjSpigotDirection, "FNCJ0010-1", "5201990413-1");
        //侧板
        CjSidePanel(swAssyTop, suffix, data.SidePanel, "FNCJ0042-1", "FNCJ0044-1", "FNCJ0043-1", "FNCJ0044-2");

        //其余零件
        swAssyTop.ChangePartLength(suffix, "FNCJ0016-1", "Length@SketchBase", data.Length-10d, Aggregator);


        //重命名BCJ腔体
        var swCompLevel2 = swAssyTop.RenameComp(suffix, "BCJ330", module, "FNCJ0045", 1, data.Length, data.Width, Aggregator);
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
        var swCompLevel2 = swAssyTop.RenameComp(suffix, "NOCJ300", module, "FNCJ0001", 1, data.Length, data.Width, Aggregator);
        if (swCompLevel2 != null)
        {
            FNCJ0001(swCompLevel2, data.Length,data.Width, data.BcjSide, data.LeftEndDis, data.RightEndDis, data.LeftBeamType, data.LeftDbToRight, data.RightBeamType, data.RightDbToLeft, data.LksSide, data.GutterSide, data.LeftGutterWidth, data.RightGutterWidth,data.NocjSide,data.NocjBackSide);
        }
    }

    public void Nocj330(ModelDoc2 swModelTop, AssemblyDoc swAssyTop, string suffix, string module, CjData data)
    {
        //侧板
        NocjSidePanel(swAssyTop, suffix, data.SidePanel, data.NocjBackSide, data.Width, "FNCJ0055-1", "FNCJ0054-1", "FNCJ0056-1", "FNCJ0054-2");

        //重命名BCJ腔体
        var swCompLevel2 = swAssyTop.RenameComp(suffix, "NOCJ330", module, "FNCJ0051", 1, data.Length, data.Width, Aggregator);
        if (swCompLevel2 != null)
        {
            FNCJ0001(swCompLevel2, data.Length, data.Width, data.BcjSide, data.LeftEndDis, data.RightEndDis, data.LeftBeamType, data.LeftDbToRight, data.RightBeamType, data.RightDbToLeft, data.LksSide, data.GutterSide, data.LeftGutterWidth, data.RightGutterWidth, data.NocjSide, data.NocjBackSide);
        }
    }

    public void Nocj340(ModelDoc2 swModelTop, AssemblyDoc swAssyTop, string suffix, string module, CjData data)
    {
        //侧板
        Nocj340SidePanel(swAssyTop, suffix, data.SidePanel,  data.Width, "FNCS0017-1", "FNCS0020-1", "FNCS0018-1", "FNCS0020-2");

        //重命名BCJ腔体
        var swCompLevel2 = swAssyTop.RenameComp(suffix, "NOCJ340", module, "FNCS0016", 1, data.Length, data.Width, Aggregator);
        if (swCompLevel2 != null)
        {
            FNCS0016(swCompLevel2, data.Length, data.Width, data.BcjSide, data.LeftEndDis, data.RightEndDis, data.LeftBeamType, data.LeftDbToRight, data.RightBeamType, data.RightDbToLeft, data.LksSide, data.GutterSide, data.LeftGutterWidth, data.RightGutterWidth, data.NocjSide, data.NocjBackSide,data.DpSide);
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
        var swCompLevel2 = swAssyTop.RenameComp(suffix, "DP330", module, "FNCS0001", 1, data.Length, data.Width, Aggregator);
        if (swCompLevel2 != null)
        {
            FNCS0001(swCompLevel2, data.Length, data.Width, data.BcjSide, data.LeftEndDis, data.RightEndDis, data.LeftBeamType, data.LeftDbToRight, data.RightBeamType, data.RightDbToLeft, data.LksSide, data.GutterSide, data.LeftGutterWidth, data.RightGutterWidth, data.NocjSide, data.NocjBackSide,data.DpSide,data.DpBackSide,data.DpDrainType);
        }
    }


    #endregion


    #region 通用方法

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

        void BlindPanel(bool back,string compName)
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

    private void Nocj340SidePanel(AssemblyDoc swAssyLevel1, string suffix, SidePanel_e sidePanel,double width, string leftBlindPart, string leftHolePart, string rightBlindPart, string rightHolePart)
    {
        switch (sidePanel)
        {
            case SidePanel_e.左:
                {
                    BlindPanel( leftBlindPart);
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


    private void DpSidePanel(AssemblyDoc swAssyLevel1, string suffix, SidePanel_e sidePanel,string leftBlindPart, string leftHolePart,string leftSealingPart, string rightBlindPart, string rightHolePart, string rightSealingPart)
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

        WaterproofSealing(swAssyLevel1,suffix,sidePanel,leftSealingPart,rightSealingPart);
    }

    private void BcjSide(Component2 swCompLevel2,ModelDoc2 swModelLevel2,BcjSide_e bcjSide,ref double leftSbDis,ref double rightSbDis)
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

    private void LeftBeamType(Component2 swCompLevel2, ModelDoc2 swModelLevel2,BeamType_e leftBeamType,LksSide_e lksSide,double length,double leftDbToRight,double leftSbDis,ref double leftLksDis,ref double leftGutterDis)
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
                swModelLevel2.ChangeDim("Dis@SketchKcjSb265Left", leftSbDis);
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
                rightGutterDis =rightLksDis+ (lksSide is LksSide_e.左LK灯腔 or LksSide_e.两LK灯腔? 270d : 0d);
                break;
            case BeamType_e.KCJSB265:
                swCompLevel2.UnSuppress("KcjSb265Right");
                swModelLevel2.ChangeDim("Dis@SketchKcjSb265Right", rightSbDis);
                rightLksDis = rightSbDis + 265d;
                rightGutterDis =rightLksDis+ (lksSide is LksSide_e.左LK灯腔 or LksSide_e.两LK灯腔? 270d : 0d);
                break;
            case BeamType_e.UCJSB385:
                swCompLevel2.UnSuppress("UcjSb385Right");
                swModelLevel2.ChangeDim("Dis@SketchUcjSb385Right", rightSbDis);
                rightLksDis = rightSbDis + 385d;
                rightGutterDis =rightLksDis+ (lksSide is LksSide_e.左LK灯腔 or LksSide_e.两LK灯腔? 270d : 0d);
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
                swModelLevel2.ChangeDim("Dis@SketchKcjSb265Right", rightSbDis);
                rightLksDis = rightSbDis + 265d;
                rightGutterDis =rightLksDis+ (lksSide is LksSide_e.左LK灯腔 or LksSide_e.两LK灯腔? 270d : 0d);
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

    private void GutterSide(Component2 swCompLevel2, ModelDoc2 swModelLevel2, GutterSide_e gutterSide, double leftGutterDis,double leftGutterWidth, double rightGutterDis,double rightGutterWidth)
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

    private void NocjSide(Component2 swCompLevel2, ModelDoc2 swModelLevel2, NocjSide_e nocjSide,double width, ref double leftSbDis, ref double rightSbDis)
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

    private void CutDpSide(Component2 swCompLevel2, ModelDoc2 swModelLevel2, DpSide_e dpSide, double width, ref double leftSbDis, ref double rightSbDis)
    {
        switch (dpSide)
        {
            case DpSide_e.左DP腔:
                swCompLevel2.UnSuppress("CutDpLeft");
                
                leftSbDis += width;
                swCompLevel2.Suppress("CutDpRight");
                break;
            case DpSide_e.右DP腔:
                swCompLevel2.UnSuppress("CutDpRight");
                
                rightSbDis += width;
                swCompLevel2.Suppress("CutDpLeft");
                break;
            case DpSide_e.两DP腔:
                swCompLevel2.UnSuppress("CutDpLeft");
                
                leftSbDis += width;
                swCompLevel2.UnSuppress("CutDpRight");
                
                rightSbDis += width;
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

    private void DpDrainType(Component2 swCompLevel2,DpDrainType_e dpDrainType)
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
        BcjSide(swCompLevel2, swModelLevel2, bcjSide,ref leftSbDis,ref rightSbDis);
        #endregion

        #region 左侧腔体
        LeftBeamType(swCompLevel2, swModelLevel2,leftBeamType,lksSide,length,leftDbToRight,leftSbDis,ref leftLksDis,ref leftGutterDis);
        #endregion

        #region 右侧腔体right
        RightBeamType(swCompLevel2, swModelLevel2, rightBeamType, lksSide, length, rightDbToLeft, rightSbDis, ref rightLksDis, ref rightGutterDis);
        #endregion

        #region LKS灯腔
        LksSide(swCompLevel2,swModelLevel2,lksSide,leftLksDis,rightLksDis);
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


    private void FNCJ0001(Component2 swCompLevel2, double length,double width, BcjSide_e bcjSide, double leftEndDis, double rightEndDis, BeamType_e leftBeamType, double leftDbToRight, BeamType_e rightBeamType, double rightDbToLeft, LksSide_e lksSide, GutterSide_e gutterSide, double leftGutterWidth, double rightGutterWidth,NocjSide_e nocjSide,NocjBackSide_e nocjBackSide)
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
        NocjSide(swCompLevel2, swModelLevel2, nocjSide,width, ref leftSbDis, ref rightSbDis);
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
    private void FNCS0016(Component2 swCompLevel2, double length, double width, BcjSide_e bcjSide, double leftEndDis, double rightEndDis, BeamType_e leftBeamType, double leftDbToRight, BeamType_e rightBeamType, double rightDbToLeft, LksSide_e lksSide, GutterSide_e gutterSide, double leftGutterWidth, double rightGutterWidth, NocjSide_e nocjSide, NocjBackSide_e nocjBackSide,DpSide_e dpSide)
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

        //todo:FNCS0016，NOCJ的三角板需要修改
        #region NOCJ腔
        NocjSide(swCompLevel2, swModelLevel2, nocjSide, width, ref leftSbDis, ref rightSbDis);
        #endregion

        #region NOCJ腔Back
        NocjBackSide(swCompLevel2, swModelLevel2, nocjBackSide, width);
        #endregion

        #region DP腔(NOCJ340特有)，DP和NOCJ不需要拧螺丝
        CutDpSide(swCompLevel2,swModelLevel2,dpSide,width,ref leftSbDis,ref rightSbDis);
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
        
        DpBackSide(swCompLevel2,dpBackSide);
    }
    private void FNCS0001(Component2 swCompLevel2, double length, double width, BcjSide_e bcjSide, double leftEndDis, double rightEndDis, BeamType_e leftBeamType, double leftDbToRight, BeamType_e rightBeamType, double rightDbToLeft, LksSide_e lksSide, GutterSide_e gutterSide, double leftGutterWidth, double rightGutterWidth, NocjSide_e nocjSide, NocjBackSide_e nocjBackSide,DpSide_e dpSide,DpBackSide_e dpBackSide,DpDrainType_e dpDrainType)
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
        DpSide(swCompLevel2,swModelLevel2,dpSide,ref leftSbDis,ref rightSbDis);
        #endregion

        #region DP腔体切除顶部
        CutDpSide(swCompLevel2, swModelLevel2, dpSide, width, ref leftSbDis, ref rightSbDis);
        #endregion

        #region DP腔体Back
        DpBackSide(swCompLevel2,dpBackSide);
        #endregion

        #region 排水管位置
        DpDrainType(swCompLevel2,dpDrainType);
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


    #endregion
}