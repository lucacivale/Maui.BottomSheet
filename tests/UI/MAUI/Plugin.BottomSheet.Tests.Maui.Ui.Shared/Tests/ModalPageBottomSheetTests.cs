using Plugin.BottomSheet.Tests.Maui.Ui.Application.Shared;
using Xunit.Abstractions;

// ReSharper disable once CheckNamespace
namespace Plugin.BottomSheet.Tests.Maui.Ui.Shared;

[Trait("Category", "ModalPageBottomSheetTests")]
public class ModalPageBottomSheetTests : BaseTest, IAsyncLifetime
{
    private readonly MainPage _mainPage;

    // Is set in init and never null
    private ModalPageBottomSheetTestsPage _modalPageBottomSheetTestsPage = null!;
    
    public ModalPageBottomSheetTests(AppiumSetupFixture appiumSetupFixture, ITestOutputHelper testOutputHelper)
        : base(appiumSetupFixture, testOutputHelper)
    {
        _mainPage = new MainPage(appiumSetupFixture.App);
    }
    
    public async Task InitializeAsync()
    {
        _modalPageBottomSheetTestsPage = await _mainPage.OpenModalPageBottomSheetTestsPage();
    }
    
    public async Task DisposeAsync()
    {
        if (App.TryFindElementByAutomationId(ModalPageBottomSheetTestsAutomationIds.BottomSheet, out _))
        {
            await GoBackAsync();
        }
        
        await GoBackAsync();
    }
    
    [Fact]
    [Trait("Category", "ModalPageBottomSheetTests")]
    public void ModalPageOpened_BottomSheetIsOpen_SheetDisplayedAboveModalPage()
    {
        Assert.True(_modalPageBottomSheetTestsPage.IsBottomSheetOpen());
    }
}