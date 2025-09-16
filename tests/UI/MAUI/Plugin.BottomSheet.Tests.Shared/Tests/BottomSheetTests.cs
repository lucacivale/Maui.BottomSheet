using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.Interfaces;
using OpenQA.Selenium.Interactions;
using Plugin.BottomSheet.Tests.Android;
using Xunit.Abstractions;

namespace Plugin.BottomSheet.Tests.Shared;

[Trait("Category", "BottomSheetTests")]
public class BottomSheetTests : BaseTest, IAsyncLifetime
{
    private readonly MainPage _mainPage;

    // Is set in init and never null
    private BottomSheetTestsPage _bottomSheetTestsPage = null!;
    
    public BottomSheetTests(AppiumSetupFixture appiumSetupFixture, ITestOutputHelper testOutputHelper)
        : base(appiumSetupFixture, testOutputHelper)
    {
        _mainPage = new MainPage(appiumSetupFixture.App);
    }
    
    public async Task InitializeAsync()
    {
        _bottomSheetTestsPage = await _mainPage.OpenBottomSheetTestsPage();
    }

    public async Task DisposeAsync()
    {
        if (App.TryFindElementByAutomationId(BottomSheetTestsAutomationIds.BottomSheet, out _))
        {
            await GoBackAsync();
        }
        
        await GoBackAsync();
    }
    
    [Fact]
    [Trait("Category", "BottomSheetTests")]
    public void OpenBottomSheetHeaderTests()
    {
        Assert.True(_bottomSheetTestsPage.IsOpen());
    }
    
    [Fact]
    [Trait("Category", "BottomSheetTests")]
    public async Task OpenBottomSheet()
    {
        var bottomSheet = await _bottomSheetTestsPage.OpenBottomSheetAsync();

        Assert.True(bottomSheet.IsOpen());
    }
    
    [Fact]
    [Trait("Category", "BottomSheetTests.CornerRadius")]
    public async Task BottomSheet_ChangeCornerRadius_HasRoundedCorners()
    {
        var bottomSheet = await _bottomSheetTestsPage.OpenBottomSheetAsync();

        await bottomSheet.SetCornerRadiusAsync(50);

        Assert.True(bottomSheet.IsOpen());
    }
    
    [Fact]
    [Trait("Category", "BottomSheetTests.Margin")]
    public async Task BottomSheet_ChangeMargin()
    {
        const float margin = 50;
        double marginPx = Math.Ceiling(App.ToPixels(margin));
        double marginWidth = marginPx * 2;

        var bottomSheet = await _bottomSheetTestsPage.OpenBottomSheetAsync();

        var oldSize = bottomSheet.Size();
        await bottomSheet.SetMarginAsync(margin);
        var newSize = bottomSheet.Size();

        Assert.InRange(oldSize.Width - newSize.Width, marginWidth - 1, marginWidth + 1);
        Assert.Equal(oldSize.Height, newSize.Height);
    }
    
    [Fact]
    [Trait("Category", "BottomSheetTests.Padding")]
    public async Task BottomSheet_ChangePadding()
    {
        const float padding = 50;
        double paddingPx = Math.Ceiling(App.ToPixels(padding));
        double paddingAmount = paddingPx * 2;

        var bottomSheet = await _bottomSheetTestsPage.OpenBottomSheetAsync();

        var size = bottomSheet.Size();
        var location = bottomSheet.Location();

        await bottomSheet.SetPaddingAsync(padding);

        var contentSize = bottomSheet.ContentSize();
        var contentLocation = bottomSheet.ContentLocation();
        var handleLocation = bottomSheet.HandleLocation();
        var handleSize = bottomSheet.HandleSize();

        Assert.InRange(size.Width - contentSize.Width, paddingAmount - 1, paddingAmount + 1);
        Assert.InRange(location.Y + paddingPx, contentLocation.Y - 1, contentLocation.Y + 1);
        Assert.InRange(location.Y + handleSize.Height, handleLocation.Y - 1, handleLocation.Y + 1);
    }
    
    [Fact]
    [Trait("Category", "BottomSheetTests.HalfExpandedRatio")]
    public async Task BottomSheet_ChangeHalfExpandedRatio()
    {
        const float halfExpandedRatio = 0.8f;
        const float halfExpandedRatioPercentage = 0.8f * 100;

        var bottomSheet = await _bottomSheetTestsPage.OpenBottomSheetAsync();

        await bottomSheet.SetHalfExpandedRatio(halfExpandedRatio);

        double actualHalfExpandedRatio = (double)bottomSheet.Size().Height / App.Manage().Window.Size.Height * 100;

        Assert.InRange(actualHalfExpandedRatio, halfExpandedRatioPercentage - 1, halfExpandedRatioPercentage + 1);
    }
    
    [Fact]
    [Trait("Category", "BottomSheetTests.Modal")]
    public async Task BottomSheet_NonModalBottomSheet_TouchDispatched()
    {
        var mainWindowButtonLocation = _bottomSheetTestsPage.SomeButtonLocation();
        var bottomSheet = await _bottomSheetTestsPage.OpenBottomSheetAsync();

        await bottomSheet.SetNonModalAsync();
        
        new Actions(App)
            .MoveToLocation(mainWindowButtonLocation.X, mainWindowButtonLocation.Y)
            .Click()
            .Perform();
        
        Assert.True(bottomSheet.IsOpen());
        
        await App.Navigate().BackAsync();

        Assert.Equal("Clicked!", _bottomSheetTestsPage.SomeButtonText());
    }
    
    [Fact]
    [Trait("Category", "BottomSheetTests.Modal")]
    public async Task BottomSheet_ModalBottomSheet_BackgroundTouched_BottomSheetClosed()
    {
        var bottomSheet = await _bottomSheetTestsPage.OpenBottomSheetAsync();
        var location = bottomSheet.Location();
        
        await bottomSheet.SetModalAsync();
        
        new Actions(App)
            .MoveToLocation(location.X, location.Y - 100)
            .Click()
            .Perform();
        
        Assert.False(bottomSheet.IsOpen());
    }
    
    [Fact]
    [Trait("Category", "BottomSheetTests.Handle")]
    public async Task BottomSheet_HandleEnabled_HandleDisplayed()
    {
        var bottomSheet = await _bottomSheetTestsPage.OpenBottomSheetAsync();
        
        await bottomSheet.ShowHandleAsync();
        
        Assert.True(bottomSheet.HandleDisplayed());
    }
    
    [Fact]
    [Trait("Category", "BottomSheetTests.Handle")]
    public async Task BottomSheet_HandleDisabled_HandleHidden()
    {
        var bottomSheet = await _bottomSheetTestsPage.OpenBottomSheetAsync();
        
        await bottomSheet.HideHandleAsync();
        
        Assert.False(bottomSheet.HandleDisplayed());
    }
    
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    [Trait("Category", "BottomSheetTests.Cancelable")]
    public async Task BottomSheet_IsCancelable_BackButtonClosesSheet(bool isCancelable)
    {
        var bottomSheet = await _bottomSheetTestsPage.OpenBottomSheetAsync();

        await bottomSheet.SetHalfExpandedRatio(0.8f);
        await bottomSheet.IsCancelableAsync(isCancelable);
        
        await App.Navigate().BackAsync();

        Assert.NotEqual(isCancelable, bottomSheet.IsOpen());

        if (isCancelable == false)
        {
            await bottomSheet.IsCancelableAsync(true);
        }
    }
    
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    [Trait("Category", "BottomSheetTests.Cancelable")]
    public async Task BottomSheet_IsCancelable_BackgroundClickClosesSheet(bool isCancelable)
    {
        var bottomSheet = await _bottomSheetTestsPage.OpenBottomSheetAsync();
        
        await bottomSheet.SetHalfExpandedRatio(0.8f);
        await bottomSheet.IsCancelableAsync(isCancelable);
        
        var location = bottomSheet.Location();
        
        new Actions(App)
            .MoveToLocation(location.X, location.Y - 100)
            .Click()
            .Perform();

        Assert.NotEqual(isCancelable, bottomSheet.IsOpen());
        
        if (isCancelable == false)
        {
            await bottomSheet.IsCancelableAsync(true);
        }
    }
    
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    [Trait("Category", "BottomSheetTests.Cancelable")]
    public async Task BottomSheet_IsCancelable_DragDownClosesSheet(bool isCancelable)
    {
        var bottomSheet = await _bottomSheetTestsPage.OpenBottomSheetAsync();
        
        await bottomSheet.SetHalfExpandedRatio(0.8f);

        var location = bottomSheet.Location();
        var size = bottomSheet.Size();
        
        await bottomSheet.IsCancelableAsync(isCancelable);
        
        int startX = location.X + (size.Width / 2);
        int startY = location.Y + 50;
    
        int endX = startX;
        int endY = startY + (size.Height / 2) + 100;
    
        new Actions(App)
            .MoveToLocation(startX, startY)
            .ClickAndHold()
            .MoveToLocation(endX, endY)
            .Release()
            .Perform();

        Assert.NotEqual(isCancelable, bottomSheet.IsOpen());
        
        if (isCancelable == false)
        {
            await bottomSheet.IsCancelableAsync(true);
        }
    }
    
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    [Trait("Category", "BottomSheetTests.Draggable")]
    public async Task BottomSheet_IsDraggable_CanBeDragged(bool isDraggable)
    {
        var bottomSheet = await _bottomSheetTestsPage.OpenBottomSheetAsync();
        var size = bottomSheet.Size();
        
        await bottomSheet.SetHalfExpandedRatio(0.8f);
        await bottomSheet.IsDraggableAsync(isDraggable);
        await bottomSheet.SetHalfExpandedRatio(0.5f);

        var location = bottomSheet.HandleLocation();
        
        int startX = location.X;
        int startY = location.Y + 100;
        
        new Actions(App)
            .MoveToLocation(startX, startY)
            .ClickAndHold()
            .MoveToLocation(startX, startY - 400)
            .Release()
            .Perform();

        Assert.True(bottomSheet.IsOpen());
        
        if (isDraggable)
        {
            Assert.True(bottomSheet.Size().Height > size.Height);
        }
        else
        {
            Assert.Equal(size.Height, bottomSheet.Size().Height);
        }
    }
    
    [Fact]
    [Trait("Category", "BottomSheetTests.States")]
    public async Task LargeSheet_MediumAndLargeState_DisableLargeState_SheetIsMedium()
    {
        var bottomSheet = await _bottomSheetTestsPage.OpenBottomSheetAsync();
        var location = bottomSheet.HandleLocation();
        
        int startY = location.Y + 100;
        
        new Actions(App)
            .MoveToLocation(location.X, startY)
            .ClickAndHold()
            .MoveToLocation(location.X, startY - 400)
            .Release()
            .Perform();

        var size = bottomSheet.Size();
        
        await bottomSheet.ChangeStates(true, false);
        
        Assert.True(size.Height > bottomSheet.Size().Height);;
    }
}