using System.Drawing;
using System.Globalization;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;

namespace Plugin.BottomSheet.Tests.Shared;

public class BottomSheetTestsBottomSheet : PomBase 
{
    public BottomSheetTestsBottomSheet(AppiumDriver driver) : base(driver)
    {
    }
    
    private IWebElement CornerRadiusElement => Wait.Until(d => d.FindElement(BottomSheetTestsAutomationIds.CornerRadius));
    
    private IWebElement MarginElement => Wait.Until(d => d.FindElement(BottomSheetTestsAutomationIds.Margin));
    
    private IWebElement PaddingElement => Wait.Until(d => d.FindElement(BottomSheetTestsAutomationIds.Padding));
    
    private IWebElement HalfExpandedRatioElement => Wait.Until(d => d.FindElement(BottomSheetTestsAutomationIds.HalfExpandedRatio));

    private IWebElement IsModalElement => Wait.Until(d => d.FindElement(BottomSheetTestsAutomationIds.IsModal));
    
    private IWebElement HasHandleElement => Wait.Until(d => d.FindElement(BottomSheetTestsAutomationIds.HasHandle));
    
    private IWebElement IsCancelableElement => Wait.Until(d => d.FindElement(BottomSheetTestsAutomationIds.IsCancelable));
    
    private IWebElement IsDraggableElement => Wait.Until(d => d.FindElement(BottomSheetTestsAutomationIds.IsDraggable));
    
    private IWebElement ChangeWindowBackgroundColorElement => Wait.Until(d => d.FindElement(BottomSheetTestsAutomationIds.ChangeWindowBackgroundColor));
    
    private IWebElement ChangeBackgroundColorElement => Wait.Until(d => d.FindElement(BottomSheetTestsAutomationIds.ChangeBackgroundColor));
    
    private IWebElement MediumStateElement => Wait.Until(d => d.FindElement(BottomSheetTestsAutomationIds.MediumState));
    
    private IWebElement LargeStateElement => Wait.Until(d => d.FindElement(BottomSheetTestsAutomationIds.LargeState));
    
    private IWebElement CurrentMediumStateElement => Wait.Until(d => d.FindElement(BottomSheetTestsAutomationIds.CurrentMediumState));
    
    private IWebElement CurrentLargeStateElement => Wait.Until(d => d.FindElement(BottomSheetTestsAutomationIds.CurrentLargeState));
    
    private IWebElement DesignContentElement => Wait.Until(d => d.FindElement(BottomSheetTestsAutomationIds.DesignBottomSheet));

    private IWebElement ContentElement => Wait.Until(d => d.FindElement(BottomSheetTestsAutomationIds.Content));

    private IWebElement HandleElement => Wait.Until(d => d.FindElement(AutomationIds.Handle));
    
    public bool IsOpen()
    {
        return App.TryFindElementByAutomationId(BottomSheetTestsAutomationIds.BottomSheet, out AppiumElement? element)
            && element.Displayed;
    }

    public async Task SetCornerRadiusAsync(float value)
    {
        CornerRadiusElement.Clear();
        CornerRadiusElement.SendKeys(value.ToString(CultureInfo.CurrentCulture));

        await WaitShortAsync();
    }
    
    public async Task SetMarginAsync(float value)
    {
        MarginElement.Clear();
        MarginElement.SendKeys(value.ToString(CultureInfo.CurrentCulture));

        await WaitShortAsync();
    }

    public Size Size()
    {
        return DesignContentElement.Size;
    }
    
    public Point Location()
    {
        return DesignContentElement.Location;
    }
    
    public async Task SetPaddingAsync(float value)
    {
        PaddingElement.Clear();
        PaddingElement.SendKeys(value.ToString(CultureInfo.CurrentCulture));

        await WaitShortAsync();
    }

    public async Task SetHalfExpandedRatio(float value)
    {
        HalfExpandedRatioElement.Clear();
        HalfExpandedRatioElement.SendKeys(Convert.ToString(value, CultureInfo.InvariantCulture));

        await WaitShortAsync();
    }

    
    public Size ContentSize()
    {
        return ContentElement.Size;
    }
    
    public Point ContentLocation()
    {
        return ContentElement.Location;
    }

    public Point HandleLocation()
    {
        return HandleElement.Location;
    }

    public Size HandleSize()
    {
        return HandleElement.Size;
    }
    
    public async Task SetNonModalAsync()
    {
        if (IsModalElement.IsChecked())
        {
            IsModalElement.Click();
            await WaitShortAsync();
        }
    }
    
    public async Task SetModalAsync()
    {
        if (IsModalElement.IsChecked() == false)
        {
            IsModalElement.Click();
            await WaitShortAsync();
        }
    }

    public async Task ShowHandleAsync()
    {
        if (HasHandleElement.IsChecked() == false)
        {
            HasHandleElement.Click();
            await WaitShortAsync();
        }
    }
    
    public async Task HideHandleAsync()
    {
        if (HasHandleElement.IsChecked())
        {
            HasHandleElement.Click();
            await WaitShortAsync();
        }
    }

    public bool HandleDisplayed()
    {
        return App.TryFindElement(AutomationIds.Handle, out AppiumElement? element)
            && element.Displayed;
    }
    
    public async Task IsCancelableAsync(bool isCancelable)
    {
        if (IsCancelableElement.IsChecked() != isCancelable)
        {
            IsCancelableElement.Click();
            await WaitShortAsync();
        }
    }
    
    public async Task IsDraggableAsync(bool isDraggable)
    {
        if (IsDraggableElement.IsChecked() != isDraggable)
        {
            IsDraggableElement.Click();
            await WaitShortAsync();
        }
    }

    public async Task ChangeStates(bool allowMedium, bool allowLarge)
    {
        if (MediumStateElement.IsChecked() != allowMedium)
        {
            MediumStateElement.Click();
        }

        if (LargeStateElement.IsChecked() != allowLarge)
        {
            LargeStateElement.Click();
        }

        await WaitShortAsync();
    }
}