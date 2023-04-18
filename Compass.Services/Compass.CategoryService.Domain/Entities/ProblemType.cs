using Compass.Wasm.Shared.Projects;
using Zack.DomainCommons.Models;

namespace Compass.CategoryService.Domain.Entities;

public record ProblemType : AggregateRootEntity, IAggregateRoot, IHasCreationTime, ISoftDelete
{
    public string Name { get;private set; }
    public Stakeholder_e Stakeholder { get; private set; }

    private ProblemType() { }
    public ProblemType(Guid id, string name, Stakeholder_e stakeholder)
    {
        Id=id;
        Name=name;
        Stakeholder=stakeholder;
    }
    public ProblemType ChangeName(string name)
    {
        Name=name;
        return this;
    }
    public ProblemType ChangeStakeholder(Stakeholder_e stakeholder)
    {
        Stakeholder=stakeholder;
        return this;
    }
}




/*
归类	Summary
0客户	合同变更/contract revise
1销售	客户图纸问题/customer service drawing issue
    客户图纸未及时下发/customer service drawing is not released on time
    订单拆分/order seperated
    销售要求暂停/sales requirement
2技术	技术方案变更/technical proposal revise
    生产图纸问题/production drawing issue
    生产图纸未及时下发/production drawing is not released on time
    物料单问题/material list issue
    物料单未及时下发/material list is not released on time
    项目经理要求暂停/project manager requirement
3采购	原材料质量问题/raw material quality issue
    缺料/lack of raw mateiral
4生产	生产质量问题/quality issue during production
    生产线物料丢失/part missing during production
    生产设备问题/production facility issue
    生产产能问题/production capacity issue
    FAT整改/FAT Modification
5物流	发货前货物损坏/goods damage before delivery
    收款问题/payment issue before delivery
    包装问题/packing issue
    运输问题/transportation issue 
 */