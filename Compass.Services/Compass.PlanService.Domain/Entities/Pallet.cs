using Zack.DomainCommons.Models;

namespace Compass.PlanService.Domain.Entities;
//装箱信息表：托盘->输出Excel，发邮件给Mabel
public record Pallet:AggregateRootEntity,IAggregateRoot,IHasCreationTime,ISoftDelete
{
    #region 基本信息
    public Guid PackingListId { get; init; } //关联装箱清单

    public int PalletNumber { get;private set; } //自增，从1开始
    //技术部生成信息
    public string? ItemNumber { get; private set; } //产品编号 
    public string? ItemType { get; private set; } //产品型号
    public string? ItemLength { get; private set; } //产品长
    public string? ItemWidth { get; private set; } //产品宽
    public string? ItemHeight { get; private set; } //产品高

    //生产现场填写信息
    public string? Length { get; private set; } //包装长
    public string? Width { get; private set; } //包装宽
    public string? Height { get; private set; } //包装高
    public string? GrossWeight { get; private set; } //毛重
    public string? NetWeight { get; private set; } //净重
    public string? Remark { get; private set; }//备注

    #endregion




}