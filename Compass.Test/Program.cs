// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

var c = typeof(KvfData);
var pors = c.GetProperties();
foreach (var pro in pors)
{
    Console.WriteLine(pro.Name);
}



public abstract record ModuleData 
{
    public Guid ModuleId { get; init; }
    //产品基本属性有长宽高，注意和以前的作图程序不同，这里是总长，
    public double Length { get; private set; }
    public double Width { get; private set; }
    public double Height { get; private set; }
    public string ModelTag { get; set; } //不映射到数据库
}

public record KvfData : ModuleData
{
    //排风口参数
    public double MiddleToRight { get; private set; } //中心距离右端
    public double ExhaustSpigotLength { get; private set; }
    public double ExhaustSpigotWidth { get; private set; }
    public double ExhaustSpigotHeight { get; private set; }
}