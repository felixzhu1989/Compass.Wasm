using Compass.Wasm.Shared.Data;
using Compass.Wasm.Shared.Projects;
using Compass.Wasm.Shared.Projects.Notifications;
using Zack.DomainCommons.Models;

namespace Compass.ProjectService.Domain.Entities;

public record Module:AggregateRootEntity,IAggregateRoot, IHasCreationTime, ISoftDelete
{
    public Guid DrawingId { get; private set; }//只能通过聚合根的标识符引用。
    public Guid ModelTypeId { get; private set; }//标明该分段是属于什么什么子模型
    public string Name { get; private set; }
    public string ModelName { get; private set; }
    public string? SpecialNotes { get;private set; }
    public string? QrCodeUrl { get; set; }
    public bool IsModuleDataOk { get;private set; }//用于标记图纸得参数是否已经得到修改
    public bool IsCutListOk { get; private set; }


    private Module() { }
    public Module(Guid id,Guid drawingId,Guid modelTypeId, string name,string modelName, string? specialNotes,double length,double width,double height,SidePanel_e sidePanel)
    {
        Id = id;
        DrawingId = drawingId;
        ModelTypeId = modelTypeId;
        Name = name;
        ModelName=modelName;
        SpecialNotes = specialNotes;

        //todo:改成Domain事件,写在Module实体类中
        #region 创建Module的ModuleData参数
        var model = modelName.Split('_')[0];
        AddDomainEvent(new ModuleCreatedNotification(id, model, modelTypeId, length, width, height, sidePanel));
        #endregion

    }

    public void Update(ModuleDto dto)
    {
        ChangeModelTypeId(dto.ModelTypeId.Value)
            .ChangeName(dto.Name.ToUpper())
            .ChangeModelName(dto.ModelName)
            .ChangeSpecialNotes(dto.SpecialNotes)
            .ChangeQrCodeUrl(dto.QrCodeUrl)
            .ChangeIsModuleDataOk(dto.IsModuleDataOk)
            .ChangeIsCutListOk(dto.IsCutListOk);
        NotifyModified();

        //todo:改成领域事件
        #region 修改Module的ModuleData参数
        var model = dto.ModelName.Split('_')[0];
        AddDomainEvent(new ModuleUpdatedNotification(dto.Id.Value, model, dto.ModelTypeId.Value,dto.Length, dto.Width, dto.Height,dto.SidePanel));
        #endregion
    }

    public Module ChangeModelTypeId(Guid modelTypeId)
    {
        
        ModelTypeId = modelTypeId;
        return this;
    }
    public Module ChangeName(string name)
    {
        Name = name;
        return this;
    }
    public Module ChangeModelName(string modelName)
    {
        ModelName = modelName;
        return this;
    }
    public Module ChangeSpecialNotes(string? specialNotes)
    {
        SpecialNotes = specialNotes;
        return this;
    }
    public Module ChangeQrCodeUrl(string? qrCodeUrl)
    {
        QrCodeUrl = qrCodeUrl;
        return this;
    }
    public Module ChangeIsModuleDataOk(bool isModuleDataOk)
    {
        IsModuleDataOk = isModuleDataOk;
        return this;
    }
    public Module ChangeIsCutListOk(bool isCutListOk)
    {
        IsCutListOk = isCutListOk;
        return this;
    }
    public override void SoftDelete()
    {
        //发出领域事件，删除当前的参数
        AddDomainEvent(new ModuleDeleteNotification(Id));
        base.SoftDelete();
    }
}