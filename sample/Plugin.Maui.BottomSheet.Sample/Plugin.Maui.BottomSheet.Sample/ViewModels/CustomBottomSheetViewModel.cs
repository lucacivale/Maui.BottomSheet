using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Plugin.Maui.BottomSheet.Navigation;

namespace Plugin.Maui.BottomSheet.Sample.ViewModels;

public partial class CustomBottomSheetViewModel : ObservableObject, IQueryAttributable, IConfirmNavigationAsync
{
    private readonly IBottomSheetNavigationService _bottomSheetNavigationService;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
    private string? _oldusername;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
    private string? _newusername;

    public CustomBottomSheetViewModel(IBottomSheetNavigationService bottomSheetNavigationService)
    {
        _bottomSheetNavigationService = bottomSheetNavigationService;
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query is null
            || query.Count == 0)
        {
            return;
        }

        if (query.TryGetValue("username", out object? username))
        {
            Oldusername = username.ToString();
            Newusername = username.ToString();
        }

        query.Clear();
    }

    public bool SaveCanExecute()
    {
        if (!string.IsNullOrEmpty(Newusername) && Newusername != Oldusername)
        {
            return true;
        }

        else
        {
            return false;
        }
    }

    [RelayCommand(CanExecute = nameof(SaveCanExecute))]
    public async Task Save()
    {
        await _bottomSheetNavigationService.GoBackAsync(new BottomSheetNavigationParameters()
        {
            ["newusername"] = Newusername!,
        });
    }

    [RelayCommand]
    public async Task GoBack()
    {
        await _bottomSheetNavigationService.GoBackAsync();
    }

    public Task<bool> CanNavigateAsync(IBottomSheetNavigationParameters? parameters)
    {
        if (Newusername != Oldusername)
        {
            return Shell.Current.CurrentPage.DisplayAlert(
                "Discard changes?",
                "Are you sure you want to discard the changes?",
                "Yes, discard",
                "Continue editing");
        }
        else
        {
            return Task.FromResult(true);
        }
    }
}