using Compass.Wasm.Shared.ProjectService;
using Compass.Wasm.Shared.ProjectService.Notification;
using Zack.DomainCommons.Models;

namespace Compass.ProjectService.Domain.Entities;

public record Project : AggregateRootEntity, IAggregateRoot, IHasCreationTime, ISoftDelete
{
    //BasicInfo
    public string OdpNumber { get; private set; }
    public string Name { get; private set; }
    public DateTime DeliveryDate { get; private set; }//交货时间，重要，根据它来排序
    //状态表示当前，根据EventBus接收到事件，自动修改
    public ProjectStatus_e ProjectStatus { get; private set; }//计划,制图,生产,入库,结束
    //其他参数
    public ProjectType_e ProjectType { get; private set; }
    public RiskLevel_e RiskLevel { get; private set; }
    public string? SpecialNotes { get; private set; }
    public string? ContractUrl { get; private set; }
    public string? BomUrl { get; private set; }
    public string? AttachmentsUrl { get; private set; }//上传得附件，多文件
    public string? FinalInspectionUrl { get; private set; }//上传最终检验单，多文件



    private Project() { }
    public Project(Guid id, string odpNumber, string name, DateTime deliveryDate, ProjectType_e projectType, RiskLevel_e riskLevel, string? specialNotes, string? contractUrl)
    {
        Id = id;
        OdpNumber= odpNumber;
        Name= name;
        DeliveryDate= deliveryDate;
        ProjectType= projectType;
        RiskLevel= riskLevel;
        SpecialNotes= specialNotes;
        ContractUrl= contractUrl;
        ProjectStatus = ProjectStatus_e.计划;//初始状态是计划状态
        //发布领域事件
        AddDomainEvent(new ProjectCreatedNotification(id, name));

    }

    public void Update(ProjectDto dto)
    {
        ChangeOdpNumber(dto.OdpNumber!.ToUpper()).ChangeName(dto.Name!)
            .ChangeDeliveryDate(dto.DeliveryDate)
            .ChangeProjectType(dto.ProjectType).ChangeRiskLevel(dto.RiskLevel)
            .ChangeContractUrl(dto.ContractUrl!).ChangeBomUrl(dto.BomUrl!)
            .ChangeFinalInspectionUrl(dto.FinalInspectionUrl!)
            .ChangeAttachmentsUrl(dto.AttachmentsUrl!)
            .ChangeSpecialNotes(dto.SpecialNotes!);
        
        NotifyModified();
        //todo:发布领域事件
        //测试发布领域事件
        //AddDomainEvent(new TestNotification(odpNumber));

    }


    #region ChangeProperty
    public Project ChangeProjectStatus(ProjectStatus_e projectStatus)
    {
        ProjectStatus = projectStatus;
        return this;
    }


    public Project ChangeOdpNumber(string odpNumber)
    {
        OdpNumber= odpNumber;
        return this;
    }
    public Project ChangeName(string name)
    {
        Name= name;
        return this;
    }
    public Project ChangeDeliveryDate(DateTime deliveryDate)
    {
        DeliveryDate= deliveryDate;
        return this;
    }

    public Project ChangeProjectType(ProjectType_e projectType)
    {
        ProjectType= projectType;
        return this;
    }
    public Project ChangeRiskLevel(RiskLevel_e riskLevel)
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

    #endregion

    public override void SoftDelete()
    {
        base.SoftDelete();
        //发布删除项目领域事件
        AddDomainEvent(new ProjectDeletedNotification(Id));
    }

}