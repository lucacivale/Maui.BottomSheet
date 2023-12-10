namespace Maui.BottomSheet.Navigation;
public interface IBottomSheetNavigationService
{
    void NavigateTo(IBottomSheet bottomSheet, Type viewModelType, IBottomSheetNavigationParameters? parameters = null);
    void NavigateTo(IBottomSheet bottomSheet, IBottomSheetNavigationParameters? parameters = null);
    void NavigateTo<TViewModel>(IBottomSheet bottomSheet, IBottomSheetNavigationParameters? parameters = null);
    void NavigateTo<TBottomSheet>() where TBottomSheet : IBottomSheet;
    void NavigateTo<TBottomSheet, TViewModel>(IBottomSheetNavigationParameters? parameters = null) where TBottomSheet : IBottomSheet;
    void GoBack(IBottomSheetNavigationParameters? parameters = null);
    void ClearBottomSheetStack();
}