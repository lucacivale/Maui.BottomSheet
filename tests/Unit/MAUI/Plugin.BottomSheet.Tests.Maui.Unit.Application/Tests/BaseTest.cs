using Plugin.Maui.BottomSheet;
using Xunit.Abstractions;

namespace Plugin.BottomSheet.Tests.Maui.Unit.Application.Tests;

[Collection("UITests")]
public abstract class BaseTest<TPage, TView> : IAsyncLifetime
    where TPage : ContentPage
    where TView : View, new()
{
    private const string ShellRoute = "uitests";

    private TPage _currentPage = null!;

    protected BaseTest(ITestOutputHelper testOutputHelper)
    {
        View = new TView();
        TestOutputHelper = testOutputHelper;
    }
    
    protected TView View { get; }
    
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
        await LoadContent(View);
    }
    
    private Task Disposing()
    {
        if (View is IBottomSheet bottomSheet)
        {
            Assert.False(bottomSheet.IsOpen, "BottomSheet still open after page is popped.");
        }

        return Task.CompletedTask;
    }
    
    private async Task LoadContent(View content)
    {
        TaskCompletionSource tcs = new TaskCompletionSource();

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

        TaskCompletionSource tcs = new TaskCompletionSource();

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