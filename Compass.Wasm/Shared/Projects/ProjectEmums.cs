namespace Compass.Wasm.Shared.Projects;
/// <summary>
/// 项目分类
/// </summary>
public enum ProjectType_e
{
    国内, 港澳台, 日本, 韩国, 中东, 华为, 海工,其他
}
/// <summary>
/// 风险等级
/// </summary>
public enum RiskLevel_e
{
    无风险, 低风险, 中风险, 高风险
}
/// <summary>
/// 项目状态
/// </summary>
public enum ProjectStatus_e
{
    计划,制图,生产,入库,发货,结束
}
/// <summary>
/// 相关方
/// </summary>
public enum Stakeholder_e
{
    客户,销售,技术,采购,生产,物流
}