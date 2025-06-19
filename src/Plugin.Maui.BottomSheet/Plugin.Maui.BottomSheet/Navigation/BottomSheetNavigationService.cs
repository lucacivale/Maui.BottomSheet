using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Plugin.Maui.BottomSheet.Navigation;

/// <summary>
/// Provides navigation services for BottomSheet controls, handling navigation stack management and view model integration.
/// </summary>
/// <inheritdoc />
public sealed class BottomSheetNavigationService : IBottomSheetNavigationService
{
    /// <summary>
    /// Maintains the stack of currently opened bottom sheets.
    /// </summary>
    private readonly BottomSheetStack _bottomSheetStack = [];

    /// <summary>
    /// Initializes a new instance of the <see cref="BottomSheetNavigationService"/> class.
    /// </summary>
    /// <param name="serviceProvider">The service provider used for dependency injection and view model resolution.</param>
    public BottomSheetNavigationService(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
    }

    /// <inheritdoc/>
    public IServiceProvider ServiceProvider { get; }

    /// <summary>
    /// Gets the mapping between BottomSheet names and their corresponding ViewModel types.
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

    /// <inheritdoc/>
    public IReadOnlyCollection<IBottomSheet> NavigationStack() => _bottomSheetStack;

    /// <summary>
    /// Prepares a bottom sheet for navigation by setting up its handler, parent, and event handlers.
    /// </summary>
    /// <param name="bottomSheet">The bottom sheet to prepare.</param>
    /// <param name="viewModel">Optional view model to set as the binding context.</param>
    /// <param name="configure">Optional action to further configure the bottom sheet.</param>
    /// <exception cref="InvalidOperationException">Thrown when required MAUI components are not available.</exception>
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

    /// <summary>
    /// Performs the actual navigation to a new bottom sheet.
    /// </summary>
    /// <param name="bottomSheet">The bottom sheet to navigate to.</param>
    /// <param name="viewModel">Optional view model for the bottom sheet.</param>
    /// <param name="parameters">Optional navigation parameters.</param>
    /// <param name="configure">Optional configuration action.</param>
    /// <returns>A navigation result indicating success or failure.</returns>
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

    /// <summary>
    /// Performs the actual navigation back operation, closing the current bottom sheet.
    /// </summary>
    /// <param name="parameters">Optional navigation parameters.</param>
    /// <returns>A navigation result indicating success or failure.</returns>
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

    /// <summary>
    /// Handles the bottom sheet's close event by initiating a navigation back operation.
    /// </summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="e">The event arguments.</param>
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

    /// <summary>
    /// Gets the dispatcher for the current UI context.
    /// </summary>
    /// <returns>The current dispatcher.</returns>
    /// <exception cref="ArgumentNullException">Thrown when no valid dispatcher is found.</exception>
    [SuppressMessage("Usage", "CA1826:Use property instead of Linq Enumerable method", Justification = "Validated.")]
    private IDispatcher GetDispatcher()
    {
        IDispatcher? dispatcher = _bottomSheetStack is { IsEmpty: false, Current: BindableObject bindable } ? bindable.Dispatcher : Application.Current?.Windows.LastOrDefault()?.Page?.Dispatcher;

        ArgumentNullException.ThrowIfNull(dispatcher);

        return dispatcher;
    }

    /// <summary>
    /// Dispatches an asynchronous operation to the UI thread.
    /// </summary>
    /// <typeparam name="TReturn">The type of value returned by the operation.</typeparam>
    /// <param name="action">The action to dispatch.</param>
    /// <returns>A task representing the dispatched operation.</returns>
    private Task<TReturn> DispatchAsync<TReturn>(Func<Task<TReturn>> action)
    {
        return GetDispatcher().DispatchAsync(action);
    }
}