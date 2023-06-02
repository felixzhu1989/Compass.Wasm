﻿using Compass.Wasm.Shared.Projects;
using Zack.DomainCommons.Models;
namespace Compass.ProjectService.Domain.Entities;

public record Drawing : AggregateRootEntity, IAggregateRoot, IHasCreationTime, ISoftDelete
{
    public Guid ProjectId { get; private set; }//只能通过聚合根的标识符引用。
    public string ItemNumber { get; private set; }
    public int Batch { get; private set; }//分批

    //图纸链接，多张图纸使用回车\n隔开
    public string? DrawingUrl { get; private set; }
    public string? ImageUrl { get; private set; }


    private Drawing() { }
    public Drawing(Guid id, Guid projectId, string itemNumber,int batch,string? drawingUrl,string imageUrl)
    {
        Id =id;
        ProjectId = projectId;
        ItemNumber = itemNumber;
        Batch = batch;
        DrawingUrl = drawingUrl;
        ImageUrl = imageUrl;
    }

    public void Update(DrawingDto dto)
    {
        ChangeItemNumber(dto.ItemNumber)
            .ChangeBatch(dto.Batch)
            .ChangeDrawingUrl(dto.DrawingUrl)
            .ChangeImageUrl(dto.ImageUrl);
        NotifyModified();
    }


    public Drawing ChangeItemNumber(string itemNumber)
    {
        ItemNumber= itemNumber;
        return this;
    }

    public Drawing ChangeBatch(int batch)
    {
        Batch=batch;
        return this;
    }
    public Drawing ChangeDrawingUrl(string drawingUrl)
    {
        DrawingUrl = drawingUrl;
        return this;
    }
    public Drawing ChangeImageUrl(string imageUrl)
    {
        ImageUrl = imageUrl;
        return this;
    }
}