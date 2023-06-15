using Zack.DomainCommons.Models;

namespace Compass.PlanService.Domain.Entities;

//打包清单：配件->打印横向A4给生产，生产现场打印标签
public record PackingItem : AggregateRootEntity, IAggregateRoot, IHasCreationTime, ISoftDelete
{
    #region 基本信息
    public Guid PackingListId { get; init; } //关联装箱清单

    //从物料模板查询信息
    public string? MtlNumber { get; private set; } //物料编码
    public string? Description { get;private set; } //中文描述/产品编号
    public string? EnDescription { get;private set; } //英文描述
    public string? Type { get; private set; } //类型/产品型号
    public double Quantity { get; private set; } //数量
    public string? Unit { get; private set; }//PCS
    public string? Length { get; private set; } //长
    public string? Width { get; private set; } //宽
    public string? Height { get; private set; } //高
    public string? Material { get; private set; } //材质
    public string? Remark { get; set; }//备注
    public string? CalcRule { get; set; } //计算规则

    #endregion
    #region 状态信息
    public bool ReCreate { get; set; } //重新生成时删除
    public bool Pallet { get; set; }//单独托盘
    public bool PrintLabel { get; set; }//打印标签
    public bool OneLabel { get; set; }//打印1张标签，默认false表示需要根据数量Quantity打印多张
    #endregion


}