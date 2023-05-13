using Compass.Wasm.Shared.Projects;
using Compass.Wasm.Shared.Projects.Notifications;
using Zack.DomainCommons.Models;

namespace Compass.ProjectService.Domain.Entities;

public record Project : AggregateRootEntity, IAggregateRoot, IHasCreationTime, ISoftDelete
{
    #region 基本属性
    public string OdpNumber { get; private set; }
    public string Name { get; private set; }
    public DateTime DeliveryDate { get; private set; }//交货时间，根据它来排序
    public ProjectType_e ProjectType { get; private set; }
    public RiskLevel_e RiskLevel { get; private set; }
    public string? SpecialNotes { get; private set; }
    public Guid? Designer { get; private set; }//制图人，由项目经理指定
    #endregion

    #region 文件属性（单独上传），上传即保存，无需再确认
    public string? ContractUrl { get; private set; }
    public string? BomUrl { get; private set; }
    public string? AttachmentsUrl { get; private set; }//上传附件，多文件
    public string? FinalInspectionUrl { get; private set; }//上传最终检验单，多文件 
    #endregion

    #region 状态属性（单独修改）
    //状态表示当前，根据EventBus接收到事件，自动修改
    public ProjectStatus_e ProjectStatus { get; private set; }//计划,制图,生产,入库,结束

    #endregion
    
    #region ctor
    private Project() { }
    public Project(Guid id, string odpNumber, string name, DateTime deliveryDate, ProjectType_e projectType, RiskLevel_e riskLevel, string? specialNotes)
    {
        Id = id;
        OdpNumber= odpNumber;
        Name= name;
        DeliveryDate= deliveryDate;
        ProjectType= projectType;
        RiskLevel= riskLevel;
        SpecialNotes= specialNotes;
        ProjectStatus = ProjectStatus_e.计划;//初始状态是计划状态
        //发布领域事件
        AddDomainEvent(new ProjectCreatedNotification(id, name));

    }
    #endregion

    #region Update
    public void Update(ProjectDto dto)
    {
        ChangeOdpNumber(dto.OdpNumber!.ToUpper()).ChangeName(dto.Name!)
            .ChangeDeliveryDate(dto.DeliveryDate)
            .ChangeProjectType(dto.ProjectType).ChangeRiskLevel(dto.RiskLevel)
            .ChangeSpecialNotes(dto.SpecialNotes!)
            .ChangeDesigner(dto.Designer);
        NotifyModified();
        //todo:发布领域事件
        //测试发布领域事件
        //AddDomainEvent(new TestNotification(odpNumber));
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

    public Project ChangeDesigner(Guid? designer)
    {
        Designer=designer;
        return this;
    }
    #endregion

    #region 更新文件属性

    public void UploadFiles(ProjectDto dto)
    {
        ChangeContractUrl(dto.ContractUrl)
            .ChangeBomUrl(dto.BomUrl)
            .ChangeAttachmentsUrl(dto.AttachmentsUrl)
            .ChangeFinalInspectionUrl(dto.FinalInspectionUrl);

        NotifyModified();
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
    
    #region 更新状态属性
    public Project ChangeProjectStatus(ProjectStatus_e projectStatus)
    {
        ProjectStatus = projectStatus;
        return this;
    }

    
    #endregion

    #region 删除
    public override void SoftDelete()
    {
        base.SoftDelete();
        //发布删除项目领域事件
        AddDomainEvent(new ProjectDeletedNotification(Id));
    } 
    #endregion
}