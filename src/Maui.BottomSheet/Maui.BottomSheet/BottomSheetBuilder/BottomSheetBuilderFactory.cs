using Maui.BottomSheet.Navigation;

namespace Maui.BottomSheet.SheetBuilder;
public class BottomSheetBuilderFactory : IBottomSheetBuilderFactory
{
    private readonly IBottomSheetNavigationService _navigationService;
    private readonly IServiceProvider _serviceProvider;

    public BottomSheetBuilderFactory(IBottomSheetNavigationService navigationService, IServiceProvider serviceProvider)
    {
        _navigationService = navigationService;
        _serviceProvider = serviceProvider;
    }
    public IBottomSheetBuilder Create()
    {
        return new BottomSheetBuilder(_navigationService, _serviceProvider);
    }
}
