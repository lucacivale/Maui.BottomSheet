using Plugin.BottomSheet.Tests.Maui.Unit.Application.Mocks.ViewModels;
using Plugin.Maui.BottomSheet.Navigation;

namespace Plugin.BottomSheet.Tests.Maui.Unit.Application.Mocks;

public class NavigationAwareContentPage : ContentPage, INavigationAware
{
    public NavigationAwareContentPage(NavigationAwareViewModel viewModel)
    {
        BindingContext = viewModel;
    }

    public bool NavigatedFromCalled { get; set; }
        
    public bool NavigatedToCalled { get; set; }

    public IBottomSheetNavigationParameters? Parameters { get; private set; }

    public void OnNavigatedFrom(IBottomSheetNavigationParameters parameters)
    {
        NavigatedFromCalled = true;
        parameters.Add($"{parameters.Count + 1}", "true");
    }

    public void OnNavigatedTo(IBottomSheetNavigationParameters parameters)
    {
        NavigatedToCalled = true;
        parameters.Add($"{parameters.Count + 1}", "true");
    }
}