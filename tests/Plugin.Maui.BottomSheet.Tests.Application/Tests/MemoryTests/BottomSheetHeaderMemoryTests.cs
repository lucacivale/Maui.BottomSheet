using Plugin.Maui.BottomSheet.Tests.Application.Mocks;
using Plugin.Maui.BottomSheet.Tests.Application.Mocks.ViewModels;
using Xunit.Abstractions;

namespace Plugin.Maui.BottomSheet.Tests.Application.Tests.MemoryTests;

public sealed class BottomSheetHeaderMemoryTests : MemoryBaseTest<EmptyContentPage, EmptyBottomSheet>
{
    private WeakReference<BottomSheetHeader>? _weakHeader;
    private WeakReference<BottomSheetHeader>? _weakHeader2;
    
    public BottomSheetHeaderMemoryTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }

    async protected override Task Disposing()
    {
        await base.Disposing();

        Assert.False(_weakHeader?.TryGetTarget(out BottomSheetHeader? _) ?? false, $"{_weakHeader} should be garbage collected");
        Assert.False(_weakHeader2?.TryGetTarget(out BottomSheetHeader? _) ?? false, $"{_weakHeader2} should be garbage collected");
    }

    [UITheory]
    [InlineData(BottomSheetHeaderButtonAppearanceMode.LeftAndRightButton, false, CloseButtonPosition.TopLeft)]
    [InlineData(BottomSheetHeaderButtonAppearanceMode.LeftAndRightButton, false, CloseButtonPosition.TopRight)]
    [InlineData(BottomSheetHeaderButtonAppearanceMode.LeftAndRightButton, true, CloseButtonPosition.TopLeft)]
    [InlineData(BottomSheetHeaderButtonAppearanceMode.LeftAndRightButton, true, CloseButtonPosition.TopRight)]
    [InlineData(BottomSheetHeaderButtonAppearanceMode.LeftButton, false, CloseButtonPosition.TopLeft)]
    [InlineData(BottomSheetHeaderButtonAppearanceMode.LeftButton, false, CloseButtonPosition.TopRight)]
    [InlineData(BottomSheetHeaderButtonAppearanceMode.LeftButton, true, CloseButtonPosition.TopLeft)]
    [InlineData(BottomSheetHeaderButtonAppearanceMode.LeftButton, true, CloseButtonPosition.TopRight)]
    [InlineData(BottomSheetHeaderButtonAppearanceMode.RightButton, false, CloseButtonPosition.TopLeft)]
    [InlineData(BottomSheetHeaderButtonAppearanceMode.RightButton, false, CloseButtonPosition.TopRight)]
    [InlineData(BottomSheetHeaderButtonAppearanceMode.RightButton, true, CloseButtonPosition.TopLeft)]
    [InlineData(BottomSheetHeaderButtonAppearanceMode.RightButton, true, CloseButtonPosition.TopRight)]
    public async Task BottomSheet_WithBindingContext_DoesNotLeak_WhenBuiltInHeaderIsShowed(
        BottomSheetHeaderButtonAppearanceMode headerAppearance,
        bool showCloseButton,
        CloseButtonPosition closeButtonPosition)
    {
        if (TryGetTarget(out EmptyBottomSheet? bottomSheet))
        {
            bottomSheet.BindingContext = new ViewModelA();

            Button topLefButton = new();
            topLefButton.SetBinding(Button.TextProperty, nameof(ViewModelA.TopLeftText));
            topLefButton.SetBinding(Button.CommandProperty, nameof(ViewModelA.TopLeftCommand));

            Button topRightButton = new();
            topRightButton.SetBinding(Button.TextProperty, nameof(ViewModelA.TopRightText));
            topRightButton.SetBinding(Button.CommandProperty, nameof(ViewModelA.TopRightCommand));
            
            BottomSheetHeader header = new()
            {
                HeaderAppearance = headerAppearance,
                ShowCloseButton = showCloseButton,
                CloseButtonPosition = closeButtonPosition,
                TopLeftButton = topLefButton,
                TopRightButton = topRightButton,
            };
            header.SetBinding(BottomSheetHeader.TitleTextProperty, nameof(ViewModelA.Title));
            
            _weakHeader = new WeakReference<BottomSheetHeader>(header);

            bottomSheet.Header = header;
            bottomSheet.ShowHeader = true;
            bottomSheet.IsOpen = true;
        
            await Task.Delay(1000);

            bottomSheet.IsOpen = false;
            
            await Task.Delay(1000);
        }
    }
    
    [UITheory]
    [InlineData(BottomSheetHeaderButtonAppearanceMode.LeftAndRightButton, false, CloseButtonPosition.TopLeft)]
    [InlineData(BottomSheetHeaderButtonAppearanceMode.LeftAndRightButton, false, CloseButtonPosition.TopRight)]
    [InlineData(BottomSheetHeaderButtonAppearanceMode.LeftAndRightButton, true, CloseButtonPosition.TopLeft)]
    [InlineData(BottomSheetHeaderButtonAppearanceMode.LeftAndRightButton, true, CloseButtonPosition.TopRight)]
    [InlineData(BottomSheetHeaderButtonAppearanceMode.LeftButton, false, CloseButtonPosition.TopLeft)]
    [InlineData(BottomSheetHeaderButtonAppearanceMode.LeftButton, false, CloseButtonPosition.TopRight)]
    [InlineData(BottomSheetHeaderButtonAppearanceMode.LeftButton, true, CloseButtonPosition.TopLeft)]
    [InlineData(BottomSheetHeaderButtonAppearanceMode.LeftButton, true, CloseButtonPosition.TopRight)]
    [InlineData(BottomSheetHeaderButtonAppearanceMode.RightButton, false, CloseButtonPosition.TopLeft)]
    [InlineData(BottomSheetHeaderButtonAppearanceMode.RightButton, false, CloseButtonPosition.TopRight)]
    [InlineData(BottomSheetHeaderButtonAppearanceMode.RightButton, true, CloseButtonPosition.TopLeft)]
    [InlineData(BottomSheetHeaderButtonAppearanceMode.RightButton, true, CloseButtonPosition.TopRight)]
    public async Task BottomSheet_WithBindingContext_DoesNotLeak_WhenBuiltInHeaderIsChanged(
        BottomSheetHeaderButtonAppearanceMode headerAppearance,
        bool showCloseButton,
        CloseButtonPosition closeButtonPosition)
    {
        if (TryGetTarget(out EmptyBottomSheet? bottomSheet))
        {
            bottomSheet.BindingContext = new ViewModelA();

            Button topLefButton = new();
            topLefButton.SetBinding(Button.TextProperty, nameof(ViewModelA.TopLeftText));
            topLefButton.SetBinding(Button.CommandProperty, nameof(ViewModelA.TopLeftCommand));

            Button topRightButton = new();
            topRightButton.SetBinding(Button.TextProperty, nameof(ViewModelA.TopRightText));
            topRightButton.SetBinding(Button.CommandProperty, nameof(ViewModelA.TopRightCommand));
            
            bottomSheet.Header = new BottomSheetHeader
            {
                HeaderAppearance = headerAppearance,
                ShowCloseButton = showCloseButton,
                CloseButtonPosition = closeButtonPosition,
                TopLeftButton = topLefButton,
                TopRightButton = topRightButton,
            };
            bottomSheet.Header.SetBinding(BottomSheetHeader.TitleTextProperty, nameof(ViewModelA.Title));
            
            _weakHeader = new WeakReference<BottomSheetHeader>(bottomSheet.Header);

            bottomSheet.ShowHeader = true;
            bottomSheet.IsOpen = true;
        
            await Task.Delay(1000);

            Button topLefButton2 = new();
            topLefButton2.SetBinding(Button.TextProperty, nameof(ViewModelA.TopLeftText));
            topLefButton2.SetBinding(Button.CommandProperty, nameof(ViewModelA.TopLeftCommand));

            Button topRightButton2 = new();
            topRightButton2.SetBinding(Button.TextProperty, nameof(ViewModelA.TopRightText));
            topRightButton2.SetBinding(Button.CommandProperty, nameof(ViewModelA.TopRightCommand));
            
            bottomSheet.Header = new BottomSheetHeader
            {
                HeaderAppearance = headerAppearance,
                ShowCloseButton = showCloseButton,
                CloseButtonPosition = closeButtonPosition,
                TopLeftButton = topLefButton2,
                TopRightButton = topRightButton2,
            };
            bottomSheet.Header.SetBinding(BottomSheetHeader.TitleTextProperty, nameof(ViewModelA.Title));
            
            _weakHeader2 = new WeakReference<BottomSheetHeader>(bottomSheet.Header);
            
            await ForceGc();
            
            Assert.False(_weakHeader.TryGetTarget(out BottomSheetHeader? _), $"{_weakHeader} should be garbage collected");
            
            bottomSheet.IsOpen = false;
            
            await Task.Delay(1000);
        }
    }
    
    [UITheory]
    [InlineData(BottomSheetHeaderButtonAppearanceMode.LeftAndRightButton, false, CloseButtonPosition.TopLeft)]
    [InlineData(BottomSheetHeaderButtonAppearanceMode.LeftAndRightButton, false, CloseButtonPosition.TopRight)]
    [InlineData(BottomSheetHeaderButtonAppearanceMode.LeftAndRightButton, true, CloseButtonPosition.TopLeft)]
    [InlineData(BottomSheetHeaderButtonAppearanceMode.LeftAndRightButton, true, CloseButtonPosition.TopRight)]
    [InlineData(BottomSheetHeaderButtonAppearanceMode.LeftButton, false, CloseButtonPosition.TopLeft)]
    [InlineData(BottomSheetHeaderButtonAppearanceMode.LeftButton, false, CloseButtonPosition.TopRight)]
    [InlineData(BottomSheetHeaderButtonAppearanceMode.LeftButton, true, CloseButtonPosition.TopLeft)]
    [InlineData(BottomSheetHeaderButtonAppearanceMode.LeftButton, true, CloseButtonPosition.TopRight)]
    [InlineData(BottomSheetHeaderButtonAppearanceMode.RightButton, false, CloseButtonPosition.TopLeft)]
    [InlineData(BottomSheetHeaderButtonAppearanceMode.RightButton, false, CloseButtonPosition.TopRight)]
    [InlineData(BottomSheetHeaderButtonAppearanceMode.RightButton, true, CloseButtonPosition.TopLeft)]
    [InlineData(BottomSheetHeaderButtonAppearanceMode.RightButton, true, CloseButtonPosition.TopRight)]
    public async Task BottomSheet_WithBindingContext_DoesNotLeak_WhenBuiltInHeaderBindingContextIsChanged(
        BottomSheetHeaderButtonAppearanceMode headerAppearance,
        bool showCloseButton,
        CloseButtonPosition closeButtonPosition)
    {
        if (TryGetTarget(out EmptyBottomSheet? bottomSheet))
        {
            bottomSheet.BindingContext = new ViewModelA();
            WeakReference<ViewModelA> weakViewModelA = new((ViewModelA)bottomSheet.BindingContext);
            
            Button topLefButton = new();
            topLefButton.SetBinding(Button.TextProperty, nameof(ViewModelA.TopLeftText));
            topLefButton.SetBinding(Button.CommandProperty, nameof(ViewModelA.TopLeftCommand));

            Button topRightButton = new();
            topRightButton.SetBinding(Button.TextProperty, nameof(ViewModelA.TopRightText));
            topRightButton.SetBinding(Button.CommandProperty, nameof(ViewModelA.TopRightCommand));
            
            bottomSheet.Header = new BottomSheetHeader
            {
                HeaderAppearance = headerAppearance,
                ShowCloseButton = showCloseButton,
                CloseButtonPosition = closeButtonPosition,
                TopLeftButton = topLefButton,
                TopRightButton = topRightButton,
            };
            bottomSheet.Header.SetBinding(BottomSheetHeader.TitleTextProperty, nameof(ViewModelA.Title));
            
            _weakHeader = new WeakReference<BottomSheetHeader>(bottomSheet.Header);
            
            bottomSheet.ShowHeader = true;
            bottomSheet.IsOpen = true;
        
            await Task.Delay(1000);

            bottomSheet.BindingContext = new ViewModelB();
            
            await ForceGc();

            Assert.False(weakViewModelA.TryGetTarget(out ViewModelA? _), $"{weakViewModelA} should be garbage collected");
            
            bottomSheet.IsOpen = false;
            
            await Task.Delay(1000);
        }
    }
    
    [UIFact]
    public async Task BottomSheet_DoesNotLeak_WhenCustomHeaderTemplateIsShowed()
    {
        if (TryGetTarget(out EmptyBottomSheet? bottomSheet))
        {
            BottomSheetHeader header = new()
            {
                ContentTemplate = new DataTemplate(() =>
                {
                    Label label = new()
                    {
                        HorizontalOptions = LayoutOptions.Center,
                        Text = "Test",
                    };

                    VerticalStackLayout stackLayout = [label];

                    return stackLayout;
                }),
            };
            _weakHeader = new WeakReference<BottomSheetHeader>(header);

            bottomSheet.Header = header;
            bottomSheet.ShowHeader = true;
            bottomSheet.IsOpen = true;
            
            await Task.Delay(1000);

            bottomSheet.IsOpen = false;

            await Task.Delay(1000);
        }
    }
    
    [UIFact]
    public async Task BottomSheet_DoesNotLeak_WhenCustomHeaderIsShowed()
    {
        if (TryGetTarget(out EmptyBottomSheet? bottomSheet))
        {
            Label label = new()
            {
                HorizontalOptions = LayoutOptions.Center,
                Text = "Test",
            };

            VerticalStackLayout stackLayout = [label];

            BottomSheetHeader header = new()
            {
                Content = stackLayout,
            };

            _weakHeader = new WeakReference<BottomSheetHeader>(header);
            
            bottomSheet.Header = header;
            bottomSheet.ShowHeader = true;
            bottomSheet.IsOpen = true;
            
            await Task.Delay(1000);

            bottomSheet.IsOpen = false;
            
            await Task.Delay(1000);
        }
    }
    
        
    [UIFact]
    public async Task BottomSheet_WithBindingContext_DoesNotLeak_WhenCustomHeaderTemplateIsShowed()
    {
        if (TryGetTarget(out EmptyBottomSheet? bottomSheet))
        {
            bottomSheet.BindingContext = new ViewModelA();
            
            BottomSheetHeader header = new()
            {
                ContentTemplate = new DataTemplate(() =>
                {
                    Label label = new()
                    {
                        HorizontalOptions = LayoutOptions.Center,
                    };
                    
                    label.SetBinding(Label.TextProperty, nameof(ViewModelA.Title));

                    VerticalStackLayout stackLayout = [label];

                    return stackLayout;
                }),
            };
            _weakHeader = new WeakReference<BottomSheetHeader>(header);

            bottomSheet.Header = header;
            bottomSheet.ShowHeader = true;
            bottomSheet.IsOpen = true;
            
            await Task.Delay(1000);

            bottomSheet.IsOpen = false;

            await Task.Delay(1000);
        }
    }
    
    [UIFact]
    public async Task BottomSheet_WithCustomHeaderTemplate_WithBindingContext_DoesNotLeak_WhenBindingContextIsChanged()
    {
        if (TryGetTarget(out EmptyBottomSheet? bottomSheet))
        {
            bottomSheet.BindingContext = new ViewModelA();
            WeakReference<ViewModelA> weakViewModelA = new((ViewModelA)bottomSheet.BindingContext);
            
            BottomSheetHeader header = new()
            {
                ContentTemplate = new DataTemplate(() =>
                {
                    Label label = new()
                    {
                        HorizontalOptions = LayoutOptions.Center,
                    };
                    
                    label.SetBinding(Label.TextProperty, nameof(ViewModelA.Title));

                    VerticalStackLayout stackLayout = [label];

                    return stackLayout;
                }),
            };
            _weakHeader = new WeakReference<BottomSheetHeader>(header);

            bottomSheet.Header = header;
            bottomSheet.ShowHeader = true;
            bottomSheet.IsOpen = true;
            
            await Task.Delay(1000);
            
            bottomSheet.BindingContext = new ViewModelB();

            await ForceGc();
            Assert.False(weakViewModelA.TryGetTarget(out ViewModelA? _), $"{weakViewModelA} should be garbage collected");

            bottomSheet.IsOpen = false;

            await Task.Delay(1000);
        }
    }
    
    [UIFact]
    public async Task BottomSheet_WithBindingContext_DoesNotLeak_WhenCustomHeaderIsShowed()
    {
        if (TryGetTarget(out EmptyBottomSheet? bottomSheet))
        {
            bottomSheet.BindingContext = new ViewModelA();

            Label label = new()
            {
                HorizontalOptions = LayoutOptions.Center,
            };
            label.SetBinding(Label.TextProperty, nameof(ViewModelA.Title));

            VerticalStackLayout stackLayout = [label];

            BottomSheetHeader header = new()
            {
                Content = stackLayout,
            };
            _weakHeader = new WeakReference<BottomSheetHeader>(header);

            bottomSheet.Header = header;
            bottomSheet.ShowHeader = true;
            bottomSheet.IsOpen = true;
            
            await Task.Delay(1000);

            bottomSheet.IsOpen = false;
            
            await Task.Delay(1000);
        }
    }
    
    [UIFact]
    public async Task BottomSheet_WithCustomContent_WithBindingContext_DoesNotLeak_WhenBindingContextIsChanged()
    {
        if (TryGetTarget(out EmptyBottomSheet? bottomSheet))
        {
            bottomSheet.BindingContext = new ViewModelA();
            WeakReference<ViewModelA> weakViewModelA = new((ViewModelA)bottomSheet.BindingContext);

            Label label = new()
            {
                HorizontalOptions = LayoutOptions.Center,
            };
            label.SetBinding(Label.TextProperty, nameof(ViewModelA.Title));

            VerticalStackLayout stackLayout = [label];

            BottomSheetHeader header = new()
            {
                Content = stackLayout,
            };
            _weakHeader = new WeakReference<BottomSheetHeader>(header);

            bottomSheet.Header = header;
            bottomSheet.ShowHeader = true;
            bottomSheet.IsOpen = true;
            
            await Task.Delay(1000);

            bottomSheet.BindingContext = new ViewModelB();
            await ForceGc();
            Assert.False(weakViewModelA.TryGetTarget(out ViewModelA? _), $"{weakViewModelA} should be garbage collected");
            
            bottomSheet.IsOpen = false;
            
            await Task.Delay(1000);
        }
    }
    
    [UIFact]
    public async Task BottomSheet_DoesNotLeak_WhenHeaderIsChanged()
    {
        if (TryGetTarget(out EmptyBottomSheet? bottomSheet))
        {
            Label label = new()
            {
                HorizontalOptions = LayoutOptions.Center,
                Text = "Test",
            };

            VerticalStackLayout stackLayout = [label];

            bottomSheet.Header = new BottomSheetHeader
            {
                Content = stackLayout,
            };
            bottomSheet.ShowHeader = true;
            bottomSheet.IsOpen = true;
            
            _weakHeader = new WeakReference<BottomSheetHeader>(bottomSheet.Header);
            
            await Task.Delay(1000);

            Label label2 = new()
            {
                HorizontalOptions = LayoutOptions.Center,
                Text = "Test2",
            };

            VerticalStackLayout stackLayout2 = [label2];

            BottomSheetHeader header2 = new()
            {
                Content = stackLayout2,
            };
            _weakHeader2 = new WeakReference<BottomSheetHeader>(header2);
            
            bottomSheet.Header = header2;

            await ForceGc();
            
            Assert.False(_weakHeader.TryGetTarget(out BottomSheetHeader? _), $"{_weakHeader} should be garbage collected");

            bottomSheet.IsOpen = false;

            await Task.Delay(1000);
        }
    }
    
    [UIFact]
    public async Task BottomSheet_WithBindingContext_DoesNotLeak_WhenHeaderIsChanged()
    {
        if (TryGetTarget(out EmptyBottomSheet? bottomSheet))
        {
            bottomSheet.BindingContext = new ViewModelA();
            
            Label label = new()
            {
                HorizontalOptions = LayoutOptions.Center,
            };
            label.SetBinding(Label.TextProperty, nameof(ViewModelA.Title));
            
            VerticalStackLayout stackLayout = [label];
            
            bottomSheet.Header = new BottomSheetHeader
            {
                Content = stackLayout,
            };
            bottomSheet.ShowHeader = true;
            bottomSheet.IsOpen = true;
            
            _weakHeader = new WeakReference<BottomSheetHeader>(bottomSheet.Header);
            
            await Task.Delay(1000);

            Label label2 = new()
            {
                HorizontalOptions = LayoutOptions.Center,
            };
            label2.SetBinding(Label.TextProperty, nameof(ViewModelA.Title));

            VerticalStackLayout stackLayout2 = [label2];

            BottomSheetHeader header2 = new()
            {
                Content = stackLayout2,
            };
            _weakHeader2 = new WeakReference<BottomSheetHeader>(header2);

            bottomSheet.Header = header2;
            
            await ForceGc();
            
            Assert.False(_weakHeader.TryGetTarget(out BottomSheetHeader? _), $"{_weakHeader} should be garbage collected");
            
            bottomSheet.IsOpen = false;

            await Task.Delay(1000);
        }
    }
}