using Maui.BottomSheet.Navigation;

namespace Maui.BottomSheet.SheetBuilder;
public interface IBottomSheetBuilder
{
    IBottomSheetBuilder FromView<TView>() where TView : View, new();
    IBottomSheetBuilder FromContentPage<TContentPage>() where TContentPage : ContentPage;
    IBottomSheetBuilder ConfigureBottomSheet(Action<IBottomSheet> configureBottomSheet);
    IBottomSheetBuilder WithParameters(IBottomSheetNavigationParameters parameters);
    IBottomSheetBuilder WireTo<TViewModel>();
    IBottomSheetBuilder TryAutoWire(bool tryAutoWire = true);
    void Open();
}
