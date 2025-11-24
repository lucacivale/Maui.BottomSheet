using System.Drawing;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using Plugin.BottomSheet.Tests.Maui.Ui.Application.Shared;

// ReSharper disable once CheckNamespace
namespace Plugin.BottomSheet.Tests.Maui.Ui.Shared;

public class BottomSheetHeaderTestsBottomSheetBuiltInHeader : PomBase
{
    public BottomSheetHeaderTestsBottomSheetBuiltInHeader(AppiumDriver driver)
        : base(driver)
    {
    }
    
    private IWebElement ShowHeaderElement => Wait.Until(d => d.FindElement(BottomSheetHeaderTestsAutomationIds.ShowHeader));
    
    private IWebElement ShowCloseButtonElement => Wait.Until(d => d.FindElement(BottomSheetHeaderTestsAutomationIds.ShowCloseButton));
    
    private IWebElement HeaderElement => Wait.Until(d => d.FindElement(AutomationIds.HeaderElementId));
    
    private IWebElement HeaderCloseButtonElement => Wait.Until(d => d.FindElementByAutomationId(AutomationIds.HeaderCloseButtonElementId));
    
    private IWebElement TitleElement => Wait.Until(d => d.FindElement(BottomSheetHeaderTestsAutomationIds.Title));
    
    private IWebElement NoneModeElement => Wait.Until(d => d.FindElement(BottomSheetHeaderTestsAutomationIds.BottomSheetHeaderButtonAppearanceModeNone));
    
    private IWebElement LeftModeElement => Wait.Until(d => d.FindElement(BottomSheetHeaderTestsAutomationIds.BottomSheetHeaderButtonAppearanceModeLeft));
    
    private IWebElement RightModeElement => Wait.Until(d => d.FindElement(BottomSheetHeaderTestsAutomationIds.BottomSheetHeaderButtonAppearanceModeRight));
    
    private IWebElement LeftAndRightModeElement => Wait.Until(d => d.FindElement(BottomSheetHeaderTestsAutomationIds.BottomSheetHeaderButtonAppearanceModeLeftAndRight));
    
    private IWebElement HeaderLeftButtonVersionOneElement => Wait.Until(d => d.FindElement(BottomSheetHeaderTestsAutomationIds.HeaderLeftButtonVersionOne));
    
    private IWebElement HeaderLeftButtonVersionTwoElement => Wait.Until(d => d.FindElement(BottomSheetHeaderTestsAutomationIds.HeaderLeftButtonVersionTwo));
    
    private IWebElement HeaderRightButtonVersionOneElement => Wait.Until(d => d.FindElement(BottomSheetHeaderTestsAutomationIds.HeaderRightButtonVersionOne));
    
    private IWebElement HeaderRightButtonVersionTwoElement => Wait.Until(d => d.FindElement(BottomSheetHeaderTestsAutomationIds.HeaderRightButtonVersionTwo));
    
    private IWebElement CloseButtonPositionLeftElement => Wait.Until(d => d.FindElement(BottomSheetHeaderTestsAutomationIds.CloseButtonPositionLeft));
    
    private IWebElement CloseButtonPositionRightElement => Wait.Until(d => d.FindElement(BottomSheetHeaderTestsAutomationIds.CloseButtonPositionRight));
    
    private IWebElement ChangeBindingContextElement => Wait.Until(d => d.FindElement(BottomSheetHeaderTestsAutomationIds.ChangeBindingContext));
    
    private IWebElement CloseBottomSheetElement => Wait.Until(d => d.FindElement(BottomSheetHeaderTestsAutomationIds.CloseBottomSheet));
    
    private IWebElement IsCancelableElement => Wait.Until(d => d.FindElement(BottomSheetHeaderTestsAutomationIds.IsCancelable));
    
    public bool IsOpen()
    {
        return App.TryFindElementByAutomationId(BottomSheetHeaderTestsAutomationIds.BottomSheetBuiltInHeader, out AppiumElement? element)
            && element.Displayed;
    }
    
    public async Task ShowHeaderAsync()
    {
        if (ShowHeaderElement.IsChecked() == false)
        {
            ShowHeaderElement.Click();
            await WaitShortAsync();
        }
    }
    
    public async Task HideHeaderAsync()
    {
        if (ShowHeaderElement.IsChecked())
        {
            ShowHeaderElement.Click();
            await WaitShortAsync();
        }
    }

    public bool HeaderDisplayed()
    {
        return App.TryFindElement(AutomationIds.HeaderElementId, out AppiumElement? element)
            && element.Displayed;
    }
    
    public async Task ShowCloseButtonAsync()
    {
        if (ShowCloseButtonElement.IsChecked() == false)
        {
            ShowCloseButtonElement.Click();
            await WaitAsync();
        }
    }
    
    public async Task HideCloseButtonAsync()
    {
        if (ShowCloseButtonElement.IsChecked())
        {
            ShowCloseButtonElement.Click();
            await WaitAsync();
        }
    }

    public bool CloseButtonDisplayed()
    {
        return App.TryFindElementByAutomationId(AutomationIds.HeaderCloseButtonElementId, out AppiumElement? element)
            && element.Displayed;
    }
    
    public async Task ClickHeaderCloseButtonAsync()
    {
        HeaderCloseButtonElement.Click();
        await WaitAsync();
    }

    public bool IsCloseButtonAlignedLeft()
    {
        if (CloseButtonDisplayed() == false)
        {
            return false;
        }
    
        Point closeButtonLocation = HeaderCloseButtonElement.Location;
        Point headerLocation = HeaderElement.Location;
        Size headerSize = HeaderElement.Size;
    
        int headerMiddleX = headerLocation.X + (headerSize.Width / 3);
        return closeButtonLocation.X < headerMiddleX;
    }
    
    public bool IsCloseButtonAlignedRight()
    {
        if (CloseButtonDisplayed() == false)
        {
            return false;
        }
    
        Point closeButtonLocation = HeaderCloseButtonElement.Location;
        Point headerLocation = HeaderElement.Location;
        Size headerSize = HeaderElement.Size;
    
        int headerMiddleX = headerLocation.X + (headerSize.Width / 3);
        return closeButtonLocation.X > headerMiddleX;
    }
    
    public void EnterTitle(string title)
    {
        TitleElement.SendKeys(title);

        App.CloseKeyboard();
    }

    public bool TitleDisplayed(string title)
    {
        return App.TryFindElement(AutomationIds.HeaderTitleElementId, out AppiumElement? titleElement)
               && titleElement.Text.Equals(title);
    }
    
    public async Task ClickNoneMode()
    {
        NoneModeElement.Click();
        
        await WaitAsync();
    }

    public bool IsTopLeftOrTopRightDisplayed()
    {
        return TopLeftDisplayed()
            || TopRightDisplayed();
    }
    
    public async Task ClickLeftMode()
    {
        LeftModeElement.Click();

        await WaitAsync();
    }

    public bool IsOnlyTopLeftDisplayed()
    {
        return TopLeftDisplayed()
            && TopRightDisplayed() == false;
    }

    public async Task ClickRightMode()
    {
        RightModeElement.Click();
        
        await WaitAsync();
    }
    
    public bool IsOnlyTopRightDisplayed()
    {
        return TopLeftDisplayed() == false
            && TopRightDisplayed();
    }
    
    public async Task ClickLeftAndRightMode()
    {
        LeftAndRightModeElement.Click();
        
        await WaitAsync();
    }
    
    public bool IsTopLeftAndRightDisplayed()
    {
        return TopLeftDisplayed()
            && TopRightDisplayed();
    }

    public void ClickTopLeftButton()
    {
        TopLeftButton().Click();
    }
    
    public void ClickTopRightButton()
    {
        TopRightButton().Click();
    }

    public bool TopLeftClicked()
    {
        return TopLeftButton().Text == "Clicked";
    }
    
    public bool TopRightClicked()
    {
        return TopRightButton().Text == "Clicked";
    }

    public async Task ClickHeaderLeftButtonVersionOneAsync()
    {
        HeaderLeftButtonVersionOneElement.Click();

        await WaitShortAsync();
    }
    
    public bool TopLeftButtonVersionOneDisplayed()
    {
        return App.TryFindElement(BottomSheetHeaderTestsAutomationIds.TopLeftButtonOne, out AppiumElement? topLeft)
            && topLeft.Displayed;
    }
    
    public async Task ClickHeaderLeftButtonVersionTwoAsync()
    {
        HeaderLeftButtonVersionTwoElement.Click();
        
        await WaitShortAsync();
    }
    
    public bool TopLeftButtonVersionTwoDisplayed()
    {
        return App.TryFindElement(BottomSheetHeaderTestsAutomationIds.TopLeftButtonTwo, out AppiumElement? topLeft)
            && topLeft.Displayed;
    }
    
    public async Task ClickHeaderRightButtonVersionOneAsync()
    {
        HeaderRightButtonVersionOneElement.Click();
        
        await WaitShortAsync();
    }
    
    public bool TopRightButtonVersionOneDisplayed()
    {
        return App.TryFindElement(BottomSheetHeaderTestsAutomationIds.TopRightButtonOne, out AppiumElement? topRight)
            && topRight.Displayed;
    }
    
    public async Task ClickHeaderRightButtonVersionTwoAsync()
    {
        HeaderRightButtonVersionTwoElement.Click();
        
        await WaitShortAsync();
    }
    
    public bool TopRightButtonVersionTwoDisplayed()
    {
        return App.TryFindElement(BottomSheetHeaderTestsAutomationIds.TopRightButtonTwo, out AppiumElement? topRight)
            && topRight.Displayed;
    }
    
    public async Task ClickCloseButtonPositionLeftAsync()
    {
        CloseButtonPositionLeftElement.Click();
        await WaitAsync();
    }
    
    public async Task ClickCloseButtonPositionRightAsync()
    {
        CloseButtonPositionRightElement.Click();
        await WaitAsync();
    }
    
    public async Task ChangeBindingContextAsync()
    {
        ChangeBindingContextElement.Click();
        
        await WaitAsync();
    }
    
    public async Task ClickCloseButtonAsync()
    {
        CloseBottomSheetElement.Click();

        await WaitAsync();
    }

    private AppiumElement TopLeftButton()
    {
        App.TryFindElement(BottomSheetHeaderTestsAutomationIds.TopLeftButtonOne, out AppiumElement? topLeftOne);
        App.TryFindElement(BottomSheetHeaderTestsAutomationIds.TopLeftButtonTwo, out AppiumElement? topLeftTwo);
        
        return topLeftOne ?? topLeftTwo ?? throw new Exception("TopLeftButton not found");
    }
    
    private AppiumElement TopRightButton()
    {
        App.TryFindElement(BottomSheetHeaderTestsAutomationIds.TopRightButtonOne, out AppiumElement? topRightOne);
        App.TryFindElement(BottomSheetHeaderTestsAutomationIds.TopRightButtonTwo, out AppiumElement? topRighTwo);
        
        return topRightOne ?? topRighTwo ?? throw new Exception("TopRightButton not found");
    }
    
    public bool TopLeftDisplayed()
    {
        return (App.TryFindElement(BottomSheetHeaderTestsAutomationIds.TopLeftButtonOne, out AppiumElement? topLeftOne)
                && topLeftOne.Displayed)
            || (App.TryFindElement(BottomSheetHeaderTestsAutomationIds.TopLeftButtonTwo, out AppiumElement? topLeftTwo)
                && topLeftTwo.Displayed);
    }

    public bool TopRightDisplayed()
    {
        return (App.TryFindElement(BottomSheetHeaderTestsAutomationIds.TopRightButtonOne, out AppiumElement? topRightOne)
                && topRightOne.Displayed)
            || (App.TryFindElement(BottomSheetHeaderTestsAutomationIds.TopRightButtonTwo, out AppiumElement? topRightTwo)
                && topRightTwo.Displayed);
    }
    
    public async Task NonCancelableAsync()
    {
        if (IsCancelableElement.IsChecked())
        {
            IsCancelableElement.Click();
            await WaitShortAsync();
        }
    }
    
    public async Task IsCancelableAsync()
    {
        if (IsCancelableElement.IsChecked() == false)
        {
            IsCancelableElement.Click();
            await WaitShortAsync();
        }
    }
}