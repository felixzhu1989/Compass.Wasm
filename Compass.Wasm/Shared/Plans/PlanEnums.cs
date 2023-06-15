namespace Compass.Wasm.Shared.Plans;

/// <summary>
/// 计划状态
/// </summary>
public enum MainPlanStatus_e
{
    计划, 制图, 生产, 入库, 发货, 结束
}

/// <summary>
/// 计划分类
/// </summary>
public enum MainPlanType_e
{
    NA,ETO,海工,KFC
}

/// <summary>
/// 计划状态
/// </summary>
public enum SubPlanStatus_e
{
    制图, 排版,切割,折弯,焊接,装配,Ansul,待检,打包,完工,取消,暂停
}

/// <summary>
/// 问题标题
/// </summary>
public enum IssueTitle_e
{
    生产图纸未及时下发,
    销售要求暂停,
    生产图纸问题,
    收款问题,
    客户图纸未及时下发,
    技术方案变更,
    订单拆分,
    项目经理要求暂停,
    合同变更,
    生产设备问题,
    缺料,
    物料单未及时下发,
    客户图纸问题,
    生产质量问题,
    包装问题,
    生产产能问题,
    原材料质量问题,
    生产线物料丢失,
    运输问题,
    物料单问题,
    发货前货物损坏,
    FAT整改,
}

/// <summary>
/// 装箱清单中产品类型
/// </summary>
public enum Product_e
{
    NA,Hood,Ceiling
}