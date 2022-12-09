﻿using Zack.DomainCommons.Models;

namespace Compass.DataService.Domain.Entities;

public abstract record ModuleData: BaseEntity
{
    //Id直接使用ModuleId
    //产品基本属性有长宽高，注意和以前的作图程序不同，这里是总长，
    public double Length { get;private set; }
    public double Width { get;private set; }
    public double Height { get;private set; }
}