using Plugin.BottomSheet.Tests.Maui.Unit.Application.Mocks;
using Plugin.BottomSheet.Tests.Maui.Unit.Application.Resources.Styles;
using Plugin.Maui.BottomSheet;

namespace Plugin.BottomSheet.Tests.Maui.Unit.Application.Tests;

public class GlobalStyleTests : IAsyncLifetime
{
    private const string ShellRoute = "uitests";

    private ContentPage _currentPage = null!;
    
    public async Task InitializeAsync()
    {
        Routing.RegisterRoute(ShellRoute, typeof(Mocks.EmptyContentPage));

        await Shell.Current.GoToAsync(ShellRoute);

        _currentPage = (Mocks.EmptyContentPage)Shell.Current.CurrentPage;

        await WaitForLoaded(_currentPage);
    }
    
    public async Task DisposeAsync()
    {
        _currentPage = null!;

        await Shell.Current.GoToAsync("..");

        await Task.Delay(1000);
        
        Routing.UnRegisterRoute(ShellRoute);
    }
    
    [UIFact]
    public async Task GlobalImplicitStyle_Padding()
    {
        Microsoft.Maui.Controls.Application.Current?.Resources.MergedDictionaries.Add(new GlobalImplicitStylePadding());

        Plugin.Maui.BottomSheet.BottomSheet bottomSheet = new()
        {
            Content  = new BottomSheetContent
            {
                Content = new BoxView
                {
                    Background = Colors.Red
                }
            },
        };
        
        await LoadContent(bottomSheet);

        bottomSheet.IsOpen = true;

        await Task.Delay(500);
        
        Assert.True(bottomSheet.IsOpen);
        Assert.Equal(bottomSheet.Padding, 0);
        Assert.Equal(bottomSheet.ContainerView.Padding, 0);
    }
    
    [UIFact]
    public async Task GlobalImplicitStyle_ApplyToDerivedTypes_Padding()
    {
        Microsoft.Maui.Controls.Application.Current?.Resources.MergedDictionaries.Add(new GlobalImplicitStyleApplyToDerivedTypesPadding());

        EmptyBottomSheet bottomSheet = new();
        
        await LoadContent(bottomSheet);

        bottomSheet.IsOpen = true;

        await Task.Delay(500);
        
        Assert.True(bottomSheet.IsOpen);
        Assert.Equal(bottomSheet.Padding, 0);
        Assert.Equal(bottomSheet.ContainerView.Padding, 0);
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
    
    protected async Task LoadContent(View content)
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
}