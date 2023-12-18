using Compass.Maui.Services;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Compass.Maui.Extensions;

namespace Compass.Maui.ViewModels;

public class MainViewModel:BaseViewModel
{
    private readonly MainPlanService _mainPlanService;
    public MainViewModel(MainPlanService mainPlanService)
    {
        _mainPlanService = mainPlanService;
        Title = "Main Plan";
        GetMainPlanDtosCommand = new AsyncRelayCommand(async () => await GetMainPlanDtosAsync());
        GetMainPlanDtosRestCommand = new AsyncRelayCommand(async () => await GetMainPlanDtosRestAsync());
    }
    public ICommand GetMainPlanDtosCommand { get; }
    public ICommand GetMainPlanDtosRestCommand { get; }
    public ObservableCollection<MainPlanDto> MainPlanDtos { get; } = new();


    private async Task GetMainPlanDtosAsync()
    {
        if (IsBusy) return;
        try
        {
            IsBusy=true;
            var dtos = await _mainPlanService.GetMainPlanDtosAsync();
            if (MainPlanDtos.Count!=0) MainPlanDtos.Clear();
            MainPlanDtos.AddRange(dtos);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            await Shell.Current.DisplayAlert("Error!", $"Unable to get mainplans:{ex.Message}", "OK");
        }
        finally
        {
            IsBusy=false;
        }
    }
    private async Task GetMainPlanDtosRestAsync()
    {
        if (IsBusy) return;
        try
        {
            IsBusy=true;
            var dtos = await _mainPlanService.GetMainPlanDtosRestAsync();
            if (MainPlanDtos.Count!=0) MainPlanDtos.Clear();
            MainPlanDtos.AddRange(dtos);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            await Shell.Current.DisplayAlert("Error!", $"Unable to get mainplans:{ex.Message}", "OK");
        }
        finally
        {
            IsBusy=false;
        }
    }
}