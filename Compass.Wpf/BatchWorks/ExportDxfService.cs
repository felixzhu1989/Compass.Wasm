using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using SolidWorks.Interop.sldworks;
using System.IO;
using System.Diagnostics;
using SolidWorks.Interop.swconst;
using Compass.Wpf.ApiServices.Projects;
using Compass.Wpf.SwServices;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using DocumentFormat.OpenXml.Wordprocessing;

namespace Compass.Wpf.BatchWorks;

public interface IExportDxfService
{
    Task ExportHoodDxfAsync(ModuleDto moduleDto);
}


public class ExportDxfService : IExportDxfService
{
    #region ctor
    private readonly ICutListService _cutListService;
    private readonly IModuleService _moduleService;
    private readonly ISldWorks _swApp;
    private readonly IEventAggregator _aggregator;
    public ExportDxfService(IContainerProvider provider)
    {
        _cutListService = provider.Resolve<ICutListService>();
        _moduleService = provider.Resolve<IModuleService>();
        _aggregator= provider.Resolve<IEventAggregator>();
        _swApp = SwUtility.ConnectSw(_aggregator);
    } 
    #endregion

    public async Task ExportHoodDxfAsync(ModuleDto moduleDto)
    {
        //获取pack后的装配体地址
        var packPath = moduleDto.GetPackPath(out string suffix, out string packDir);
        //判断装配体是否存在，不存在抛出异常，提醒用户
        if (!File.Exists(packPath))
        {
            var psi = new ProcessStartInfo("Explorer.exe") { Arguments =packDir };
            Process.Start(psi);
            throw new FileNotFoundException("PackAndGo后的文件未找到，请检查该分段是否已经完成作图", packPath);
        }
        //优化进程外调用命令变缓慢的问题，不知是否能够解决导图慢的问题（貌似没什么用）
        _swApp.CommandInProgress = true;
        //打开装配体
        var swAssy = _swApp.OpenAssemblyDoc(out ModelDoc2 swModel, packPath,_aggregator);
        List<CutListDto> dtos = new List<CutListDto>();
        var comps = swAssy.GetComponents(false);
        foreach (var comp in (IEnumerable)comps)
        {
            var swComp = comp as Component2;
            //Debug.Print(swComp.GetPathName());//查看所有状态OK的零件文件
            //获取下料清单,尽早判断是否存在，存在就数量+1，跳出去
            var partName = Path.GetFileNameWithoutExtension(swComp.GetPathName()); ;
            var existDto = dtos.FirstOrDefault(x => x.PartNo.Equals(partName, StringComparison.OrdinalIgnoreCase));
            if (existDto!=null)
            {
                existDto.Quantity++;//数量+1
                continue;//继续循环下一个零件
            }

            //Debug.Print(swComp.GetPathName());//查看所有文件
            var swCompModel = swComp.GetModelDoc2() as ModelDoc2;

            //检查零部件是否为零件，并且检查状态，并且是钣金
            if (swCompModel!=null &&
                swCompModel.GetType()==(int)swDocumentTypes_e.swDocPART &&
                CheckPartStatus(swComp) &&
                CheckSheetMetal(swComp))
            {
                //如果是则增加下料清单信息
                var swFeat = swCompModel.FirstFeature() as Feature;
                //获取下料清单信息
                var dto = GetCutListDto(swCompModel,swFeat);
                if (dto == null) continue;

                dto.ModuleId = moduleDto.Id.Value;//分段的Id
                dto.PartDescription = swCompModel.CustomInfo2["", "Part Name"];//描述使用Part Name
                dto.PartNo = partName;//文件名
                dto.FilePath = swCompModel.GetPathName();

                dtos.Add(dto);
            }
        }
        //关闭总装
        _swApp.CloseDoc(swModel.GetPathName());

        //再逐个打开导出dxf图
        foreach (var dto in dtos.Where(dto => !ExportDxf(dto.FilePath,dto.PartNo, moduleDto)))
        {
            throw new Exception($"零件【{dto.PartNo}】导出dxf时发生错误！");
        }
        _swApp.CommandInProgress=false;

        //所有图纸导出完成后
        foreach (var dto in dtos)
        {
            //CutList提交数据库
            await _cutListService.AddAsync(dto);
        }
        //更新Module的IsCutListOk
        moduleDto.IsCutListOk= true;
        await _moduleService.UpdateAsync(moduleDto.Id.Value, moduleDto);
    }

    #region 内部实现
    /// <summary>
    /// 检查零件状态，压缩，封套和虚拟零部件
    /// </summary>
    /// <param name="swComp"></param>
    /// <returns></returns>
    private bool CheckPartStatus(Component2 swComp)
    {
        var status = true;
        //检查三种状态，压缩，封套和虚拟零部件
        if (swComp.IsSuppressed() || swComp.IsEnvelope() || swComp.IsVirtual)
        {
            return false;
        }
        //递归检查零部件的父零部件的窗台
        var parent = swComp.GetParent();
        if (parent != null)
        {
            status = status && CheckPartStatus(parent);
        }
        return status;
    }
    /// <summary>
    /// 检查零件是否包含钣金实体
    /// </summary>
    /// <param name="swComp"></param>
    /// <returns></returns>
    private bool CheckSheetMetal(Component2 swComp)
    {
        var bodies = swComp.GetBodies3((int)swBodyType_e.swSolidBody, out _);
        foreach (var body in (IEnumerable)bodies)
        {
            var swBody = body as Body2;
            if (swBody.IsSheetMetal())
            {
                return true;
            }
        }
        return false;
    }
    /// <summary>
    /// 获取Cutlist信息
    /// </summary>
    /// <param name="swFeat"></param>
    /// <returns></returns>
    private CutListDto? GetCutListDto(ModelDoc2 swModel,Feature swFeat)
    {
        if (!swFeat.IIsSuppressed2(1, 1, null)
            && swFeat.GetTypeName2() == "SolidBodyFolder")
        {
            var swBodyFolder = swFeat.GetSpecificFeature2() as BodyFolder;
            swBodyFolder.SetAutomaticCutList(true);
            swBodyFolder.SetAutomaticUpdate(true);
            swBodyFolder.UpdateCutList();
            var swSubFeat = swFeat.GetFirstSubFeature() as Feature;
            if (swSubFeat != null)
            {
                //切割清单属性
                var swPropMgr = swSubFeat.CustomPropertyManager;
                //配置特定属性
                var swConfig = (Configuration)swModel.GetActiveConfiguration();
                var swConfigPropMgr = swConfig.CustomPropertyManager;
                var dto = new CutListDto
                {
                    Quantity = 1,
                    Length = swPropMgr.GetPropDoubleValue("Bounding Box Length", "边界框长度"),
                    Width = swPropMgr.GetPropDoubleValue( "Bounding Box Width", "边界框宽度"),
                    Thickness = swPropMgr.GetPropDoubleValue( "Sheet Metal Thickness", "钣金厚度"),
                    Material = swPropMgr.GetPropStringValue( "Material", "材料"),
                    //从配置特定读取属性
                    BendingMark = swConfigPropMgr.GetPropStringValue( "BendingMark", "折弯备注")
                };
                return dto;
            }
        }
        var nextFeat = swFeat.GetNextFeature() as Feature;
        if (nextFeat != null)
        {
            return GetCutListDto(swModel, nextFeat);
        }
        return null;
    }
    

    /// <summary>
    /// 导出DXF图纸
    /// </summary>
    /// <param name="swCompModel"></param>
    /// <param name="partName"></param>
    /// <param name="moduleDto"></param>
    /// <returns></returns>
    private bool ExportDxf(string filePath, string partName, ModuleDto moduleDto)
    {
        var warnings = 0;
        var errors = 0;
        var swCompModel = _swApp.OpenDoc6(filePath, (int)swDocumentTypes_e.swDocPART, 1, "", ref errors,
            ref warnings);
        var swCompPart = swCompModel as PartDoc;
        var modelPath = swCompModel.GetPathName();
        var dxfDir = Path.Combine(@"D:\MyProjects", moduleDto.OdpNumber, "DxfCutlist", $"{moduleDto.ItemNumber}_{moduleDto.Name}_{moduleDto.ModelName}");
        //如果不存在则创建该文件夹
        if (!Directory.Exists(dxfDir))
        {
            Directory.CreateDirectory(dxfDir);
        }
        var outPath = Path.Combine(dxfDir, $"{partName}.dxf");
        _aggregator.SendMessage($"导出Dxf:\t{outPath}", Filter_e.Batch);
        swCompModel.Visible = true;
        //获取拉丝方向
        var dataAlignment = GetAlignment(swCompModel);

        var result = swCompPart.ExportToDWG2(outPath, modelPath,
            (int)swExportToDWG_e.swExportToDWG_ExportSheetMetal,
            true, dataAlignment, false, false,
            1, null);
        //不能直接隐藏，这样会让sw打开的零件越来越多，而是应该全部关闭
        //swCompModel.Visible = false;
        _swApp.CloseDoc(filePath);
        return result;
    }
    /// <summary>
    /// 获取导出DXF图的XY方向
    /// </summary>
    /// <param name="swCompModel"></param>
    /// <returns></returns>
    private double[] GetAlignment(ModelDoc2 swCompModel)
    {
        double[] dataAlignment = { 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 1 };
        //Array[0], Array[1], Array[2] - XYZ coordinates of new origin
        //Array[3], Array[4], Array[5] - coordinates of new x direction vector
        //Array[6], Array[7], Array[8] - coordinates of new y direction vector
        //判断XYAXIS，长边作为X轴，短的作为Y轴，用于限定拉丝方向
        bool status = false;
        string[] sketchNames = { "xy", "XY", "xyaxis", "XYAXIS" };
        foreach (var sketchName in sketchNames)
        {
            swCompModel.ClearSelection2(true);
            if (swCompModel.Extension.SelectByID2(sketchName, "SKETCH", 0, 0, 0, false, 0, null, 0))
            {
                status = true;
                break;//遇到就，跳出循环
            }
        }
        var swFeat = (swCompModel.SelectionManager as SelectionMgr).GetSelectedObject6(1, -1) as Feature;
        if (status && swFeat != null)
        {
            var swSketch = swFeat.GetSpecificFeature2() as Sketch;
            //获取草图中的所有点，转换为object的数组
            var swSketchPoints = swSketch.GetSketchPoints2() as object[];
            //用这三个点计算点到点的距离，并判断长短，长边作为X轴，
            //画3D草图的时候一次性画出两条线，不能分两次画出，否则会判断错误
            var p0 = swSketchPoints[0] as SketchPoint;//最先画的点
            var p1 = swSketchPoints[1] as SketchPoint;//作为坐标原点
            var p2 = swSketchPoints[2] as SketchPoint;//最后画的点
            dataAlignment[0] = p1.X;
            dataAlignment[1] = p1.Y;
            dataAlignment[2] = p1.Z;
            double l1 = Math.Sqrt(Math.Pow(p0.X - p1.X, 2) + Math.Pow(p0.Y - p1.Y, 2) + Math.Pow(p0.Z - p1.Z, 2));
            double l2 = Math.Sqrt(Math.Pow(p2.X - p1.X, 2) + Math.Pow(p2.Y - p1.Y, 2) + Math.Pow(p2.Z - p1.Z, 2));
            if (l1 > l2)
            {
                dataAlignment[3] = p0.X - p1.X;
                dataAlignment[4] = p0.Y - p1.Y;
                dataAlignment[5] = p0.Z - p1.Z;
                dataAlignment[6] = p2.X - p1.X;
                dataAlignment[7] = p2.Y - p1.Y;
                dataAlignment[8] = p2.Z - p1.Z;
            }
            else
            {
                dataAlignment[3] = p2.X - p1.X;
                dataAlignment[4] = p2.Y - p1.Y;
                dataAlignment[5] = p2.Z - p1.Z;
                dataAlignment[6] = p0.X - p1.X;
                dataAlignment[7] = p0.Y - p1.Y;
                dataAlignment[8] = p0.Z - p1.Z;
            }
        }
        swCompModel.ClearSelection2(true);
        return dataAlignment;
    } 
    #endregion

}