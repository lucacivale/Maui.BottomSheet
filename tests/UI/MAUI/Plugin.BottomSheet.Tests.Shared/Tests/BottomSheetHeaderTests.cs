using Xunit.Abstractions;

namespace Plugin.BottomSheet.Tests.Shared;

[Trait("Category", "BottomSheetHeaderTests")]
public class BottomSheetHeaderTests : BaseTest, IAsyncLifetime
{
    private readonly MainPage _mainPage;

    // Is set in init and never null
    private BottomSheetHeaderTestsPage _bottomSheetHeaderTestsPage = null!;
    
    public BottomSheetHeaderTests(AppiumSetupFixture appiumSetupFixture, ITestOutputHelper testOutputHelper)
        : base(appiumSetupFixture, testOutputHelper)
    {
        _mainPage = new MainPage(appiumSetupFixture.App);
    }
    
    public async Task InitializeAsync()
    {
        _bottomSheetHeaderTestsPage = await _mainPage.OpenBottomSheetHeaderTestsPage();
    }

    public async Task DisposeAsync()
    {
        if (App.TryFindElementByAutomationId(BottomSheetHeaderTestsAutomationIds.BottomSheetBuiltInHeader, out _)
            || App.TryFindElementByAutomationId(BottomSheetHeaderTestsAutomationIds.BottomSheetCustomHeader, out _))
        {
            await GoBackAsync();
        }
        
        await GoBackAsync();
    }
    
    [Fact]
    [Trait("Category", "BottomSheetHeaderTests")]
    public void OpenBottomSheetHeaderTests()
    {
        Assert.True(_bottomSheetHeaderTestsPage.IsOpen());
    }
    
    [Fact]
    [Trait("Category", "BottomSheetHeaderTests")]
    public async Task OpenBottomSheet()
    {
        var bottomSheet = await _bottomSheetHeaderTestsPage.OpenBottomSheetAsync();

        Assert.True(bottomSheet.IsOpen());
    }
    
    [Fact]
    [Trait("Category", "BottomSheetHeaderTests")]
    public async Task OpenBottomSheet_ChangeBindingContext_BindingContextChanged()
    {
        string title = "Title";
        var bottomSheet = await _bottomSheetHeaderTestsPage.OpenBottomSheetAsync();

        await bottomSheet.ShowHeaderAsync();
        bottomSheet.EnterTitle(title);

        await bottomSheet.ChangeBindingContextAsync();
        
        Assert.False(bottomSheet.HeaderDisplayed());
        Assert.False(bottomSheet.TitleDisplayed(title));
    }
    
    [Fact]
    [Trait("Category", "BottomSheetHeaderTests")]
    public async Task OpenBottomSheet_CloseButtonClicked_BottomSheetClosed()
    {
        var bottomSheet = await _bottomSheetHeaderTestsPage.OpenBottomSheetAsync();

        await bottomSheet.ClickCloseButtonAsync();
        
        Assert.False(bottomSheet.IsOpen());
    }
    
    [Fact]
    [Trait("Category", "BottomSheetHeaderTests.Header")]
    public async Task UnconfiguredBuiltInHeader_ShowHeader_HeaderNotVisible()
    {
        var bottomSheet = await _bottomSheetHeaderTestsPage.OpenBottomSheetAsync();

        await bottomSheet.ShowHeaderAsync();

        Assert.False(bottomSheet.HeaderDisplayed());
    }
    
    [Fact]
    [Trait("Category", "BottomSheetHeaderTests.Header")]
    public async Task HeaderWithTitle_ShowHeader_HeaderVisible()
    {
        var bottomSheet = await _bottomSheetHeaderTestsPage.OpenBottomSheetAsync();

        bottomSheet.EnterTitle("Title");
        
        await bottomSheet.ShowHeaderAsync();

        Assert.True(bottomSheet.HeaderDisplayed());
    }
    
    [Fact]
    [Trait("Category", "BottomSheetHeaderTests.Header")]
    public async Task HeaderWithAnyMode_ShowHeader_HeaderVisible()
    {
        var bottomSheet = await _bottomSheetHeaderTestsPage.OpenBottomSheetAsync();

        await bottomSheet.ClickNoneMode();
        await bottomSheet.ShowHeaderAsync();

        Assert.False(bottomSheet.HeaderDisplayed());

        await bottomSheet.ClickLeftMode();
        Assert.True(bottomSheet.HeaderDisplayed());
        
        await bottomSheet.ClickRightMode();
        Assert.True(bottomSheet.HeaderDisplayed());
        
        await bottomSheet.ClickLeftAndRightMode();
        Assert.True(bottomSheet.HeaderDisplayed());
    }
    
    [Fact]
    [Trait("Category", "BottomSheetHeaderTests.Header")]
    public async Task VisibleHeader_NoneMode_HeaderHidden()
    {
        var bottomSheet = await _bottomSheetHeaderTestsPage.OpenBottomSheetAsync();

        await bottomSheet.ClickLeftMode();
        await bottomSheet.ShowHeaderAsync();

        await bottomSheet.ClickNoneMode();

        Assert.False(bottomSheet.HeaderDisplayed());
    }
    
    [Fact]
    [Trait("Category", "BottomSheetHeaderTests.Header")]
    public async Task VisibleHeader_HideHeader_HeaderHidden()
    {
        var bottomSheet = await _bottomSheetHeaderTestsPage.OpenBottomSheetAsync();

        bottomSheet.EnterTitle("Title");
        
        await bottomSheet.ShowHeaderAsync();
        await WaitAsync();
        await bottomSheet.HideHeaderAsync();

        Assert.False(bottomSheet.HeaderDisplayed());
    }
    
    [Fact]
    [Trait("Category", "BottomSheetHeaderTests.Title")]
    public async Task VisibleHeader_TitleEntered_HeaderTitleVisible()
    {
        string title = "Title";
        var bottomSheet = await _bottomSheetHeaderTestsPage.OpenBottomSheetAsync();

        await bottomSheet.ShowHeaderAsync();
        
        bottomSheet.EnterTitle(title);

        Assert.True(bottomSheet.TitleDisplayed(title));
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [Trait("Category", "BottomSheetHeaderTests.Title")]
    public async Task VisibleHeader_TitleCleared_HeaderTitleHidden(string clearTitle)
    {
        string title = "Title";
        var bottomSheet = await _bottomSheetHeaderTestsPage.OpenBottomSheetAsync();

        await bottomSheet.ShowHeaderAsync();
        
        bottomSheet.EnterTitle(title);
        bottomSheet.EnterTitle(clearTitle);

        Assert.False(bottomSheet.TitleDisplayed(clearTitle));
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [Trait("Category", "BottomSheetHeaderTests.Title")]
    public async Task VisibleInHeader_TitleEmpty_HeaderTitleHidden(string title)
    {
        var bottomSheet = await _bottomSheetHeaderTestsPage.OpenBottomSheetAsync();

        await bottomSheet.ShowHeaderAsync();

        bottomSheet.EnterTitle(title);

        Assert.False(bottomSheet.TitleDisplayed(title));
    }
    
    [Fact]
    [Trait("Category", "BottomSheetHeaderTests.CloseButton")]
    public async Task VisibleHeader_CloseButtonDisabled_CloseButtonNotVisible()
    {
        var bottomSheet = await _bottomSheetHeaderTestsPage.OpenBottomSheetAsync();
        
        await bottomSheet.ShowHeaderAsync();
        
        Assert.False(bottomSheet.CloseButtonDisplayed());
    }
    
    [Fact]
    [Trait("Category", "BottomSheetHeaderTests.CloseButton")]
    public async Task VisibleHeader_CloseButtonEnabled_CloseButtonVisible()
    {
        var bottomSheet = await _bottomSheetHeaderTestsPage.OpenBottomSheetAsync();
        
        await bottomSheet.ShowHeaderAsync();
        await bottomSheet.ShowCloseButtonAsync();
        
        Assert.True(bottomSheet.CloseButtonDisplayed());
    }
    
    [Fact]
    [Trait("Category", "BottomSheetHeaderTests.CloseButton")]
    public async Task VisibleCloseButton_DisableCloseButton_CloseButtonHidden()
    {
        var bottomSheet = await _bottomSheetHeaderTestsPage.OpenBottomSheetAsync();
        
        await bottomSheet.ShowHeaderAsync();
        await bottomSheet.ShowCloseButtonAsync();

        await bottomSheet.HideCloseButtonAsync();
        
        Assert.False(bottomSheet.CloseButtonDisplayed());
    }
    
    [Fact]
    [Trait("Category", "BottomSheetHeaderTests.CloseButton")]
    public async Task CloseButton_Clicked_BottomSheetClosed()
    {
        var bottomSheet = await _bottomSheetHeaderTestsPage.OpenBottomSheetAsync();
        
        await bottomSheet.ShowHeaderAsync();
        await bottomSheet.ShowCloseButtonAsync();

        await bottomSheet.ClickHeaderCloseButtonAsync();

        Assert.False(bottomSheet.IsOpen());
    }
    
    [Fact]
    [Trait("Category", "BottomSheetHeaderTests.CloseButton")]
    public async Task CloseButton_PositonLeft_CloseButtonLeft()
    {
        var bottomSheet = await _bottomSheetHeaderTestsPage.OpenBottomSheetAsync();
        
        await bottomSheet.ShowHeaderAsync();
        await bottomSheet.ShowCloseButtonAsync();
        await bottomSheet.ClickCloseButtonPositionLeftAsync();

        Assert.True(bottomSheet.IsCloseButtonAlignedLeft());
    }
    
    [Fact]
    [Trait("Category", "BottomSheetHeaderTests.CloseButton")]
    public async Task CloseButton_PositonLeft_CloseButtonRight()
    {
        var bottomSheet = await _bottomSheetHeaderTestsPage.OpenBottomSheetAsync();
        
        await bottomSheet.ShowHeaderAsync();
        await bottomSheet.ShowCloseButtonAsync();
        await bottomSheet.ClickCloseButtonPositionRightAsync();

        Assert.True(bottomSheet.IsCloseButtonAlignedRight());
    }
    
    [Fact]
    [Trait("Category", "BottomSheetHeaderTests.CloseButton")]
    public async Task NonCancelableSheet_CloseButtonClicked_BottomSheetClosed()
    {
        var bottomSheet = await _bottomSheetHeaderTestsPage.OpenBottomSheetAsync();

        await bottomSheet.ShowHeaderAsync();
        await bottomSheet.ShowCloseButtonAsync();
        await bottomSheet.NonCancelableAsync();

        await bottomSheet.ClickHeaderCloseButtonAsync();

        Assert.True(bottomSheet.IsOpen());

        await bottomSheet.IsCancelableAsync();
    }
    
    [Fact]
    [Trait("Category", "BottomSheetHeaderTests.CloseButton")]
    public async Task CancelableSheet_CloseButtonClicked_BottomSheetClosed()
    {
        var bottomSheet = await _bottomSheetHeaderTestsPage.OpenBottomSheetAsync();

        await bottomSheet.IsCancelableAsync();
        await bottomSheet.ShowHeaderAsync();
        await bottomSheet.ShowCloseButtonAsync();

        await bottomSheet.ClickHeaderCloseButtonAsync();

        Assert.False(bottomSheet.IsOpen());
    }
    
    [Fact]
    [Trait("Category", "BottomSheetHeaderTests.AppearanceMode")]
    public async Task HeaderHidden_AnyMode_DoesNotAddButton()
    {
        var bottomSheet = await _bottomSheetHeaderTestsPage.OpenBottomSheetAsync();
        
        await bottomSheet.ClickNoneMode();
        Assert.False(bottomSheet.IsTopLeftOrTopRightDisplayed());
        
        await bottomSheet.ClickLeftMode();
        Assert.False(bottomSheet.IsTopLeftOrTopRightDisplayed());
        
        await bottomSheet.ClickRightMode();
        Assert.False(bottomSheet.IsTopLeftOrTopRightDisplayed());
        
        await bottomSheet.ClickLeftAndRightMode();
        Assert.False(bottomSheet.IsTopLeftOrTopRightDisplayed());
    }
    
    [Fact]
    [Trait("Category", "BottomSheetHeaderTests.AppearanceMode")]
    public async Task HeaderDisplayed_NoneMode_DoesNotAddButton()
    {
        var bottomSheet = await _bottomSheetHeaderTestsPage.OpenBottomSheetAsync();
        
        await bottomSheet.ShowHeaderAsync();
        
        await bottomSheet.ClickNoneMode();
        Assert.False(bottomSheet.IsTopLeftOrTopRightDisplayed());
    }
    
    [Fact]
    [Trait("Category", "BottomSheetHeaderTests.AppearanceMode")]
    public async Task HeaderDisplayed_LeftMode_ShowsOnlyLeftButton()
    {
        var bottomSheet = await _bottomSheetHeaderTestsPage.OpenBottomSheetAsync();

        await bottomSheet.ShowHeaderAsync();
        
        await bottomSheet.ClickLeftMode();
        
        Assert.True(bottomSheet.IsOnlyTopLeftDisplayed());
        
        bottomSheet.ClickTopLeftButton();

        Assert.True(bottomSheet.TopLeftClicked());
    }
    
    [Fact]
    [Trait("Category", "BottomSheetHeaderTests.AppearanceMode")]
    public async Task HeaderDisplayed_RightMode_ShowsOnlyRightButton()
    {
        var bottomSheet = await _bottomSheetHeaderTestsPage.OpenBottomSheetAsync();

        await bottomSheet.ShowHeaderAsync();
        
        await bottomSheet.ClickRightMode();
        Assert.True(bottomSheet.IsOnlyTopRightDisplayed());
        
        bottomSheet.ClickTopRightButton();

        Assert.True(bottomSheet.TopRightClicked());
    }
    
    [Fact]
    [Trait("Category", "BottomSheetHeaderTests.AppearanceMode")]
    public async Task HeaderDisplayed_LeftAndRightMode_ShowsLeftAndRightButton()
    {
        var bottomSheet = await _bottomSheetHeaderTestsPage.OpenBottomSheetAsync();

        await bottomSheet.ShowHeaderAsync();
        
        await bottomSheet.ClickLeftAndRightMode();
        Assert.True(bottomSheet.IsTopLeftAndRightDisplayed());
        
        bottomSheet.ClickTopLeftButton();
        bottomSheet.ClickTopRightButton();

        Assert.True(bottomSheet.TopLeftClicked());
        Assert.True(bottomSheet.TopRightClicked());
    }
    
    [Fact]
    [Trait("Category", "BottomSheetHeaderTests.AppearanceMode")]
    public async Task HeaderDisplayedWithLeftMode_ShowCloseButton_DisplaysCloseButtonAndHidesLeftButton()
    {
        var bottomSheet = await _bottomSheetHeaderTestsPage.OpenBottomSheetAsync();

        await bottomSheet.ShowHeaderAsync();
        await bottomSheet.ClickLeftMode();

        await bottomSheet.ShowCloseButtonAsync();
        await bottomSheet.ClickCloseButtonPositionLeftAsync();

        Assert.False(bottomSheet.TopLeftDisplayed());
        Assert.True(bottomSheet.CloseButtonDisplayed());
    }
    
    [Fact]
    [Trait("Category", "BottomSheetHeaderTests.AppearanceMode")]
    public async Task HeaderDisplayedWithRightMode_ShowCloseButton_DisplaysCloseButtonAndHidesRightButton()
    {
        var bottomSheet = await _bottomSheetHeaderTestsPage.OpenBottomSheetAsync();

        await bottomSheet.ShowHeaderAsync();
        await bottomSheet.ClickRightMode();

        await bottomSheet.ShowCloseButtonAsync();
        await bottomSheet.ClickCloseButtonPositionRightAsync();

        Assert.False(bottomSheet.TopRightDisplayed());
        Assert.True(bottomSheet.CloseButtonDisplayed());
    }
    
    [Fact]
    [Trait("Category", "BottomSheetHeaderTests.AppearanceMode")]
    public async Task HeaderWithLeftModeAndLeftVersionOne_SetLeftVersionTwo_DisplaysVersionTwoAndHidesVersionOne()
    {
        var bottomSheet = await _bottomSheetHeaderTestsPage.OpenBottomSheetAsync();

        await bottomSheet.ShowHeaderAsync();
        await bottomSheet.ClickLeftMode();
        await bottomSheet.ClickHeaderLeftButtonVersionOneAsync();
        await bottomSheet.ClickHeaderLeftButtonVersionTwoAsync();

        Assert.False(bottomSheet.TopLeftButtonVersionOneDisplayed());
        Assert.True(bottomSheet.TopLeftButtonVersionTwoDisplayed());
    }
    
    [Fact]
    [Trait("Category", "BottomSheetHeaderTests.AppearanceMode")]
    public async Task HeaderWithRightModeAndLeftVersionOne_SetRightVersionTwo_DisplaysVersionTwoAndHidesVersionOne()
    {
        var bottomSheet = await _bottomSheetHeaderTestsPage.OpenBottomSheetAsync();

        await bottomSheet.ShowHeaderAsync();
        await bottomSheet.ClickRightMode();
        await bottomSheet.ClickHeaderRightButtonVersionOneAsync();
        await bottomSheet.ClickHeaderRightButtonVersionTwoAsync();

        Assert.False(bottomSheet.TopRightButtonVersionOneDisplayed());
        Assert.True(bottomSheet.TopRightButtonVersionTwoDisplayed());
    }
    
    [Fact]
    [Trait("Category", "BottomSheetHeaderTests.AppearanceMode")]
    public async Task HeaderWithLeftAndRight_ShowCloseButton_ChangePosition_DisplaysCloseButtonAndButton()
    {
        var bottomSheet = await _bottomSheetHeaderTestsPage.OpenBottomSheetAsync();

        await bottomSheet.ShowHeaderAsync();
        await bottomSheet.ClickLeftAndRightMode();
        await bottomSheet.ShowCloseButtonAsync();
        await bottomSheet.ClickCloseButtonPositionLeftAsync();
        
        Assert.False(bottomSheet.TopLeftDisplayed());
        Assert.True(bottomSheet.TopRightDisplayed());

        await bottomSheet.ClickCloseButtonPositionRightAsync();
        
        Assert.True(bottomSheet.TopLeftDisplayed());
        Assert.False(bottomSheet.TopRightDisplayed());
        
        await bottomSheet.ClickCloseButtonPositionLeftAsync();
        
        Assert.False(bottomSheet.TopLeftDisplayed());
        Assert.True(bottomSheet.TopRightDisplayed());
    }
    
    [Fact]
    [Trait("Category", "BottomSheetHeaderTests.CustomHeader")]
    public async Task OpenBottomSheetCustomHeader()
    {
        var bottomSheet = await _bottomSheetHeaderTestsPage.OpenBottomSheetCustomHeaderAsync();

        Assert.True(bottomSheet.IsOpen());
    }
    
    [Fact]
    [Trait("Category", "BottomSheetHeaderTests.CustomHeader")]
    public async Task OpenBottomSheetCustomHeader_HeaderDisabled_HeaderHidden()
    {
        var bottomSheet = await _bottomSheetHeaderTestsPage.OpenBottomSheetCustomHeaderAsync();
        await bottomSheet.HideHeaderAsync();

        Assert.False(bottomSheet.HeaderDisplayed());
    }
    
    [Fact]
    [Trait("Category", "BottomSheetHeaderTests.CustomHeader")]
    public async Task OpenBottomSheetCustomHeader_HeaderEnabled_HeaderDisplayed()
    {
        var bottomSheet = await _bottomSheetHeaderTestsPage.OpenBottomSheetCustomHeaderAsync();
        await bottomSheet.ShowHeaderAsync();

        Assert.True(bottomSheet.HeaderDisplayed());
    }
    
    [Fact]
    [Trait("Category", "BottomSheetHeaderTests.CustomHeader")]
    public async Task OpenBottomSheetCustomHeader_ButtonClickable()
    {
        var bottomSheet = await _bottomSheetHeaderTestsPage.OpenBottomSheetCustomHeaderAsync();
        await bottomSheet.ShowHeaderAsync();

        bottomSheet.ClickButton();
        Assert.True(bottomSheet.ButtonClicked());
    }
    
    [Fact]
    [Trait("Category", "BottomSheetHeaderTests.CustomHeader")]
    public async Task OpenBottomSheetCustomHeader_ChangeBindingContext_HeaderContextChanged()
    {
        var bottomSheet = await _bottomSheetHeaderTestsPage.OpenBottomSheetCustomHeaderAsync();
        await bottomSheet.ShowHeaderAsync();

        await bottomSheet.ChangeBindingContextAsync();
        
        Assert.False(bottomSheet.HeaderDisplayed());
    }
}