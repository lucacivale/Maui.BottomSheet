using Plugin.Maui.BottomSheet.Navigation;

namespace Plugin.BottomSheet.Tests.Maui.Unit.Application.Mocks.ViewModels;

public class NavigationAwareViewModel : Plugin.Maui.BottomSheet.BottomSheet, IConfirmNavigation, IConfirmNavigationAsync, INavigationAware
{
    public bool NavigatedFromCalled { get; set; }
    
    public bool NavigatedToCalled { get; set; }
    public bool CanNavigateCalled { get; set; }
    public bool CanNavigateAsyncCalled { get; set; }
    
    public bool NavigationAllowed { get; set; } = true;

    public IBottomSheetNavigationParameters? Parameters { get; private set; }

    public bool CanNavigate(IBottomSheetNavigationParameters parameters)
    {
        Parameters = parameters;
        CanNavigateCalled = true;
        parameters.Add($"{parameters.Count + 1}", "true");

        return NavigationAllowed;
    }

    public Task<bool> CanNavigateAsync(IBottomSheetNavigationParameters parameters)
    {
        Parameters = parameters;
        CanNavigateAsyncCalled = true;
        parameters.Add($"{parameters.Count + 1}", "true");

        return Task.FromResult(NavigationAllowed);
    }

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