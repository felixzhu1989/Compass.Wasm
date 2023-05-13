namespace Compass.Wasm.Shared.Projects;

public class LessonDto : BaseDto
{
    public Guid Id { get; set; }
    public Guid ProjectId { get; set; }
    public DateTime CreationTime { get; set; }//记录问题的时间（不允许修改）


    private string? content;
    public string? Content
    {
        get => content;
        set { content = value; OnPropertyChanged();}
    }
    private string? contentUrl;
    public string? ContentUrl
    {
        get => contentUrl;
        set { contentUrl = value; OnPropertyChanged(); }
    }
}