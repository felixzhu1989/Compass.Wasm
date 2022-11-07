using Compass.Wasm.Shared.ProjectService;
using Zack.DomainCommons.Models;

namespace Compass.ProjectService.Domain.Entities;

public record Project : AggregateRootEntity, IAggregateRoot,IHasCreationTime, ISoftDelete
{
    //BasicInfo
    public string OdpNumber { get; private set; }
    public string Name { get; private set; }
    public DateTime DeliveryDate { get;private set; }//交货时间，重要
    public ProjectType ProjectType { get; private set; }
    public RiskLevel RiskLevel { get; private set; }
    public string? SpecialNotes { get; private set; }
    public string? ContractUrl { get; private set; }
    public string? BomUrl { get; private set; }
    //AdditionalInfo
    public DateTime CreationTime { get; private set; }//创建时间，根据它来排序

    private Project() { }
    public Project(Guid id, string odpNumber, string name,DateTime deliveryDate, ProjectType projectType, RiskLevel riskLevel, string? specialNotes)
    {
        Id = id;
        OdpNumber= odpNumber;
        Name= name;
        DeliveryDate = deliveryDate;
        ProjectType= projectType;
        RiskLevel= riskLevel;
        SpecialNotes= specialNotes;
        CreationTime=DateTime.Now;
    }
    #region ChangeProperty
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
    #endregion



}