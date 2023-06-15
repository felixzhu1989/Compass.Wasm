using Compass.Wasm.Shared.Plans;
using Zack.DomainCommons.Models;

namespace Compass.PlanService.Domain.Entities;

public record PackingList : AggregateRootEntity, IAggregateRoot, IHasCreationTime, ISoftDelete
{
	#region 基本信息
    public Guid MainPlanId { get; init; }//关联主计划Id

    public Product_e Product { get; private set; }//装箱清单产品类型
    public string PackingType { get; private set; }//包装形式
    public string DeliveryType { get; private set; }//发货形式
    public string? AssyPath { get; set; }//天花烟罩时，需要填写总装地址
    #endregion

    #region ctor
    private PackingList() { }
    public PackingList(Guid id, Guid mainPlanId,Product_e product,string packingType,string deliveryType,string assyPath)
    {
        Id=id;
        MainPlanId=mainPlanId;
        Product=product;
        PackingType=packingType;
        DeliveryType=deliveryType;
        AssyPath=assyPath;
    }
    #endregion
    #region Update
    public void Update(PackingListDto dto)
    {
        ChangeProduct(dto.Product)
            .ChangePackingType(dto.PackingType)
            .ChangeDeliveryType(dto.DeliveryType)
            .ChangeAssyPath(dto.AssyPath);
        NotifyModified();
    }
    public PackingList ChangeProduct(Product_e product)
    {
        Product=product;
        return this;
    }

    public PackingList ChangePackingType(string packingType)
    {
        PackingType = packingType;
        return this;
    }

    public PackingList ChangeDeliveryType(string deliveryType)
    {
        DeliveryType = deliveryType;
        return this;
    }
    public PackingList ChangeAssyPath(string? assyPath)
    {
        AssyPath = assyPath;
        return this;
    }
    #endregion

}