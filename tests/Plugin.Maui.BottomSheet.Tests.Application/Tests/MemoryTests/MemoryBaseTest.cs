using System.Diagnostics.CodeAnalysis;
using Xunit.Abstractions;

namespace Plugin.Maui.BottomSheet.Tests.Application.Tests.MemoryTests;

[Collection("UITests")]
public abstract class MemoryBaseTest<TPage, TView> : IAsyncLifetime
    where TPage : ContentPage
    where TView : View, new()
{
    private const string ShellRoute = "uitests";

    private TPage _currentPage = null!;
    private WeakReference<TView> _weakView = null!;

    protected MemoryBaseTest(ITestOutputHelper testOutputHelper)
    {
        TestOutputHelper = testOutputHelper;
    }
    
    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    protected ITestOutputHelper TestOutputHelper { get; }
    
    public async Task InitializeAsync()
    {
        Routing.RegisterRoute(ShellRoute, typeof(TPage));

        await Shell.Current.GoToAsync(ShellRoute);

        _currentPage = (TPage)Shell.Current.CurrentPage;

        await WaitForLoaded(_currentPage);
        
        await Initialized();
    }
    
    public async Task DisposeAsync()
    {
        _currentPage = null!;

        await Shell.Current.GoToAsync("..");
        
        await Task.Delay(1000);
        
        await Disposing();

        Routing.UnRegisterRoute(ShellRoute);
    }

    private async Task Initialized()
    {
        TView view = new();
        _weakView = new WeakReference<TView>(view);
        
        await LoadContent(view);
    }

    virtual async protected Task Disposing()
    {
        if (TryGetTarget(out TView? view) 
            && view is IBottomSheet bottomSheet)
        {
            Assert.False(bottomSheet.IsOpen, $"{typeof(TView).Name} still open after page is popped.");
        }
        
        view = null;
        bottomSheet = null!;

        await ForceGc();

        Assert.False(_weakView.TryGetTarget(out TView? _), $"{typeof(TView).Name} should be garbage collected");
    }

    async protected Task ForceGc()
    {
        // Force multiple garbage collection cycles to ensure cleanup
        for (int i = 0; i < 3; i++)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            await Task.Delay(100); // Allow time for cleanup
        }
    }
    
    protected bool TryGetTarget([MaybeNullWhen(false), NotNullWhen(true)] out TView target)
    {
        bool result = _weakView.TryGetTarget(out target);
        
        Assert.True(result, $"Could not get {typeof(TView).Name} from weak reference.");

        return result;
    }
    
    protected bool TryGetTarget<TTarget>(WeakReference<TTarget> weakReference, [MaybeNullWhen(false), NotNullWhen(true)] out TTarget target)
        where TTarget : class
    {
        bool result = weakReference.TryGetTarget(out target);
        
        Assert.True(result, $"Could not get {typeof(TTarget).Name} from weak reference.");

        return result;
    }
    
    private async Task LoadContent(View content)
    {
        TaskCompletionSource tcs = new();

        content.Loaded += OnLoaded;

        _currentPage.Content = content;

        await tcs.Task;

        void OnLoaded(object? sender, EventArgs e)
        {
            content.Loaded -= OnLoaded;
            tcs.SetResult();
        }
    }
    
    private static async Task WaitForLoaded(VisualElement element, int timeout = 1000)
    {
        if (element.IsLoaded)
            return;

        var tcs = new TaskCompletionSource();

        element.Loaded += OnLoaded;

        await Task.WhenAny(tcs.Task, Task.Delay(timeout));

        Assert.True(element.IsLoaded);

        return;

        void OnLoaded(object? sender, EventArgs e)
        {
            element.Loaded -= OnLoaded;
            tcs.SetResult();
        }
    }
}