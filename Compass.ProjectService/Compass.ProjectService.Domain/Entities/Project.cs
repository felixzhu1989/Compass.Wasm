using Compass.Wasm.Shared.ProjectService;
using Compass.Wasm.Shared.ProjectService.Notification;
using Zack.DomainCommons.Models;

namespace Compass.ProjectService.Domain.Entities;

public record Project : AggregateRootEntity, IAggregateRoot,IHasCreationTime, ISoftDelete
{
    //BasicInfo
    public string OdpNumber { get; private set; }
    public string Name { get; private set; }
    //合同参数
    public DateTime ReceiveDate { get; private set; }
    public DateTime DeliveryDate { get; private set; }//交货时间，重要，根据它来排序
    //其他参数
    public ProjectType ProjectType { get; private set; }
    public RiskLevel RiskLevel { get; private set; }
    public string? SpecialNotes { get; private set; }
    public string? ContractUrl { get; private set; }
    public string? BomUrl { get; private set; }
    public string? AttachmentsUrl { get; private set; }//上传得附件，多文件
    public string? FinalInspectionUrl { get;private set; }//上传最终检验单，多文件
    
    private Project() { }
    public Project(Guid id, string odpNumber, string name, DateTime receiveDate, DateTime deliveryDate, ProjectType projectType, RiskLevel riskLevel, string? specialNotes)
    {
        Id = id;
        OdpNumber= odpNumber;
        Name= name;
        ReceiveDate = receiveDate;
        DeliveryDate= deliveryDate;
        ProjectType= projectType;
        RiskLevel= riskLevel;
        SpecialNotes= specialNotes;
        //发布领域事件
        AddDomainEvent(new ProjectCreatedNotification(id,name, deliveryDate));

    }
    #region ChangeProperty
    public Project ChangeOdpNumber(string odpNumber)
    {
        OdpNumber= odpNumber;
        //测试发布领域事件
        //AddDomainEvent(new TestNotification(odpNumber));
        return this;
    }
    public Project ChangeName(string name)
    {
        Name= name;
        return this;
    }

    public Project ChangeReceiveDate(DateTime receiveDate)
    {
        ReceiveDate = receiveDate;
        return this;
    }
    public Project ChangeDeliveryDate(DateTime deliveryDate)
    {
        DeliveryDate= deliveryDate;
        return this;
    }

    public Project ChangeProjectType(ProjectType projectType)
    {
        ProjectType= projectType;
        return this;
    }
    public Project ChangeRiskLevel(RiskLevel riskLevel)
    {
        RiskLevel= riskLevel;
        return this;
    }

    public Project ChangeSpecialNotes(string specialNotes)
    {
        SpecialNotes= specialNotes;
        return this;
    }
    public Project ChangeContractUrl(string contractUrl)
    {
        ContractUrl= contractUrl;
        return this;
    }
    public Project ChangeBomUrl(string bomUrl)
    {
        BomUrl= bomUrl;
        return this;
    }
    public Project ChangeAttachmentsUrl(string attachmentsUrl)
    {
        AttachmentsUrl= attachmentsUrl;
        return this;
    }
    public Project ChangeFinalInspectionUrl(string finalInspectionUrl)
    {
        FinalInspectionUrl= finalInspectionUrl;
        return this;
    }

    public override void SoftDelete()
    {
        base.SoftDelete();
        AddDomainEvent(new ProjectDeletedNotification(Id));
    }

    #endregion

}