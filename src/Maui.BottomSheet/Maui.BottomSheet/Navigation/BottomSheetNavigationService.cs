namespace Maui.BottomSheet.Navigation;

public class BottomSheetNavigationService(IServiceProvider serviceProvider) : IBottomSheetNavigationService
{
    private readonly BottomSheetStack _bottomSheetStack = new();
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    public void NavigateTo<TBottomSheet>() where TBottomSheet : IBottomSheet
    {
        NavigateTo(_serviceProvider.Resolve<IBottomSheet, TBottomSheet>());
    }

    public void NavigateTo<TBottomSheet, TViewModel>(IBottomSheetNavigationParameters? parameters = null) where TBottomSheet : IBottomSheet
    {
        var bottomSheet = _serviceProvider.Resolve<IBottomSheet, TBottomSheet>();
        var bottomSheetViewModel = _serviceProvider.Resolve<TViewModel>();

        if (bottomSheetViewModel is not null)
        {
            bottomSheet.BindingContext = bottomSheetViewModel;
        }

        NavigateTo(bottomSheet, parameters);
    }

    private void NavigateTo(IBottomSheet bottomSheet, IBottomSheetNavigationParameters? parameters = null)
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
        ArgumentNullException.ThrowIfNull(Application.Current?.Handler.MauiContext);

        //Refactor as soon as #1718 is closed or Application.Current.FindMauiContext() is exposed.
        _bottomSheetStack.Current.Handler = new BottomSheetHandler(Application.Current.Handler.MauiContext);
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