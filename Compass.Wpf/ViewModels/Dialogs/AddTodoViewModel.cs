using MaterialDesignThemes.Wpf;
using Prism.Mvvm;

namespace Compass.Wpf.ViewModels.Dialogs;
public class AddTodoViewModel : BindableBase, IDialogHostAware
{
    public AddTodoViewModel()
    {
        SaveCommand=new DelegateCommand(Save);
        CancelCommand=new DelegateCommand(Cancel);
    }
    public string DialogHostName { get; set; }
    public DelegateCommand SaveCommand { get; set; }
    public DelegateCommand CancelCommand { get; set; }
    private TodoDto model;
    public TodoDto Model
    {
        get => model;
        set { model = value; RaisePropertyChanged(); }
    }

    private void Cancel()
    {
        if (DialogHost.IsDialogOpen(DialogHostName))
            //取消时只返回No，告知操作结束
            DialogHost.Close(DialogHostName, new DialogResult(ButtonResult.No));
    }
    private void Save()
    {
        //验证数据已填写
        if (string.IsNullOrWhiteSpace(Model.Title)||string.IsNullOrWhiteSpace(model.Content)) return;
        if (DialogHost.IsDialogOpen(DialogHostName))
        {
            DialogParameters param = new() { { "Value", Model } };
            //保存时传递参数param
            DialogHost.Close(DialogHostName, new DialogResult(ButtonResult.OK, param));
        }
    }
    public void OnDialogOpen(IDialogParameters parameters)
    {
        Model = parameters.ContainsKey("Value") ? parameters.GetValue<TodoDto>("Value") : new();
    }
}
