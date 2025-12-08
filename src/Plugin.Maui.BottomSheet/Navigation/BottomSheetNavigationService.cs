using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Plugin.Maui.BottomSheet.Navigation;

/// <summary>
/// Manages navigation operations for BottomSheet components, facilitating transitions and state management within the navigation stack.
/// </summary>
/// <inheritdoc />
public sealed class BottomSheetNavigationService : IBottomSheetNavigationService
{
    private readonly BottomSheetStack _bottomSheetStack = [];

    /// <summary>
    /// Initializes a new instance of the <see cref="BottomSheetNavigationService"/> class.
    /// Provides functionality for navigating using a bottom sheet design pattern.
    /// </summary>
    /// <param name="serviceProvider">The service provider used to resolve dependencies and manage view models during navigation.</param>
    public BottomSheetNavigationService(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
    }

    /// <summary>
    /// Gets an instance of the service provider used to resolve dependencies
    /// within the BottomSheet navigation service.
    /// </summary>
    public IServiceProvider ServiceProvider { get; }

    /// <summary>
    /// Gets a dictionary that maps bottom sheet names to their associated view model types.
    /// This mapping is used to resolve and associate view models with bottom sheets at runtime,
    /// facilitating dynamic bottom sheet navigation and dependency injection.
    /// </summary>
    internal static Dictionary<string, Type> BottomSheetToViewModelMapping { get; } = [];

    /// <summary>
    /// Navigates to a specified bottom sheet, optionally initializing it with a view model, navigation parameters, and configuration.
    /// </summary>
    /// <param name="bottomSheet">The bottom sheet instance to navigate to.</param>
    /// <param name="viewModel">The view model to associate with the bottom sheet. Optional.</param>
    /// <param name="parameters">The navigation parameters to pass to the bottom sheet. Optional.</param>
    /// <param name="configure">An optional action to further configure the bottom sheet before navigation.</param>
    /// <returns>A task representing the asynchronous navigation operation that results in an <see cref="INavigationResult"/>.</returns>
    public Task<INavigationResult> NavigateToAsync(IBottomSheet bottomSheet, object? viewModel = null, IBottomSheetNavigationParameters? parameters = null, Action<IBottomSheet>? configure = null)
    {
        return DispatchAsync(() => DoNavigateAsync(bottomSheet, viewModel, parameters, configure));
    }

    /// <summary>
    /// Navigates back to the previous view in the bottom sheet navigation stack asynchronously.
    /// </summary>
    /// <param name="parameters">
    /// Optional parameters that may be used during the navigation process. Can be null if no parameters are required.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous navigation operation. The task result contains an <see cref="INavigationResult"/> indicating the outcome of the navigation.
    /// </returns>
    public Task<INavigationResult> GoBackAsync(IBottomSheetNavigationParameters? parameters = null)
    {
        return DispatchAsync(() => DoGoBackAsync(parameters));
    }

    /// <summary>
    /// Clears the bottom sheet navigation stack by sequentially removing all bottom sheets
    /// and navigating backward until the stack is empty.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.
    /// The task result contains a collection of <see cref="INavigationResult"/> indicating
    /// the results of each backward navigation operation.</returns>
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

    /// <summary>
    /// Gets the current stack of navigation for the bottom sheets.
    /// </summary>
    /// <returns>
    /// A read-only collection of bottom sheets representing the navigation stack.
    /// </returns>
    public IReadOnlyCollection<IBottomSheet> NavigationStack() => _bottomSheetStack;

    /// <summary>
    /// Prepares a bottom sheet for navigation by setting its handler, parent, and event listeners, and optionally configuring it.
    /// </summary>
    /// <param name="bottomSheet">The bottom sheet to be prepared for navigation.</param>
    /// <param name="viewModel">An optional view model to set as the binding context of the bottom sheet.</param>
    /// <param name="configure">An optional action to perform additional configuration on the bottom sheet.</param>
    /// <exception cref="InvalidOperationException">Thrown if the necessary MAUI components cannot be resolved.</exception>
    [SuppressMessage("Usage", "CA1826:Use property instead of Linq Enumerable method", Justification = "Improved readability.")]
    private void PrepareBottomSheetForNavigation(IBottomSheet bottomSheet, object? viewModel = null, Action<IBottomSheet>? configure = null)
    {
        Page page = Application.Current?.Windows.LastOrDefault()?.Page ?? throw new InvalidOperationException("Application.Current?.Windows.LastOrDefault()?.Page cannot be null.");
        IMauiContext mauiContext = page.Handler?.MauiContext ?? throw new InvalidOperationException("Page.Handler?.MauiContext cannot be null.");

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
    /// Performs the actual navigation to a specified bottom sheet, handling optional view models, parameters, and configurations.
    /// </summary>
    /// <param name="bottomSheet">The bottom sheet instance to navigate to.</param>
    /// <param name="viewModel">An optional view model to bind to the bottom sheet.</param>
    /// <param name="parameters">An optional set of navigation parameters provided during navigation.</param>
    /// <param name="configure">An optional action to configure the bottom sheet before display.</param>
    /// <returns>A task that represents the asynchronous navigation operation, providing a navigation result indicating success or failure.</returns>
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

                    TaskCompletionSource taskCompletionSource = new();
                    EventHandler @event = null!;

                    @event = (s, e) =>
                    {
                        ((View)s!).Loaded -= @event;
                        _ = taskCompletionSource.TrySetResult();
                    };

                    bottomSheet.ContainerView.Loaded += @event;

                    await taskCompletionSource.Task.ConfigureAwait(true);
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
    /// <returns>A task that represents the asynchronous operation. The task result contains a navigation result indicating success or failure.</returns>
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
                IBottomSheet bottomSheet = _bottomSheetStack.Remove();

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
    /// Handles the bottom sheet's close event and triggers the navigation back operation.
    /// </summary>
    /// <param name="sender">The source of the close event.</param>
    /// <param name="e">The event data associated with the close event.</param>
    [SuppressMessage("Design", "CA1031: Do not catch general exception types", Justification = "Catch all exceptions to prevent crash.")]
    [SuppressMessage("Usage", "VSTHRD100: Avoid async void methods, because any exceptions not handled by the method will crash the process", Justification = "Event.")]
    private async void OnClose(object? sender, EventArgs e)
    {
        try
        {
            await DispatchAsync(() => DoGoBackAsync()).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            Trace.TraceError("Invoking GoBackAsync failed: {0}", ex);
        }
    }

    /// <summary>
    /// Retrieves the dispatcher associated with the current UI context.
    /// </summary>
    /// <returns>The dispatcher for the current context.</returns>
    /// <exception cref="ArgumentNullException">Thrown when a valid dispatcher cannot be determined.</exception>
    [SuppressMessage("Usage", "CA1826:Use property instead of Linq Enumerable method", Justification = "Validated.")]
    private IDispatcher GetDispatcher()
    {
        IDispatcher? dispatcher = _bottomSheetStack is { IsEmpty: false, Current: BindableObject bindable } ? bindable.Dispatcher : Application.Current?.Windows.LastOrDefault()?.Page?.Dispatcher;

        ArgumentNullException.ThrowIfNull(dispatcher);

        return dispatcher;
    }

    /// <summary>
    /// Dispatches an asynchronous operation to the UI thread and returns a value upon completion.
    /// </summary>
    /// <typeparam name="TReturn">The type of the value returned by the operation.</typeparam>
    /// <param name="action">The asynchronous operation to dispatch.</param>
    /// <returns>A task representing the asynchronous operation, with a result of type <typeparamref name="TReturn"/>.</returns>
    private Task<TReturn> DispatchAsync<TReturn>(Func<Task<TReturn>> action)
    {
        return GetDispatcher().DispatchAsync(action);
    }
}