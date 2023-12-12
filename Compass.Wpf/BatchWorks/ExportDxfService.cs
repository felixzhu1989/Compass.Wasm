using System.Collections;
using System.Collections.Generic;
using SolidWorks.Interop.sldworks;
using System.IO;
using Compass.Wasm.Shared.Categories;
using Compass.Wasm.Shared.Extensions;
using Compass.Wasm.Shared.Params;
using SolidWorks.Interop.swconst;

namespace Compass.Wpf.BatchWorks;

public interface IExportDxfService
{
    Task ExportHoodDxfAsync(ModuleDto moduleDto);
}


public class ExportDxfService : IExportDxfService
{
    #region ctor
    private readonly ICutListService _cutListService;
    private readonly IDxfDataService _dxfDataService;
    private readonly IAccCutListService _accCutListService;
    private readonly IModuleService _moduleService;
    private readonly ISldWorks _swApp;
    private readonly IEventAggregator _aggregator;
    public ExportDxfService(IContainerProvider provider)
    {
        _cutListService = provider.Resolve<ICutListService>();
        _dxfDataService = provider.Resolve<IDxfDataService>();
        _accCutListService = provider.Resolve<IAccCutListService>();
        _moduleService = provider.Resolve<IModuleService>();
        _aggregator= provider.Resolve<IEventAggregator>();
        _swApp = SwUtility.ConnectSw(_aggregator);
    }
    #endregion

    public async Task ExportHoodDxfAsync(ModuleDto moduleDto)
    {
        var dtos = new List<CutListDto>();
        //todo:判断导图类型
        switch (moduleDto.ExportWay)
        {
            case ExportWay_e.标准模式:
                {
                    //获取pack后的装配体地址
                    var packPath = moduleDto.GetPackPath(out _, out var packDir);
                    //判断装配体是否存在，不存在抛出异常，提醒用户
                    if (!File.Exists(packPath))
                    {
                        var psi = new ProcessStartInfo("Explorer.exe") { Arguments =packDir };
                        Process.Start(psi);
                        throw new FileNotFoundException("PackAndGo后的文件未找到，请检查该分段是否已经完成作图", packPath);
                    }
                    ExportByAssy(ref dtos, packPath, moduleDto);
                    //更新Module的IsCutListOk
                    moduleDto.IsCutListOk= true;
                    break;
                }
            case ExportWay_e.装配体模式:
                {
                    if (string.IsNullOrEmpty(moduleDto.AssyPath)) break;//空字符串跳过
                    if (!File.Exists(moduleDto.AssyPath)) break;//不存在跳过

                    ExportByAssy(ref dtos, moduleDto.AssyPath, moduleDto);

                    //更新Module的IsCutListOk
                    moduleDto.IsCutListOk= true;
                    break;
                }
            case ExportWay_e.拷贝文件模式:
                {
                    //IDxfDataService
                    var response = await _dxfDataService.GetFirstOrDefault(moduleDto.Id.Value);
                    var dxfData = response.Result;
                    var accNumber = dxfData.AccNumber;
                    if (string.IsNullOrEmpty(accNumber)) break;

                    var accs = accNumber.StringToAccs();
                    foreach (var acc in accs)
                    {
                        //先拷贝文件
                        CopyDxf(acc.Type, moduleDto);
                        
                        //根据AccType查询到模板CutList
                        var param = new AccCutListParam { AccType = acc.Type };
                        var accResponse = await _accCutListService.GetAllByAccTypeAsync(param);
                        var tempDtos = accResponse.Result;

                        foreach (var tempDto in tempDtos)
                        {
                            //首先查看有没有重复，重复就增加数量
                            var existDto = dtos.FirstOrDefault(x => x.PartNo.Equals(tempDto.PartNo, StringComparison.OrdinalIgnoreCase));
                            if (existDto!=null)
                            {
                                existDto.Quantity+=tempDto.Quantity*acc.Number;//增加数量
                                continue;//继续循环下一个零件
                            }
                            //没有重复，没有重复就新增行
                            var dto = new CutListDto
                            {
                                ModuleId = moduleDto.Id.Value,//分段的Id
                                PartDescription =tempDto.PartDescription,//描述使用Part Name
                                Length = tempDto.Length,
                                Width = tempDto.Width,
                                Thickness = tempDto.Thickness,
                                Quantity = tempDto.Quantity*acc.Number,
                                Material = tempDto.Material,
                                PartNo = tempDto.PartNo,//文件名
                                BendingMark = tempDto.BendingMark,
                            };
                            dtos.Add(dto);
                        }
                    }
                    //更新Module的IsCutListOk
                    moduleDto.IsCutListOk= true;
                    break;
                }
            case ExportWay_e.不导图模式:
            default:
                break;
        }

        //所有图纸导出完成后
        foreach (var dto in dtos)
        {
            //CutList提交数据库
            await _cutListService.AddAsync(dto);
        }
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
        Debug.Print(swComp.Name);
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
    private CutListDto? GetCutListDto(ModelDoc2 swModel, Feature swFeat)
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
                    Width = swPropMgr.GetPropDoubleValue("Bounding Box Width", "边界框宽度"),
                    Thickness = swPropMgr.GetPropDoubleValue("Sheet Metal Thickness", "钣金厚度"),
                    Material = swPropMgr.GetPropStringValue("Material", "材料"),
                    //从配置特定读取属性
                    BendingMark = swConfigPropMgr.GetPropStringValue("BendingMark", "折弯备注")
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
    /// 根据装配体地址导出
    /// </summary>
    private void ExportByAssy(ref List<CutListDto> dtos, string assyPath, ModuleDto moduleDto)
    {
        //优化进程外调用命令变缓慢的问题，不知是否能够解决导图慢的问题（貌似没什么用）
        _swApp.CommandInProgress = true;
        //打开装配体
        var swAssy = _swApp.OpenAssemblyDoc(out ModelDoc2 swModel, assyPath, _aggregator);
        var comps = (IEnumerable)swAssy.GetComponents(false);
        foreach (var comp in comps)
        {
            var swComp = comp as Component2;
            //Debug.Print(swComp.GetPathName());//查看所有状态OK的零件文件

            //Debug.Print(swComp.GetPathName());//查看所有文件
            var swCompModel = swComp.GetModelDoc2() as ModelDoc2;
            //2023.10.13，必须先判断状态，再计数，否则非法状态下的零件也会被计数
            //检查零部件是否为零件，并且检查合法状态，并且是钣金
            if (swCompModel!=null &&
                swCompModel.GetType()==(int)swDocumentTypes_e.swDocPART &&
                CheckPartStatus(swComp) &&
                CheckSheetMetal(swComp))
            {
                //获取下料清单,存在就数量+1，跳出去
                var partName = Path.GetFileNameWithoutExtension(swComp.GetPathName()); ;
                var existDto = dtos.FirstOrDefault(x => x.PartNo.Equals(partName, StringComparison.OrdinalIgnoreCase));
                if (existDto!=null)
                {
                    existDto.Quantity++;//数量+1
                    continue;//继续循环下一个零件
                }

                //如果是则增加下料清单信息
                var swFeat = swCompModel.FirstFeature() as Feature;
                //获取下料清单信息
                var dto = GetCutListDto(swCompModel, swFeat);
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
        foreach (var dto in dtos.Where(dto => !ExportDxf(dto.FilePath, dto.PartNo, moduleDto)))
        {
            throw new Exception($"零件【{dto.PartNo}】导出dxf时发生错误！");
        }
        _swApp.CommandInProgress=false;
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

    /// <summary>
    /// 拷贝Dxf模板文件
    /// </summary>
    private void CopyDxf(AccType_e accType, ModuleDto moduleDto)
    {
        var keyword = accType.ToString().Split('_').First();
        //目标地址
        var dxfDir = Path.Combine(@"D:\MyProjects", moduleDto.OdpNumber, "DxfCutlist", $"{moduleDto.ItemNumber}_{moduleDto.Name}_{moduleDto.ModelName}");
        //如果不存在则创建该文件夹
        if (!Directory.Exists(dxfDir))
        {
            Directory.CreateDirectory(dxfDir);
        }
        //dxf模板文件地址
        var dxfTemplateDir = @"D:\halton\01 Tech Dept\05 Products Library\07 Control Cabin\00 DxfTemplate";
        //根据关键字，获取要拷贝的文件夹
        var dxfTemplatePath = Path.Combine(dxfTemplateDir, keyword);
        var files = Directory.GetFiles(dxfTemplatePath);
        foreach (var sourceFile in files)
        {
            var destFile = Path.Combine(dxfDir, Path.GetFileName(sourceFile));
            //如果文件在程序目录中已经存在，则删除，避免弹窗中断程序
            if (File.Exists(destFile)) File.Delete(destFile);
            File.Copy(sourceFile, destFile);
        }
    }


    #endregion

}