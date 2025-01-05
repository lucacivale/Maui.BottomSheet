namespace Plugin.Maui.BottomSheet.Navigation;

/// <inheritdoc />
internal sealed class BottomSheetNavigationService : IBottomSheetNavigationService
{
    private readonly BottomSheetStack _bottomSheetStack = [];

    /// <summary>
    /// Initializes a new instance of the <see cref="BottomSheetNavigationService"/> class.
    /// </summary>
    /// <param name="serviceProvider">Service provider.</param>
    public BottomSheetNavigationService(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
    }

    /// <inheritdoc/>
    public IServiceProvider ServiceProvider { get; }

    /// <inheritdoc/>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA1826:Use property instead of Linq Enumerable method", Justification = "Validated.")]
    public void NavigateTo(IBottomSheet bottomSheet, object? viewModel = null, IBottomSheetNavigationParameters? parameters = null, Action<IBottomSheet>? configure = null)
    {
        var page = Application.Current?.Windows.LastOrDefault()?.Page ?? throw new InvalidOperationException("Application.Current?.Windows.LastOrDefault()?.Page cannot be null.");
        var mauiContext = page.Handler?.MauiContext ?? throw new InvalidOperationException("Page.Handler?.MauiContext cannot be null.");

        Dispatch(() =>
        {
            bottomSheet.Handler = new Handlers.BottomSheetHandler(mauiContext);
            bottomSheet.Parent = page;
            bottomSheet.Closed += OnClose;

            if (viewModel is not null)
            {
                bottomSheet.BindingContext = viewModel;
            }

            configure?.Invoke(bottomSheet);

            bottomSheet.IsOpen = true;

            if (bottomSheet.BindingContext is IQueryAttributable queryAttributable
                && parameters is not null)
            {
                queryAttributable.ApplyQueryAttributes(parameters);
            }

            _bottomSheetStack.Add(bottomSheet);
        });
    }

    /// <inheritdoc/>
    public void GoBack(IBottomSheetNavigationParameters? parameters = null)
    {
        Dispatch(() =>
        {
            var parent = _bottomSheetStack.Current.Parent;

            _bottomSheetStack.Current.Closed -= OnClose;
            _bottomSheetStack.Current.IsOpen = false;
            _bottomSheetStack.Current.Handler?.DisconnectHandler();
            _bottomSheetStack.Remove();

            IQueryAttributable? queryAttributable = null;

            if (_bottomSheetStack.IsEmpty
                && parent.BindingContext is IQueryAttributable parentBindingContext)
            {
                queryAttributable = parentBindingContext;
            }
            else if (_bottomSheetStack is { IsEmpty: false, Current.BindingContext: IQueryAttributable sheetBindingContext })
            {
                queryAttributable = sheetBindingContext;
            }

            if (parameters is not null
                && queryAttributable is not null)
            {
                queryAttributable.ApplyQueryAttributes(parameters);
            }
        });
    }

    /// <inheritdoc/>
    public void ClearBottomSheetStack()
    {
        Dispatch(() =>
        {
            while (!_bottomSheetStack.IsEmpty)
            {
                GoBack();
            }
        });
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA1826:Use property instead of Linq Enumerable method", Justification = "Validated.")]
    private static void Dispatch(Action action)
    {
        var page = Application.Current?.Windows.LastOrDefault()?.Page ?? throw new InvalidOperationException("Application.Current?.Windows.LastOrDefault()?.Page cannot be null.");

        page.Dispatcher.Dispatch(action);
    }

    private void OnClose(object? sender, EventArgs e)
    {
        GoBack();
    }
}