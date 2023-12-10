using Maui.BottomSheet.Navigation;

namespace Maui.BottomSheet.SheetBuilder;

public class BottomSheetBuilder : IBottomSheetBuilder
{
    private readonly IBottomSheetNavigationService _navigationService;
    private readonly IServiceProvider _serviceProvider;

    private IBottomSheet? _bottomSheet;
    private IBottomSheetNavigationParameters? _navigationParameters;
    private Type? _viewModelType;
    private bool _tryAutoWire;
    private ContentPage? _targetContentPage;

    public BottomSheetBuilder(IBottomSheetNavigationService navigationService, IServiceProvider serviceProvider)
    {
        _navigationService = navigationService;
        _serviceProvider = serviceProvider;
    }

    public IBottomSheetBuilder FromView<TView>() where TView : View, new()
    {
        TView view = Activator.CreateInstance<TView>();

        _bottomSheet = view.ToBottomSheet();

        return this;
    }

    public IBottomSheetBuilder FromContentPage<TContentPage>() where TContentPage : ContentPage
    {
        _targetContentPage = _serviceProvider.Resolve<TContentPage>();

        _bottomSheet = _targetContentPage.ToBottomSheet();

        return this;
    }

    public IBottomSheetBuilder ConfigureBottomSheet(Action<IBottomSheet> configureBottomSheet)
    {
        _ = _bottomSheet ?? throw new NullReferenceException(nameof(BottomSheet));

        configureBottomSheet?.Invoke(_bottomSheet);

        return this;
    }

    public IBottomSheetBuilder WithParameters(IBottomSheetNavigationParameters parameters)
    {
        _navigationParameters = parameters;

        return this;
    }

    public IBottomSheetBuilder WireTo<TViewModel>()
    {
        _viewModelType = typeof(TViewModel);

        return this;
    }

    public IBottomSheetBuilder TryAutoWire(bool tryAutoWire = true)
    {
        _tryAutoWire = tryAutoWire;

        return this;
    }

    public void Open()
    {
        _ = _bottomSheet ?? throw new NullReferenceException(nameof(BottomSheet));

        if (_tryAutoWire
            && _targetContentPage?.BindingContext is not null)
        {
            _viewModelType = _targetContentPage.BindingContext.GetType();
        }

        if (_viewModelType is not null)
        {
            _navigationService.NavigateTo(_bottomSheet, _viewModelType, _navigationParameters);

        }
        else
        {
            _navigationService.NavigateTo(_bottomSheet, _navigationParameters);
        }
    }
}
