namespace Compass.Wasm.Shared.ProjectService;
/// <summary>
/// 项目分类
/// </summary>
public enum ProjectType
{
    国内, 港澳台, 日本, 韩国, 中东, 华为, 其他
}
/// <summary>
/// 风险等级
/// </summary>
public enum RiskLevel
{
    低风险, 中风险, 高风险
}
/// <summary>
/// 项目状态
/// </summary>
public enum ProjectStatus
{
    计划,制图,生产,入库,结束
}
/// <summary>
/// 相关方
/// </summary>
public enum Stakeholder
{
    客户,销售,技术,采购,生产,物流
}