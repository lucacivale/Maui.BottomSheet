namespace Maui.BottomSheet.Navigation;
public interface IBottomSheetNavigationService
{
    public void NavigateTo<TBottomSheet>() where TBottomSheet : IBottomSheet;
    public void NavigateTo<TBottomSheet, TViewModel>(IBottomSheetNavigationParameters? parameters = null) where TBottomSheet : IBottomSheet;
    public void GoBack(IBottomSheetNavigationParameters? parameters = null);
    public void ClearBottomSheetStack();
}