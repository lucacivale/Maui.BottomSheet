using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Plugin.Maui.BottomSheet.Navigation;

/// <inheritdoc />
public sealed class BottomSheetNavigationService : IBottomSheetNavigationService
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

    /// <summary>
    /// Gets BottomSheet ViewModel mapping.
    /// </summary>
    internal static Dictionary<string, Type> BottomSheetToViewModelMapping { get; } = [];

    /// <inheritdoc/>
    public void NavigateTo(IBottomSheet bottomSheet, object? viewModel = null, IBottomSheetNavigationParameters? parameters = null, Action<IBottomSheet>? configure = null)
    {
        Dispatch(() =>
        {
            PrepareBottomSheetForNavigation(bottomSheet, viewModel, configure);

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
    public Task NavigateToAsync(IBottomSheet bottomSheet, object? viewModel = null, IBottomSheetNavigationParameters? parameters = null, Action<IBottomSheet>? configure = null)
    {
        return DispatchAsync(async () =>
        {
            parameters ??= BottomSheetNavigationParameters.Empty();

            if (_bottomSheetStack.IsEmpty == false
                && await MvvmHelpers.ConfirmNavigationAsync(_bottomSheetStack.Current, parameters).ConfigureAwait(true) == false)
            {
                return;
            }

            PrepareBottomSheetForNavigation(bottomSheet, viewModel, configure);

            if (bottomSheet.BindingContext is IQueryAttributable queryAttributable)
            {
                ApplyAttributes(queryAttributable, parameters);
            }

            if (_bottomSheetStack.IsEmpty)
            {
                MvvmHelpers.OnNavigatedFrom(bottomSheet.GetPageParent(), parameters);
            }
            else
            {
                MvvmHelpers.OnNavigatedFrom(_bottomSheetStack.Current, parameters);
            }

            MvvmHelpers.OnNavigatedTo(bottomSheet, parameters);

            if (bottomSheet.Handler is Handlers.BottomSheetHandler bottomSheetHandler)
            {
                await bottomSheetHandler.OpenAsync().ConfigureAwait(true);
                _bottomSheetStack.Add(bottomSheet);
                _bottomSheetStack.Current.IsOpen = true;
            }
        });
    }

    /// <inheritdoc/>
    public void GoBack(IBottomSheetNavigationParameters? parameters = null)
    {
        Dispatch(() => DoGoBack(parameters));
    }

    /// <inheritdoc/>
    public Task GoBackAsync(IBottomSheetNavigationParameters? parameters = null)
    {
        return DispatchAsync(() => DoGoBackAsync(parameters));
    }

    /// <inheritdoc/>
    public void ClearBottomSheetStack()
    {
        Dispatch(() =>
        {
            while (!_bottomSheetStack.IsEmpty)
            {
                DoGoBack();
            }
        });
    }

    /// <inheritdoc/>
    public Task ClearBottomSheetStackAsync()
    {
        return DispatchAsync(async () =>
        {
            while (!_bottomSheetStack.IsEmpty)
            {
                await DoGoBackAsync().ConfigureAwait(true);
            }
        });
    }

    public IReadOnlyCollection<IBottomSheet> NavigationStack() => _bottomSheetStack;

    private static void ApplyAttributes(IQueryAttributable? attributable, IBottomSheetNavigationParameters? parameters)
    {
        if (parameters is not null
            && attributable is not null)
        {
            attributable.ApplyQueryAttributes(parameters);
        }
    }

    private void ApplyGoBackParameters(IBottomSheet bottomSheet, IBottomSheetNavigationParameters? parameters)
    {
        IQueryAttributable? queryAttributable = null;

        if (_bottomSheetStack.IsEmpty
            && bottomSheet.GetPageParent()?.BindingContext is IQueryAttributable parentBindingContext)
        {
            queryAttributable = parentBindingContext;
        }
        else if (_bottomSheetStack is { IsEmpty: false, Current.BindingContext: IQueryAttributable sheetBindingContext })
        {
            queryAttributable = sheetBindingContext;
        }

        ApplyAttributes(queryAttributable, parameters);
    }

    [SuppressMessage("Usage", "CA1826:Use property instead of Linq Enumerable method", Justification = "Improved readability.")]
    private void PrepareBottomSheetForNavigation(IBottomSheet bottomSheet, object? viewModel = null, Action<IBottomSheet>? configure = null)
    {
        var page = Application.Current?.Windows.LastOrDefault()?.Page ?? throw new InvalidOperationException("Application.Current?.Windows.LastOrDefault()?.Page cannot be null.");
        var mauiContext = page.Handler?.MauiContext ?? throw new InvalidOperationException("Page.Handler?.MauiContext cannot be null.");

        bottomSheet.Handler = new Handlers.BottomSheetHandler(mauiContext);
        bottomSheet.Parent = page;
        bottomSheet.Closed += OnClose;

        if (viewModel is not null)
        {
            bottomSheet.BindingContext = viewModel;
        }

        configure?.Invoke(bottomSheet);
    }

    private void DoGoBack(IBottomSheetNavigationParameters? parameters = null)
    {
        if (_bottomSheetStack.IsEmpty)
        {
            return;
        }

        _bottomSheetStack.Current.Closed -= OnClose;
        _bottomSheetStack.Current.IsOpen = false;
        _bottomSheetStack.Current.Handler?.DisconnectHandler();
        var bottomSheet = _bottomSheetStack.Remove();

        ApplyGoBackParameters(bottomSheet, parameters);
    }

    private async Task DoGoBackAsync(IBottomSheetNavigationParameters? parameters = null)
    {
        if (_bottomSheetStack.IsEmpty)
        {
            return;
        }

        parameters ??= BottomSheetNavigationParameters.Empty();

        if (await MvvmHelpers.ConfirmNavigationAsync(_bottomSheetStack.Current, parameters).ConfigureAwait(true) == false)
        {
            return;
        }

        _bottomSheetStack.Current.Closed -= OnClose;

        if (_bottomSheetStack.Current.Handler is Handlers.BottomSheetHandler bottomSheetHandler)
        {
            await bottomSheetHandler.CloseAsync().ConfigureAwait(true);
        }

        _bottomSheetStack.Current.Handler?.DisconnectHandler();
        var bottomSheet = _bottomSheetStack.Remove();

        MvvmHelpers.OnNavigatedFrom(bottomSheet, parameters);

        if (_bottomSheetStack.IsEmpty)
        {
            MvvmHelpers.OnNavigatedTo(bottomSheet.GetPageParent(), parameters);
        }
        else
        {
            MvvmHelpers.OnNavigatedTo(_bottomSheetStack.Current, parameters);
        }

        ApplyGoBackParameters(bottomSheet, parameters);
    }

    [SuppressMessage("Design", "CA1031: Do not catch general exception types", Justification = "Catch all exceptions to prevent crash.")]
    [SuppressMessage("Usage", "VSTHRD100: Avoid async void methods, because any exceptions not handled by the method will crash the process", Justification = "Event.")]
    private async void OnClose(object? sender, EventArgs e)
    {
        try
        {
            await DispatchAsync(() => DoGoBackAsync()).ConfigureAwait(false);
        }
        catch
        {
            Trace.TraceError("Invoking GoBackAsync failed.");
        }
    }

    [SuppressMessage("Usage", "CA1826:Use property instead of Linq Enumerable method", Justification = "Validated.")]
    private IDispatcher GetDispatcher()
    {
        IDispatcher? dispatcher = _bottomSheetStack is { IsEmpty: false, Current: BindableObject bindable } ? bindable.Dispatcher : Application.Current?.Windows.LastOrDefault()?.Page?.Dispatcher;

        ArgumentNullException.ThrowIfNull(dispatcher);

        return dispatcher;
    }

    private void Dispatch(Action action)
    {
        GetDispatcher().Dispatch(action);
    }

    private Task DispatchAsync(Func<Task> action)
    {
        return GetDispatcher().DispatchAsync(action);
    }
}
