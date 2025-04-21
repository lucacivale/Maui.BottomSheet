using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Plugin.Maui.BottomSheet.Navigation;

namespace Plugin.Maui.BottomSheet.Sample.ViewModels;

public partial class ShellPageViewModel (IBottomSheetNavigationService bottomSheetNavigationService) : ObservableObject, IQueryAttributable
{
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query is null || query.Count == 0)
            return;

        if (query.TryGetValue("newusername", out object? newusername))
        {
            Username = newusername.ToString();
        }

        query.Clear();
    }

    [ObservableProperty]
    string? _username = "John, Smith";

    [RelayCommand]
    async Task OpenCustomBottomSheet()
    {
        await bottomSheetNavigationService.NavigateToAsync("CustomBottomSheet", new BottomSheetNavigationParameters()
        {
            ["username"] = Username!,
        });
    }
}