namespace Compass.Test;

public class CodeBackUp
{
    #region 读取CSV文件乱码的处理
    ////https://joshclose.github.io/CsvHelper/getting-started/
    ////Install-Package CsvHelper
    /*var path = @"C:\Users\Administrator\Desktop\MainPlanUpdate.csv";
    var mainplans = new List<MainPlanCsv>();
    //Excel默认以ANSI格式保存csv文件，读取的时候中文会乱码，应当指定Encoding为GB2312
    Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
    using (var reader = new StreamReader(path, Encoding.GetEncoding("GB2312")))
    using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
    {
        mainplans= csv.GetRecords<MainPlanCsv>().ToList();
    }*/
    //foreach (var mainplan in mainplans)
    //{
    //    Console.WriteLine(mainplan.Name);
    //} 
    #endregion

    #region SolidWorks安装新版本后CLSID变化
    //计算机\HKEY_CLASSES_ROOT\SldWorks.Application\CLSID（主要修改这个就可以了）
    //2023：{b2f1524f-6cfe-4386-b472-ab1148dea4f1}
    /*var rgrt = Registry.ClassesRoot;
    var clsid = rgrt.OpenSubKey(@"SldWorks.Application\CLSID", true);
    var names = clsid.GetValueNames();
    foreach (var name in names)
    {
        Console.WriteLine(name);
    }
    //Console.WriteLine(clsid.GetValue("(默认)").ToString());
    rgrt.Close();
    clsid.Close();*/
    #endregion

    #region 用一个数组保存另一个数组的排序
    //用一个数组保存另一个数组的排序
    /*var score = new double[] { 40, 13, 89, 52, 7 };
    var rank = new int[] { 1, 1, 1, 1, 1 };
    for (int i = 0; i < score.Length; i++)
    {
        for (int j = 0; j < score.Length; j++)
        {
            if (score[i] < score[j]) rank[i]++;
        }
    }

    foreach (var i in rank)
    {
        Console.WriteLine(i);
    }*/
    #endregion

    #region 操作属性
    /*var swConfig = (Configuration)swModel.GetActiveConfiguration();
    var swPropMgr = swConfig.CustomPropertyManager;
    //写入属性
    swPropMgr.Add3("BendingMark", (int)swCustomInfoType_e.swCustomInfoText, "123", (int)swCustomPropertyAddOption_e.swCustomPropertyDeleteAndAdd);
    //读取属性
    swPropMgr.Get6("BendingMark", false, out _, out var valout, out _, out _);
    Debug.Print(valout);*/
    #endregion

    #region 参考引用
    //var swModelExt = swModel.Extension;
    //var files = (IEnumerable)swModelExt.GetDependencies(true, false, true, true, true);
    #endregion

    #region 测试打包
    //var modelPath = @"D:\halton\01 Tech Dept\05 Products Library\01 Ceiling\UCW_DB_800.SLDASM";
    //var modelPath = @"D:\halton\01 Tech Dept\05 Products Library\01 Ceiling\SidePanel_KCW_DB_800.SLDASM";
    //var modelPath = @"D:\halton\01 Tech Dept\05 Products Library\01 Ceiling\SidePanel_KCW_DB_800.SLDASM";
    //var packDir = @"D:\MyProjects\FSO230000\Test\B2.2_UCW_DB_800";
    //swApp.PackAndGo(modelPath, packDir, "_B2.2");
    #endregion

    #region 测试一个镜像点返回值
    /*//就是通过查找具有对称关系，坐标与输入点不同的点就是要输出的点
    //这个函数返回的指定草图点的镜像点，返回的点对象就是com_object。
    //感觉solidworks api中的陷阱不少。就像上面的这个函数，返回的镜像点，一般没啥问题。
    //但在装配体中会出现这个镜像点无法被选中，没法添加尺寸的问题。
    var swModel = (ModelDoc2)swApp.ActiveDoc;
    var swSelMgr = (SelectionMgr)swModel.SelectionManager;
    //25,swSelEXTSKETCHPOINTS
    //var type = swSelMgr.GetSelectedObjectType3(1, -1);
    var swSketchPoint = (SketchPoint)swSelMgr.GetSelectedObject6(1,-1);
    var swMirPoint=getMirrorPoint(swSketchPoint);
    Debug.Print(swMirPoint.X.ToString());
    swMirPoint.Select2(false,0);

    SketchPoint? getMirrorPoint(SketchPoint sketchPoint)
    {
        var diff = 0.0000001d;
        SketchPoint? swSkPt = null;
        object[] vRelation = (object[])sketchPoint.GetRelations();
        foreach (object vSkRel in vRelation)
        {
            SketchRelation swSkRel = (SketchRelation)vSkRel;
            int type = swSkRel.GetRelationType();
            if (type == (int)swConstraintType_e.swConstraintType_SYMMETRIC)
            {
                int[] vEntTypeArr = (int[])swSkRel.GetEntitiesType();
                object[] vEntArr = (object[])swSkRel.GetEntities();
                object[] vDefEntArr = (object[])swSkRel.GetDefinitionEntities2();
                if (vEntTypeArr.GetUpperBound(0) == vEntArr.GetUpperBound(0))
                {
                    int j = 0;
                    foreach (swSketchRelationEntityTypes_e vType in vEntTypeArr)
                    {
                        if (vType == swSketchRelationEntityTypes_e.swSketchRelationEntityType_Point)
                        {
                            SketchPoint sketchPoint1 = (SketchPoint)vEntArr[j];
                            if (Math.Abs(sketchPoint1.X - sketchPoint.X) > diff || Math.Abs(sketchPoint1.Y - sketchPoint.Y) > diff)
                            {
                                swSkPt = sketchPoint1;
                                break;
                            }
                        }
                        j++;
                    }
                }
            }
        }
        return swSkPt;
    }*/
    #endregion

    #region 测试截取字符串
    //var oldStr = "FNCL0007[LPZ-LP01]{590}(500)";
    //var newStr = oldStr.Substring(oldStr.IndexOf('[') + 1, oldStr.IndexOf(']')-oldStr.IndexOf('[')-1);
    //Console.WriteLine(newStr);
    #endregion
}