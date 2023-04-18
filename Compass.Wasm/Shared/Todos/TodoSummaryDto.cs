using System.Collections.ObjectModel;

namespace Compass.Wasm.Shared.Todos;

public class TodoSummaryDto : BaseDto
{
    private int sum;
    /// <summary>
    /// 待办事项总数
    /// </summary>
    public int Sum
    {
        get => sum;
        set { sum = value; OnPropertyChanged(); }
    }
    private int completedCount;
    /// <summary>
    /// 待办事项完成数量
    /// </summary>
    public int CompletedCount
    {
        get => completedCount;
        set { completedCount = value; OnPropertyChanged(); }
    }
    private int memoCount;
    /// <summary>
    /// 备忘录数量
    /// </summary>
    public int MemoCount
    {
        get => memoCount;
        set { memoCount = value; OnPropertyChanged(); }
    }
    private string completedRatio;
    /// <summary>
    /// 完成比例
    /// </summary>
    public string CompletedRatio
    {
        get => completedRatio;
        set { completedRatio = value; OnPropertyChanged(); }
    }
    private ObservableCollection<TodoDto> todoDtos;
    /// <summary>
    /// 待办事项列表
    /// </summary>
    public ObservableCollection<TodoDto> TodoDtos
    {
        get => todoDtos;
        set { todoDtos = value; OnPropertyChanged(); }
    }
    private ObservableCollection<MemoDto> memoDtos;
    /// <summary>
    /// 备忘录列表
    /// </summary>
    public ObservableCollection<MemoDto> MemoDtos
    {
        get => memoDtos;
        set { memoDtos = value; OnPropertyChanged(); }
    }
}