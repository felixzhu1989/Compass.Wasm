namespace Compass.Wasm.Shared.TodoService;

public class TodoDto:BaseDto
{
    private string title;
    public string? Title
    {
        get => title;
        set { title = value; OnPropertyChanged();}
    }
    private string content;
    public string? Content
    {
        get => content;
        set { content = value; OnPropertyChanged(); }
    }
    private int status;
    public int Status
    {
        get => status;
        set { status = value; OnPropertyChanged(); }
    }
    private Guid userId;
    public Guid UserId
    {
        get => userId;
        set { userId = value; OnPropertyChanged(); }
    }
}