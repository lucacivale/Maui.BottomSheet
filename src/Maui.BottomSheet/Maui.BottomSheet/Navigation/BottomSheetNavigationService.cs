namespace Maui.BottomSheet.Navigation;

public class BottomSheetNavigationService : IBottomSheetNavigationService
{
    private readonly BottomSheetStack _bottomSheetStack = new();
    private readonly IServiceProvider _serviceProvider;

    public BottomSheetNavigationService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    public void NavigateTo<TBottomSheet>() where TBottomSheet : IBottomSheet
    {
        NavigateTo(_serviceProvider.Resolve<IBottomSheet, TBottomSheet>());
    }

    public void NavigateTo<TBottomSheet, TViewModel>(IBottomSheetNavigationParameters? parameters = null) where TBottomSheet : IBottomSheet
    {
        var bottomSheet = _serviceProvider.Resolve<IBottomSheet, TBottomSheet>();

        WireViewModel<TViewModel>(bottomSheet);

        NavigateTo(bottomSheet, parameters);
    }

    public void NavigateTo<TViewModel>(IBottomSheet bottomSheet, IBottomSheetNavigationParameters? parameters = null)
    {
        WireViewModel<TViewModel>(bottomSheet);

        NavigateTo(bottomSheet, parameters);
    }

    public void NavigateTo(IBottomSheet bottomSheet, Type viewModelType, IBottomSheetNavigationParameters? parameters = null)
    {
        WireViewModel(bottomSheet, viewModelType);

        NavigateTo(bottomSheet, parameters);
    }

    public void NavigateTo(IBottomSheet bottomSheet, IBottomSheetNavigationParameters? parameters = null)
    {
        AddBottomSheetToStack(bottomSheet);
        PrepareSheetForNavigation();
        
        OpenBottomSheet();

        ApplyNavigationParameters(parameters);
    }

    public void GoBack(IBottomSheetNavigationParameters? parameters = null)
    {
        CloseBottomSheet();

        IBottomSheet bottomSheet = RemoveBottomSheetFromStack();
        bottomSheet.Handler?.DisconnectHandler();

        ApplyNavigationParameters(parameters);
    }

    public void ClearBottomSheetStack()
    {
        while (_bottomSheetStack.IsEmpty == false)
        {
            GoBack();
        }
    }

    private void WireViewModel<TViewModel>(IBottomSheet bottomSheet)
    {
        ApplyViewModel(bottomSheet, _serviceProvider.Resolve<TViewModel>());
    }

    private void WireViewModel(IBottomSheet bottomSheet, Type viewModelType)
    {
        ApplyViewModel(bottomSheet,_serviceProvider.GetService(viewModelType));
    }

    private void AddBottomSheetToStack(IBottomSheet bottomSheet)
    {
        _bottomSheetStack.Add(bottomSheet);
    }

    private IBottomSheet RemoveBottomSheetFromStack()
    {
        return _bottomSheetStack.Remove();
    }

    private void OpenBottomSheet()
    {
        SetBottomSheetIsOpen(true);

        _bottomSheetStack.Current.Closed += OnClose;
    }

    private void OnClose(object? sender, EventArgs e)
    {
        GoBack();
    }

    private void CloseBottomSheet()
    {
        _bottomSheetStack.Current.Closed -= OnClose;

        SetBottomSheetIsOpen(false);
    }

    private void SetBottomSheetIsOpen(bool isOpen)
    {
        _bottomSheetStack.Current.IsOpen = isOpen;
    }

    private void PrepareSheetForNavigation()
    {
        ArgumentNullException.ThrowIfNull(Application.Current?.MainPage?.Handler?.MauiContext);

        //Refactor as soon as #1718 is closed or Element.FindMauiContext() is exposed.
        _bottomSheetStack.Current.Handler = new BottomSheetHandler(Application.Current.MainPage.Handler.MauiContext);
    }

    private static void ApplyViewModel(IBottomSheet bottomSheet, object? viewModel)
    {
        if (viewModel is not null)
        {
            bottomSheet.BindingContext = viewModel;
        }
    }
    private void ApplyNavigationParameters(IBottomSheetNavigationParameters? parameters)
    {
        if (parameters is null)
        {
            return;
        }

        if (_bottomSheetStack.Current.BindingContext is IQueryAttributable queryAttributable)
        {
            queryAttributable.ApplyQueryAttributes(parameters);
        }
    }
}