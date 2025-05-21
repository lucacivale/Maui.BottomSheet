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
    public Task<INavigationResult> NavigateToAsync(IBottomSheet bottomSheet, object? viewModel = null, IBottomSheetNavigationParameters? parameters = null, Action<IBottomSheet>? configure = null)
    {
        return DispatchAsync(() => DoNavigateAsync(bottomSheet, viewModel, parameters, configure));
    }

    /// <inheritdoc/>
    public Task<INavigationResult> GoBackAsync(IBottomSheetNavigationParameters? parameters = null)
    {
        return DispatchAsync(() => DoGoBackAsync(parameters));
    }

    /// <inheritdoc/>
    public Task<IEnumerable<INavigationResult>> ClearBottomSheetStackAsync()
    {
        return DispatchAsync<IEnumerable<INavigationResult>>(async () =>
        {
            List<INavigationResult> results = new();

            while (!_bottomSheetStack.IsEmpty)
            {
                results.Add(await DoGoBackAsync().ConfigureAwait(true));
            }

            return results;
        });
    }

    public IReadOnlyCollection<IBottomSheet> NavigationStack() => _bottomSheetStack;

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

    [SuppressMessage("Design", "CA1031: Do not catch general exception types", Justification = "Catch all exceptions.")]
    private async Task<INavigationResult> DoNavigateAsync(IBottomSheet bottomSheet, object? viewModel = null, IBottomSheetNavigationParameters? parameters = null, Action<IBottomSheet>? configure = null)
    {
        NavigationResult result = new();

        try
        {
            parameters ??= BottomSheetNavigationParameters.Empty();

            if (_bottomSheetStack.IsEmpty == false
                && await MvvmHelpers.ConfirmNavigationAsync(_bottomSheetStack.Current, parameters).ConfigureAwait(true) == false)
            {
                result.Cancelled = true;
            }
            else
            {
                PrepareBottomSheetForNavigation(bottomSheet, viewModel, configure);

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
            }
        }
        catch (Exception e)
        {
            result.Exception = e;
            result.Success = false;
        }

        return result;
    }

    [SuppressMessage("Design", "CA1031: Do not catch general exception types", Justification = "Catch all exceptions.")]
    private async Task<INavigationResult> DoGoBackAsync(IBottomSheetNavigationParameters? parameters = null)
    {
        NavigationResult result = new();

        if (_bottomSheetStack.IsEmpty)
        {
            result.Success = false;
            return result;
        }

        try
        {
            parameters ??= BottomSheetNavigationParameters.Empty();

            if (await MvvmHelpers.ConfirmNavigationAsync(_bottomSheetStack.Current, parameters).ConfigureAwait(true) == false)
            {
                result.Cancelled = true;
            }
            else
            {
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
            }
        }
        catch (Exception e)
        {
            result.Exception = e;
            result.Success = false;
        }

        return result;
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

    private Task<TReturn> DispatchAsync<TReturn>(Func<Task<TReturn>> action)
    {
        return GetDispatcher().DispatchAsync(action);
    }
}
