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
public enum HoodPlanStatus_e
{
    制图, 排版,切割,折弯,焊接,装配,Ansul,待检,打包,完工,取消,暂停
}

public enum DeliveryBatch_e
{
    NA,一,二,三,四,五,六,七,八,九,十
}