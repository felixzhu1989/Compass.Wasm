namespace Compass.Wasm.Shared.PlanService;
/// <summary>
/// 计划分类
/// </summary>
public enum ProductionPlanType_e
{
    No, 海工, ETO, KFC
}
/// <summary>
/// 计划状态
/// </summary>
public enum ProductionPlanStatus_e
{
    生产中, 排版,切割,折弯,焊接,装配,Ansul,待检,打包,完工, 取消, 暂停
}
