﻿using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;

namespace Compass.Wpf.SwServices;

public static class SwUtility
{
    #region ConnectSw
    private static ISldWorks? _swApp;
    private const string ProgId = "SldWorks.Application";//SldWorks.Application.31代表SolidWorks2023
    //将注册表，修改为当前的CLSID
    //计算机\HKEY_CLASSES_ROOT\SldWorks.Application\CLSID（主要修改这个就可以了）
    //计算机\HKEY_LOCAL_MACHINE\SOFTWARE\Classes\SldWorks.Application\CLSID
    //2021：{0A5BC87B-FDBF-4A0B-BC8A-DE1271220E81}
    //2023：{b2f1524f-6cfe-4386-b472-ab1148dea4f1}
    public static ISldWorks ConnectSw(IEventAggregator aggregator)
    {
        if (_swApp != null) return _swApp;
        try
        {
            //尝试连接已打开的Solidworks
            _swApp = (SldWorks)GetActiveObject(ProgId);
        }
        catch
        {
            //如果没有事先打开SW，那么打开你上一次使用的SW
            var swType = Type.GetTypeFromProgID(ProgId);
            _swApp=(ISldWorks)Activator.CreateInstance(swType!)!;
        }
        _swApp.Visible=true;
        var swRev = Convert.ToInt32(_swApp.RevisionNumber()[..2]) - 8;
        Debug.Print($"SolidWorks 20{swRev}");
        aggregator.SendMessage($"连接到 SolidWorks 20{swRev}...OK...", Filter_e.Batch);
        return _swApp;
    }

    [DllImport("oleaut32.dll", PreserveSig = false)]
    private static extern void GetActiveObject(
        ref Guid rclsid,
        IntPtr pvReserved,
        [MarshalAs(UnmanagedType.IUnknown)] out Object ppunk
    );

    [DllImport("ole32.dll")]
    private static extern int CLSIDFromProgID(
        [MarshalAs(UnmanagedType.LPWStr)] string lpszProgID,
        out Guid pclsid
    );

    private static object GetActiveObject(string progId)
    {
        Guid clsid;
        CLSIDFromProgID(progId, out clsid);

        object obj;
        GetActiveObject(ref clsid, IntPtr.Zero, out obj);

        return obj;
    }

    #endregion

    #region 转换单位

    #endregion

    #region ISldWorks扩展
    /// <summary>
    /// 获取pack后的装配体地址
    /// </summary>
    public static string GetPackPath(this ModuleDto moduleDto, out string suffix,out string packDir)
    {
        //packandgo文件夹位置
        packDir = Path.Combine(@"D:\MyProjects", moduleDto.OdpNumber,moduleDto.ItemNumber, $"{moduleDto.Name}_{moduleDto.ModelName}");

        suffix = $"_{moduleDto.ItemNumber}_{moduleDto.Name}_{moduleDto.OdpNumber.Substring(moduleDto.OdpNumber.Length-6)}";
        //packandgo装配体位置
        var packPath = Path.Combine(packDir, $"{moduleDto.ModelName}{suffix}.SLDASM");
        return packPath;
    }

    /// <summary>
    /// 打包模型到项目，根据模型地址和分段ModuleDto，抛出后缀
    /// </summary>
    public static string PackToProject(this ISldWorks? swApp, out string suffix, string modelPath, ModuleDto moduleDto, IEventAggregator aggregator)
    {
        //获取pack后的装配体地址
        var packPath= moduleDto.GetPackPath(out suffix,out string packDir);
        //判断pack后的装配体是否存在，存在就直接打开，否则先执行pack
        if (!File.Exists(packPath))
        {
            aggregator.SendMessage($"打包总装:\t{packDir}", Filter_e.Batch);
            aggregator.SendMessage("正在PackAndGo...请稍候...", Filter_e.Batch);
            swApp.PackAndGo(modelPath, packDir, suffix);
            aggregator.SendMessage("PackAndGo完成...", Filter_e.Batch);
        }
        else
        {
            aggregator.SendMessage($"文件存在:\t{packDir}", Filter_e.Batch);
        }
        return packPath;
    }

    /// <summary>
    /// 打开装配体（通常用于最顶层装配体），抛出swModel
    /// </summary>
    public static AssemblyDoc OpenAssemblyDoc(this ISldWorks? swApp, out ModelDoc2 swModel, string fileName,IEventAggregator aggregator)
    {
        int errors = 0;
        int warnings = 0;
        swModel = swApp.OpenDoc6(fileName,
            (int)swDocumentTypes_e.swDocASSEMBLY,
            (int)swOpenDocOptions_e.swOpenDocOptions_Silent,
            "", ref errors, ref warnings);
        //todo:变更发送消息方式
        aggregator.SendMessage($"打开总装:\t{swModel.GetPathName()}",Filter_e.Batch);
        //打开装配体后必须重建，使Pack后的零件名都更新到带后缀的状态，否则程序出错
        swModel.ForceRebuild3(true); //TopOnly参数设置成true，只重建顶层，不重建零件内部
        return (swModel as AssemblyDoc)!;
    }

    #endregion
    
    #region 绘图代码扩展方法

    #region 文件名添加后缀
    /// <summary>
    /// 根据文件名和后缀组合
    /// </summary>
    public static string AddSuffix(this string partName, string suffix)
    {
        //从-号拆分，-前添加suffix，例如：FNHE0001-1 -> FNHE0001_Item-M1-210203-1 其中（_Item-M1-210203）是suffix
        var endIndex = partName.LastIndexOf("-", StringComparison.Ordinal);
        return $"{partName.Substring(0, endIndex)}{suffix}{partName.Substring(endIndex)}";
    }
    #endregion
    
    #region AssemblyDoc扩展
    /// <summary>
    /// 获取子装配
    /// </summary>
    public static AssemblyDoc GetSubAssemblyDoc(this AssemblyDoc swAssy, string suffix, string assyName, IEventAggregator aggregator)
    {
        swAssy.UnSuppress(out ModelDoc2 swModelLevel1, suffix, assyName, aggregator);
        aggregator.SendMessage($"处理装配:\t{swModelLevel1.GetPathName()}",Filter_e.Batch);
        return (swModelLevel1 as AssemblyDoc)!;
    }
    /// <summary>
    /// 获取子装配并抛出ModelDoc2
    /// </summary>
    public static AssemblyDoc GetSubAssemblyDoc(this AssemblyDoc swAssy, out ModelDoc2 swModelLevel1, string suffix, string assyName, IEventAggregator aggregator)
    {
        swAssy.UnSuppress(out swModelLevel1, suffix, assyName, aggregator);
        return (swModelLevel1 as AssemblyDoc)!;
    }
    
    /// <summary>
    /// AssemblyDoc更改尺寸，int数量
    /// </summary>
    public static void ChangeDim(this AssemblyDoc swAssy, string dimName, int intValue)
    {
        var swModel = swAssy as ModelDoc2;
        var dim = (IDimension)swModel.Parameter(dimName);
        dim.SystemValue=intValue;
    }

    /// <summary>
    /// AssemblyDoc更改尺寸，double距离,已除1000
    /// </summary>
    public static void ChangeDim(this AssemblyDoc swAssy, string dimName, double dblValue)
    {
        var swModel = swAssy as ModelDoc2;
        var dim = (IDimension)swModel.Parameter(dimName);
        dim.SystemValue=dblValue / 1000d;
    }

    /// <summary>
    /// 装配体压缩特征
    /// </summary>
    public static void Suppress(this AssemblyDoc swAssy, string featureName)
    {
        var feat = (IFeature)swAssy.FeatureByName(featureName);
        feat.SetSuppression2(0, 2, null);
    }
    /// <summary>
    /// 装配体解压特征，重建
    /// </summary>
    public static void UnSuppress(this AssemblyDoc swAssy, string featureName)
    {
        var feat = (IFeature)swAssy.FeatureByName(featureName);
        feat.SetSuppression2(1, 2, null);
        swAssy.ForceRebuild2(true);
    }


    /// <summary>
    /// 装配体压缩部件
    /// </summary>
    public static void Suppress(this AssemblyDoc swAssy, string suffix, string compName)
    {
        swAssy.GetComponentByName(compName.AddSuffix(suffix)).SetSuppression2(0);
    }

    /// <summary>
    /// 装配体解压部件，解压零部件根方法，解压后对装配体进行重建
    /// </summary>
    public static Component2 UnSuppress(this AssemblyDoc swAssy, string suffix, string compName, IEventAggregator aggregator)
    {
        var swComp = swAssy.GetComponentByName(compName.AddSuffix(suffix));
        swComp.SetSuppression2(2);
        aggregator.SendMessage($"处理零件:\t{swComp.GetPathName()}",Filter_e.Batch);
        //如果二级装配中的零件被压缩，那么或获取不到零件，因此建议先解压零件,重建装配体，再操作
        swAssy.ForceRebuild2(true);
        return swComp;
    }

    /// <summary>
    /// 装配体解压部件，抛出ModelDoc2
    /// </summary>
    public static Component2 UnSuppress(this AssemblyDoc swAssy, out ModelDoc2 swModel, string suffix, string compName, IEventAggregator aggregator)
    {
        var swComp = swAssy.UnSuppress(suffix, compName,aggregator);
        swModel=swComp.GetModelDoc2() as ModelDoc2;
        return swComp;
    }
    #endregion

    #region ModelDoc2扩展
    /// <summary>
    /// ModelDoc2更改尺寸，int数量
    /// </summary>
    public static void ChangeDim(this ModelDoc2 swModel, string dimName, int intValue)
    {
        var dim = (IDimension)swModel.Parameter(dimName);
        dim.SystemValue=intValue;
    }

    /// <summary>
    /// 更改尺寸，double距离,已除1000
    /// </summary>
    public static void ChangeDim(this ModelDoc2 swModel, string dimName, double dblValue)
    {
        var dim = (IDimension)swModel.Parameter(dimName);
        dim.SystemValue=dblValue / 1000d;
    }
    #endregion

    #region Component2扩展
    /// <summary>
    /// 部件压缩特征
    /// </summary>
    public static void Suppress(this Component2 swComp, string featureName)
    {
        swComp.FeatureByName(featureName).SetSuppression2(0, 2, null);
    }

    /// <summary>
    /// 部件解压特征
    /// </summary>
    public static void UnSuppress(this Component2 swComp, string featureName)
    {
        swComp.FeatureByName(featureName).SetSuppression2(1, 2, null);
        var swModel = swComp.GetModelDoc2()as ModelDoc2;
        swModel.ForceRebuild3(true);//进行重建
    } 
    #endregion
    
    #endregion
    
    #region packango
    public static void PackAndGo(this ISldWorks? swApp, string modelPath, string packDir, string suffix)
    {
        try
        {
            if (!File.Exists(modelPath))
            {
                throw new FileNotFoundException();
            }
            int errors = 0;
            int warnings = 0;
            //打开需要pack的模型
            var swModel = swApp.OpenDoc6(modelPath,
                (int)swDocumentTypes_e.swDocASSEMBLY, (int)
                swOpenDocOptions_e.swOpenDocOptions_Silent,
                "", ref errors, ref warnings);
            swModel.ForceRebuild3(true);
            var swModelExt = swModel.Extension;
            var swPackAndGo = swModelExt.GetPackAndGo();
            swPackAndGo.IncludeDrawings = false;
            swPackAndGo.IncludeSimulationResults = false;
            swPackAndGo.IncludeToolboxComponents = false;
            swPackAndGo.IncludeSuppressed = true;

            // Set folder where to save the files,目标存放文件夹
            //判断打包目标文件夹是否存在，不存在则创建
            if (!Directory.Exists(packDir)) Directory.CreateDirectory(packDir);
            swPackAndGo.SetSaveToName(true, packDir);
            swPackAndGo.FlattenToSingleFolder = true;
            swPackAndGo.AddSuffix = suffix;

            // 执行Pack and Go
            swModelExt.SavePackAndGo(swPackAndGo);
            swApp.CloseDoc(modelPath);
        }
        catch
        {
            swApp.CloseDoc(modelPath);
            swApp.CommandInProgress = false;
            throw;
        }
    }
    #endregion




}