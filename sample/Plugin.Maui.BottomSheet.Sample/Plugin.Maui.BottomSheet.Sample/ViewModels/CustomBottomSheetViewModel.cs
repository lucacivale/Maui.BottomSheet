using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Plugin.Maui.BottomSheet.Navigation;

namespace Plugin.Maui.BottomSheet.Sample.ViewModels;

public partial class CustomBottomSheetViewModel : ObservableObject, IConfirmNavigationAsync, INavigationAware
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
        await _bottomSheetNavigationService.GoBackAsync();
    }

    [RelayCommand]
    public async Task GoBack()
    {
        await _bottomSheetNavigationService.GoBackAsync();
    }

    public async Task<bool> CanNavigateAsync(IBottomSheetNavigationParameters parameters)
    {
        bool canNavigate = true;
        
        if (Newusername != Oldusername)
        {
            string button = await  Shell.Current.CurrentPage.DisplayActionSheetAsync(
                "Pending changes?",
                "Cancel",
                "Discard",
                "Save");
            
            canNavigate = button == "Discard" || button == "Save";

            if (button == "Save")
            {
                parameters.Add("Save", true);
            }
        }

        return canNavigate;
    }

    public void OnNavigatedFrom(IBottomSheetNavigationParameters parameters)
    {
        if (parameters.ContainsKey("Save"))
        {
            parameters.Add("newusername", Newusername!);
        }

        Console.WriteLine($"OnNavigatedFrom CustomBottomSheetViewModel");
    }

    public void OnNavigatedTo(IBottomSheetNavigationParameters parameters)
    {
        if (parameters.TryGetValue("username", out object? username))
        {
            Oldusername = username.ToString();
            Newusername = username.ToString();
        }
    }
}