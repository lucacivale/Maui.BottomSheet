using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Plugin.Maui.BottomSheet.Navigation;

namespace Plugin.Maui.BottomSheet.Sample.ViewModels;

public partial class CustomBottomSheetViewModel (IBottomSheetNavigationService bottomSheetNavigationService) : ObservableObject, IQueryAttributable
{
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query is null || query.Count == 0)
            return;

        if (query.TryGetValue("username", out object? username))
        {
            Oldusername = username.ToString();
            Newusername = username.ToString();
        }

        query.Clear();
    }

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
    string? _oldusername;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
    string? _newusername;


    bool SaveCanExecute()
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
    async Task Save()
    {
        await bottomSheetNavigationService.GoBackAsync(new BottomSheetNavigationParameters()
        {
            ["newusername"] = Newusername!,
        });
    }

    [RelayCommand]
    async Task GoBack()
    {
        await bottomSheetNavigationService.GoBackAsync();
    }
}