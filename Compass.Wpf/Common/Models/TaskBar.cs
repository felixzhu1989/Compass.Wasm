using Prism.Mvvm;

namespace Compass.Wpf.Common.Models;
/// <summary>
/// 任务栏实体类
/// </summary>
public class TaskBar:BindableBase
{
    /// <summary>
    /// 图标
    /// </summary>
    public string Icon { get; set; }
    /// <summary>
    /// 标题
    /// </summary>
    public string Title { get; set; }    
    private string content;
    /// <summary>
    /// 内容，需要实现自动更新
    /// </summary>
    public string Content
    {
        get => content;
        set { content = value; RaisePropertyChanged(); }
    }
    /// <summary>
    /// 颜色
    /// </summary>
    public string Color { get; set; }
    /// <summary>
    /// 触发目标
    /// </summary>
    public string Target { get; set; }
}