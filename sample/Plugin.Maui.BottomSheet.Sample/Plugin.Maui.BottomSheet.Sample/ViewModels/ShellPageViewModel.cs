using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Plugin.Maui.BottomSheet.Navigation;

namespace Plugin.Maui.BottomSheet.Sample.ViewModels;

public partial class ShellPageViewModel (IBottomSheetNavigationService bottomSheetNavigationService) : ObservableObject, INavigationAware
{
    [ObservableProperty]
    public partial string? Username { get; set; } = "John, Smith";

    [RelayCommand]
    public async Task OpenCustomBottomSheet()
    {
        await bottomSheetNavigationService.NavigateToAsync("CustomBottomSheet", new BottomSheetNavigationParameters()
        {
            ["username"] = Username!,
        });
    }

    public void OnNavigatedFrom(IBottomSheetNavigationParameters parameters)
    {
        Console.WriteLine($"OnNavigatedFrom ShellPageViewModel");
    }

    public void OnNavigatedTo(IBottomSheetNavigationParameters parameters)
    {
        if (parameters.TryGetValue("newusername", out object? newusername))
        {
            Username = newusername.ToString();
        }
        Console.WriteLine($"OnNavigatedTo ShellPageViewModel");
    }
}