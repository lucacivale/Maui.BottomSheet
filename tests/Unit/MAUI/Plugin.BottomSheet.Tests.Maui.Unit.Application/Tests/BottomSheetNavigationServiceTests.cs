using Plugin.BottomSheet.Tests.Maui.Unit.Application.Mocks;
using Plugin.BottomSheet.Tests.Maui.Unit.Application.Mocks.ViewModels;
using Plugin.Maui.BottomSheet;
using Plugin.Maui.BottomSheet.Navigation;
using Xunit.Abstractions;

namespace Plugin.BottomSheet.Tests.Maui.Unit.Application.Tests;

[Collection("UITests.Navigation")]
public class BottomSheetNavigationServiceTests : IAsyncLifetime
{
    private const string ShellRoute = "uitests";

    private readonly ITestOutputHelper _testOutputHelper;
    private readonly IBottomSheetNavigationService _bottomSheetNavigationService;

    public BottomSheetNavigationServiceTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
        _bottomSheetNavigationService = Shell.Current.Handler?.MauiContext?.Services.GetRequiredService<IBottomSheetNavigationService>()!;
    }
    
    public async Task InitializeAsync()
    {
        Routing.RegisterRoute(ShellRoute, typeof(EmptyContentPage));

        await Shell.Current.GoToAsync(ShellRoute);
    }
    
    public async Task DisposeAsync()
    {
        await _bottomSheetNavigationService.ClearBottomSheetStackAsync();
        
        await Shell.Current.GoToAsync("..");

        await Task.Delay(1000);
        
        Routing.UnRegisterRoute(ShellRoute);
    }

    [UIFact]
    public async Task NavigateToBottomSheet()
    {
        INavigationResult result = await _bottomSheetNavigationService.NavigateToAsync("EmptyBottomSheet");
        
        WriteResult(result);
        
        Assert.True(result.Success);
    }
    
    [UIFact]
    public async Task NavigateToBottomSheet_WithViewModel()
    {
        INavigationResult result = await _bottomSheetNavigationService.NavigateToAsync("EmptyBottomSheetWithViewModel");
        
        WriteResult(result);
        
        Assert.True(result.Success);
        Assert.NotNull(_bottomSheetNavigationService.NavigationStack().First().BindingContext);
    }
    
    [UIFact]
    public async Task NavigateToBottomSheet_WithViewModelAndConfig()
    {
        INavigationResult result = await _bottomSheetNavigationService.NavigateToAsync("EmptyBottomSheetWithViewModelAndConfig");
        
        WriteResult(result);
        
        Assert.True(result.Success);
        Assert.Equal(new ViewModelA().Title, _bottomSheetNavigationService.NavigationStack().First().Header?.TitleText);
    }
    
    [UIFact]
    public async Task NavigateToViewAsBottomSheet()
    {
        INavigationResult result = await _bottomSheetNavigationService.NavigateToAsync("EmptyView");
        
        WriteResult(result);
        
        Assert.True(result.Success);
    }

    [UIFact]
    public async Task NavigateToViewAsBottomSheet_WithConfig()
    {
        INavigationResult result = await _bottomSheetNavigationService.NavigateToAsync("EmptyViewWithConfig");
        
        WriteResult(result);
        
        Assert.True(result.Success);
        Assert.True(_bottomSheetNavigationService.NavigationStack().First().ShowHeader);
        Assert.Equal("Title", _bottomSheetNavigationService.NavigationStack().First().Header?.TitleText);
    }
    
    [UIFact]
    public async Task NavigateToViewAsBottomSheet_WithViewModel()
    {
        INavigationResult result = await _bottomSheetNavigationService.NavigateToAsync("EmptyViewWithViewModel");
        
        WriteResult(result);
        
        Assert.True(result.Success);
        Assert.NotNull(_bottomSheetNavigationService.NavigationStack().First().BindingContext);
    }
    
    [UIFact]
    public async Task NavigateToViewAsBottomSheet_WithViewModelAndConfig()
    {
        INavigationResult result = await _bottomSheetNavigationService.NavigateToAsync("EmptyViewWithViewModelAndConfig");
        
        WriteResult(result);
        
        Assert.True(result.Success);
        Assert.Equal(new ViewModelA().Title, _bottomSheetNavigationService.NavigationStack().First().Header?.TitleText);
    }
    
    [UIFact]
    public async Task NavigateToEmptyContentPageAsBottomSheet()
    {
        INavigationResult result = await _bottomSheetNavigationService.NavigateToAsync("EmptyContentPage");
        
        WriteResult(result);
        
        Assert.True(result.Success);
    }

    [UIFact]
    public async Task NavigateToEmptyContentPageAsBottomSheet_WithConfig()
    {
        INavigationResult result = await _bottomSheetNavigationService.NavigateToAsync("EmptyContentPageWithConfig");
        
        WriteResult(result);
        
        Assert.True(result.Success);
        Assert.True(_bottomSheetNavigationService.NavigationStack().First().ShowHeader);
        Assert.Equal("Title", _bottomSheetNavigationService.NavigationStack().First().Header?.TitleText);
    }
    
    [UIFact]
    public async Task NavigateToEmptyContentPageAsBottomSheet_WithViewModel()
    {
        INavigationResult result = await _bottomSheetNavigationService.NavigateToAsync("EmptyContentPageWithViewModel");
        
        WriteResult(result);
        
        Assert.True(result.Success);
        Assert.NotNull(_bottomSheetNavigationService.NavigationStack().First().BindingContext);
    }
    
    [UIFact]
    public async Task NavigateToEmptyContentPageAsBottomSheet_WithViewModelAndConfig()
    {
        INavigationResult result = await _bottomSheetNavigationService.NavigateToAsync("EmptyContentPageWithViewModelAndConfig");
        
        WriteResult(result);

        Assert.True(result.Success);
        Assert.Equal(new ViewModelA().Title, _bottomSheetNavigationService.NavigationStack().First().Header?.TitleText);
    }
    
    [UIFact]
    public async Task NavigateToBottomSheet_LoadedUnloaded_Fired()
    {
        await _bottomSheetNavigationService.NavigateToAsync("EmptyBottomSheet");

        EmptyBottomSheet bottomSheet = ((EmptyBottomSheet)_bottomSheetNavigationService.NavigationStack().First());
        Assert.True(bottomSheet.IsLoaded);

        await _bottomSheetNavigationService.GoBackAsync();
        
        Assert.True(bottomSheet.IsUnloaded);
    }
    
    [UIFact]
    public async Task NavigateToEmptyViewAsBottomSheet_LoadedUnloaded_Fired()
    {
        await _bottomSheetNavigationService.NavigateToAsync("EmptyView");

        IBottomSheet bottomSheet = (_bottomSheetNavigationService.NavigationStack().First());
        EmptyView? content = ((EmptyView?)bottomSheet.Content?.Content);
        
        Assert.True(content?.IsLoaded);

        await _bottomSheetNavigationService.GoBackAsync();
        
        Assert.True(content?.IsUnloaded);
    }
    
    [UIFact]
    public async Task NavigateToEmptyContentPageAsBottomSheet_LoadedUnloaded_Fired()
    {
        await _bottomSheetNavigationService.NavigateToAsync("EmptyContentPage");

        IBottomSheet bottomSheet = (_bottomSheetNavigationService.NavigationStack().First());
        EmptyView? content = ((EmptyView?)bottomSheet.Content?.Content);
        
        Assert.True(content?.IsLoaded);

        await _bottomSheetNavigationService.GoBackAsync();
        
        Assert.True(content?.IsUnloaded);
    }
    
    [UIFact]
    public async Task NavigateToBottomSheetPeek_PeekHeight_Calculated()
    {
#if !WINDOWS
        await _bottomSheetNavigationService.NavigateToAsync("BottomSheetPeek");

        BottomSheetPeek bottomSheet = ((BottomSheetPeek)_bottomSheetNavigationService.NavigationStack().First());

        double? height = bottomSheet.Content?.Content?.Measure().Height;
        
        Assert.InRange(bottomSheet.PeekHeight, height ?? 0, (height ?? 0) + 10);
#endif
    }

    [UIFact]
    public async Task NavigateBack_ConfirmNavigation_Invoked()
    {
        await _bottomSheetNavigationService.NavigateToAsync("NavigationAwareBottomSheet");

        NavigationAwareBottomSheet bottomSheet = ((NavigationAwareBottomSheet)_bottomSheetNavigationService.NavigationStack().First());
        NavigationAwareViewModel vm = (NavigationAwareViewModel)bottomSheet.BindingContext;

        await _bottomSheetNavigationService.GoBackAsync();
        
        Assert.True(bottomSheet.CanNavigateAsyncCalled);
        Assert.True(bottomSheet.CanNavigateCalled);
        Assert.True(vm.CanNavigateAsyncCalled);
        Assert.True(vm.CanNavigateCalled);
    }
    
    [UIFact]
    public async Task NavigateBack_FromPageToBottomSheet_NavigatedFrom_Invoked()
    {
        const string route = "NavigationAwareContentPage";
    
        Routing.RegisterRoute(route, typeof(NavigationAwareContentPage));
        
        await Shell.Current.GoToAsync(route);
        
        await _bottomSheetNavigationService.NavigateToAsync("NavigationAwareBottomSheet");

        NavigationAwareContentPage page = ((NavigationAwareContentPage)Shell.Current.CurrentPage);
        NavigationAwareViewModel vm = (NavigationAwareViewModel)page.BindingContext;

        await _bottomSheetNavigationService.GoBackAsync();
        
        Assert.True(page.NavigatedFromCalled);
        Assert.True(vm.NavigatedFromCalled);
        
        await Shell.Current.GoToAsync("..");

        Routing.UnRegisterRoute(route);
    }
    
        [UIFact]
        public async Task NavigateBack_FromBottomSheetToPage_NavigatedTo_Invoked()
        {
            const string route = "NavigationAwareContentPage";
        
            Routing.RegisterRoute(route, typeof(NavigationAwareContentPage));
            
            await Shell.Current.GoToAsync(route);
            
            await _bottomSheetNavigationService.NavigateToAsync("NavigationAwareBottomSheet");
    
            NavigationAwareContentPage page = ((NavigationAwareContentPage)Shell.Current.CurrentPage);
            NavigationAwareViewModel vm = (NavigationAwareViewModel)page.BindingContext;
    
            await _bottomSheetNavigationService.GoBackAsync();
            
            Assert.True(page.NavigatedToCalled);
            Assert.True(vm.NavigatedToCalled);
            
            await Shell.Current.GoToAsync("..");
    
            Routing.UnRegisterRoute(route);
        }
    
    [UIFact]
    public async Task NavigateBack_ConfirmNavigation_ParametersPassed()
    {
        await _bottomSheetNavigationService.NavigateToAsync("NavigationAwareBottomSheet");

        NavigationAwareBottomSheet bottomSheet = ((NavigationAwareBottomSheet)_bottomSheetNavigationService.NavigationStack().First());
        NavigationAwareViewModel vm = (NavigationAwareViewModel)bottomSheet.BindingContext;

        BottomSheetNavigationParameters parameters = new()
        {
            { "Test", "Test" }
        };
        
        await _bottomSheetNavigationService.GoBackAsync(parameters);
        
        Assert.Same(bottomSheet.Parameters, parameters);
        Assert.True(bottomSheet.CanNavigateAsyncCalled);
        Assert.True(bottomSheet.CanNavigateCalled);
        Assert.Same(vm.Parameters, parameters);
        Assert.True(vm.CanNavigateAsyncCalled);
        Assert.True(vm.CanNavigateCalled);
    }
    
    [UIFact]
    public async Task NavigateBack_ConfirmNavigation_NavigationCancelled()
    {
        await _bottomSheetNavigationService.NavigateToAsync("NavigationAwareBottomSheet");

        NavigationAwareBottomSheet bottomSheet = ((NavigationAwareBottomSheet)_bottomSheetNavigationService.NavigationStack().First());

        bottomSheet.NavigationAllowed = false;
        
        await _bottomSheetNavigationService.GoBackAsync();
        
        bottomSheet.NavigationAllowed = true;
        
        Assert.True(bottomSheet.CanNavigateCalled);
        Assert.Equal(bottomSheet, _bottomSheetNavigationService.NavigationStack().First());
    }
    
    [UIFact]
    public async Task NavigateToOtherSheet_ConfirmNavigation_Invoked()
    {
        await _bottomSheetNavigationService.NavigateToAsync("NavigationAwareBottomSheet");

        NavigationAwareBottomSheet bottomSheet = ((NavigationAwareBottomSheet)_bottomSheetNavigationService.NavigationStack().First());
        NavigationAwareViewModel vm = (NavigationAwareViewModel)bottomSheet.BindingContext;
        
        await _bottomSheetNavigationService.NavigateToAsync("EmptyBottomSheet");
        
        Assert.True(bottomSheet.CanNavigateAsyncCalled);
        Assert.True(bottomSheet.CanNavigateCalled);
        Assert.True(vm.CanNavigateAsyncCalled);
        Assert.True(vm.CanNavigateCalled);
    }
    
    [UIFact]
    public async Task NavigateToOtherSheet_ConfirmNavigation_ParametersPassed()
    {
        await _bottomSheetNavigationService.NavigateToAsync("NavigationAwareBottomSheet");

        NavigationAwareBottomSheet bottomSheet = ((NavigationAwareBottomSheet)_bottomSheetNavigationService.NavigationStack().First());
        NavigationAwareViewModel vm = (NavigationAwareViewModel)bottomSheet.BindingContext;

        BottomSheetNavigationParameters parameters = new()
        {
            { "Test", "Test" }
        };
        
        await _bottomSheetNavigationService.NavigateToAsync("EmptyBottomSheet", parameters);
        
        Assert.Same(bottomSheet.Parameters, parameters);
        Assert.True(bottomSheet.CanNavigateAsyncCalled);
        Assert.True(bottomSheet.CanNavigateCalled);
        Assert.Same(vm.Parameters, parameters);
        Assert.True(vm.CanNavigateAsyncCalled);
        Assert.True(vm.CanNavigateCalled);
    }
    
    [UIFact]
    public async Task NavigateToOtherSheet_ConfirmNavigation_NavigationCancelled()
    {
        await _bottomSheetNavigationService.NavigateToAsync("NavigationAwareBottomSheet");

        NavigationAwareBottomSheet bottomSheet = ((NavigationAwareBottomSheet)_bottomSheetNavigationService.NavigationStack().First());

        bottomSheet.NavigationAllowed = false;
        
        await _bottomSheetNavigationService.NavigateToAsync("EmptyBottomSheet");
        
        bottomSheet.NavigationAllowed = true;
        
        Assert.True(bottomSheet.CanNavigateCalled);
    }
    
    [UIFact]
    public async Task Navigation_ParametersShared()
    {
        BottomSheetNavigationParameters parameters = new()
        {
            { "Test", "Test" }
        };
        
        await _bottomSheetNavigationService.NavigateToAsync("NavigationAwareBottomSheet", parameters);

        NavigationAwareBottomSheet bottomSheet = ((NavigationAwareBottomSheet)_bottomSheetNavigationService.NavigationStack().First());
        NavigationAwareViewModel vm = (NavigationAwareViewModel)bottomSheet.BindingContext;
        
        await _bottomSheetNavigationService.NavigateToAsync("EmptyBottomSheet", parameters);
        
        Assert.True(bottomSheet.CanNavigateCalled);
        Assert.True(bottomSheet.NavigatedFromCalled);
        Assert.True(bottomSheet.NavigatedToCalled);
        Assert.Same(bottomSheet.Parameters, parameters);
        Assert.Equal(9, parameters.Count);
        Assert.True(vm.CanNavigateCalled);
        Assert.True(vm.NavigatedFromCalled);
        Assert.True(vm.NavigatedToCalled);
        Assert.Same(vm.Parameters, parameters);
        Assert.Equal(9, parameters.Count);
    }
    
    [UIFact]
    public async Task Navigation_ParametersShared_BetweenBottomSheets()
    {
        BottomSheetNavigationParameters parameters = new()
        {
            { "Test", "Test" }
        };
        
        await _bottomSheetNavigationService.NavigateToAsync("NavigationAwareBottomSheet", parameters);

        NavigationAwareBottomSheet bottomSheet = ((NavigationAwareBottomSheet)_bottomSheetNavigationService.NavigationStack().First());
        NavigationAwareViewModel vm = (NavigationAwareViewModel)bottomSheet.BindingContext;
        
        await _bottomSheetNavigationService.NavigateToAsync("NavigationAwareBottomSheet", parameters);
        
        NavigationAwareBottomSheet bottomSheet2 = ((NavigationAwareBottomSheet)_bottomSheetNavigationService.NavigationStack().Last());
        NavigationAwareViewModel vm2 = (NavigationAwareViewModel)bottomSheet2.BindingContext;
        
        Assert.True(bottomSheet.CanNavigateCalled);
        Assert.True(bottomSheet.NavigatedFromCalled);
        Assert.True(bottomSheet.NavigatedToCalled);
        Assert.Same(bottomSheet.Parameters, parameters);
        Assert.Equal(11, parameters.Count);
        Assert.True(vm.CanNavigateCalled);
        Assert.True(vm.NavigatedFromCalled);
        Assert.True(vm.NavigatedToCalled);
        Assert.Same(vm.Parameters, parameters);
        Assert.Equal(11, parameters.Count);

        await _bottomSheetNavigationService.GoBackAsync(parameters);
        
        Assert.True(vm2.CanNavigateCalled);
        Assert.True(vm2.NavigatedFromCalled);
        Assert.True(vm2.NavigatedToCalled);
        Assert.Same(vm2.Parameters, parameters);
        Assert.Equal(19, parameters.Count);
    }

    [UIFact]
    public async Task Issue210_ModalPage_NavigateToBottomSheet()
    {
        Routing.RegisterRoute(nameof(Issue210_ModalPage_NavigateToBottomSheet), typeof(EmptyModalContentPage));

        await Shell.Current.GoToAsync(nameof(Issue210_ModalPage_NavigateToBottomSheet), true);
        
        INavigationResult result = await _bottomSheetNavigationService.NavigateToAsync("EmptyBottomSheet");
        
        WriteResult(result);
        
        await Shell.Current.GoToAsync("..");
        Routing.UnRegisterRoute(nameof(Issue210_ModalPage_NavigateToBottomSheet));
        
        Assert.True(result.Success);
    }
    
    private void WriteResult(INavigationResult result)
    {
        if (result.Exception is not null)
        {
            _testOutputHelper.WriteLine(result.Exception.ToString());
        }
    }
}