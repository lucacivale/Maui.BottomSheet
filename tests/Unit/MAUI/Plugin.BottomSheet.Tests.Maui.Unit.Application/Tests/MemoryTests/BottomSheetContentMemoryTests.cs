using Plugin.BottomSheet.Tests.Maui.Unit.Application.Mocks;
using Plugin.BottomSheet.Tests.Maui.Unit.Application.Mocks.ViewModels;
using Plugin.Maui.BottomSheet;
using Xunit.Abstractions;

namespace Plugin.BottomSheet.Tests.Maui.Unit.Application.Tests.MemoryTests;

public class BottomSheetContentMemoryTests : MemoryBaseTest<EmptyContentPage, EmptyBottomSheet>
{
    private WeakReference<BottomSheetContent>? _weakContent;
    private WeakReference<BottomSheetContent>? _weakContent2;

    
    public BottomSheetContentMemoryTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }
    
    async protected override Task Disposing()
    {
        await base.Disposing();

        Assert.False(_weakContent?.TryGetTarget(out BottomSheetContent? _) ?? false, $"{_weakContent} should be garbage collected");
        Assert.False(_weakContent2?.TryGetTarget(out BottomSheetContent? _) ?? false, $"{_weakContent2} should be garbage collected");
    }
    
    [UIFact]
    public async Task BottomSheet_DoesNotLeak_WithContent()
    {
        if (TryGetTarget(out EmptyBottomSheet? bottomSheet))
        {
            BottomSheetContent content = new()
            {
                Content = new VerticalStackLayout
                {
                    new Label
                    {
                        Text = "Test",
                    },
                },
            };

            bottomSheet.Content = content;
            bottomSheet.IsOpen = true;
            
            _weakContent = new WeakReference<BottomSheetContent>(content);
            
            await Task.Delay(1000);
            
            bottomSheet.IsOpen = false;

            await Task.Delay(1000);
        }
    }
    
    [UIFact]
    public async Task BottomSheet_DoesNotLeak_WhenContentIsChanged()
    {
        if (TryGetTarget(out EmptyBottomSheet? bottomSheet))
        {
            bottomSheet.Content = new BottomSheetContent
            {
                Content = new VerticalStackLayout
                {
                    new Label
                    {
                        Text = "Test",
                    },
                },
            };
            bottomSheet.IsOpen = true;
            
            _weakContent = new WeakReference<BottomSheetContent>(bottomSheet.Content);
            
            await Task.Delay(1000);
            
            BottomSheetContent content2 = new()
            {
                Content = new VerticalStackLayout
                {
                    new Label
                    {
                        Text = "Test2",
                    },
                },
            };
            
            _weakContent2 = new WeakReference<BottomSheetContent>(content2);
            
            bottomSheet.Content = content2;

            await ForceGc();
            
            Assert.False(_weakContent?.TryGetTarget(out BottomSheetContent? _), $"{_weakContent} should be garbage collected");
            
            bottomSheet.IsOpen = false;

            await Task.Delay(1000);
        }
    }
    
    [UIFact]
    public async Task BottomSheet_WithBindingContext_DoesNotLeak_WithContent()
    {
        if (TryGetTarget(out EmptyBottomSheet? bottomSheet))
        {
            bottomSheet.BindingContext = new ViewModelA();

            Label label = new();
            label.SetBinding(Label.TextProperty, static (ViewModelA vm) => vm.Title);
            
            BottomSheetContent content = new()
            {
                Content = new VerticalStackLayout
                {
                    label,
                },
            };
            
            bottomSheet.Content = content;
            bottomSheet.IsOpen = true;
            
            _weakContent = new WeakReference<BottomSheetContent>(content);
            
            await Task.Delay(1000);
            
            bottomSheet.IsOpen = false;

            await Task.Delay(1000);
        }
    }
    
    [UIFact]
    public async Task BottomSheet_WithBindingContext_DoesNotLeak_WhenContentIsChanged()
    {
        if (TryGetTarget(out EmptyBottomSheet? bottomSheet))
        {
            bottomSheet.BindingContext = new ViewModelA();
            
            Label label = new();
            label.SetBinding(Label.TextProperty, static (ViewModelA vm) => vm.Title);
            bottomSheet.Content = new BottomSheetContent
            {
                Content = new VerticalStackLayout
                {
                    label,
                },
            };
            bottomSheet.IsOpen = true;
            
            _weakContent = new WeakReference<BottomSheetContent>(bottomSheet.Content);
            
            await Task.Delay(1000);
            
            Label label2 = new();
            label2.SetBinding(Label.TextProperty, static (ViewModelA vm) => vm.Title);
            BottomSheetContent content2 = new()
            {
                Content = new VerticalStackLayout
                {
                    label2,
                },
            };
            
            _weakContent2 = new WeakReference<BottomSheetContent>(content2);
            
            bottomSheet.Content = content2;

            await ForceGc();
            
            Assert.False(_weakContent?.TryGetTarget(out BottomSheetContent? _), $"{_weakContent} should be garbage collected");
            
            bottomSheet.IsOpen = false;

            await Task.Delay(1000);
        }
    }
    
    [UIFact]
    public async Task BottomSheet_WithBindingContext_DoesNotLeak_WhenBindingContextChanged()
    {
        if (TryGetTarget(out EmptyBottomSheet? bottomSheet))
        {
            bottomSheet.BindingContext = new ViewModelA();
            WeakReference<ViewModelA> weakViewModelA = new((ViewModelA)bottomSheet.BindingContext);
            
            Label label = new();
            label.SetBinding(Label.TextProperty, nameof(ViewModelA.Title));
            bottomSheet.Content = new BottomSheetContent
            {
                Content = new VerticalStackLayout
                {
                    label,
                },
            };
            bottomSheet.IsOpen = true;
            
            _weakContent = new WeakReference<BottomSheetContent>(bottomSheet.Content);
            
            await Task.Delay(1000);

            bottomSheet.BindingContext = new ViewModelB();
            
            await ForceGc();
            
            Assert.False(weakViewModelA.TryGetTarget(out ViewModelA? _), $"{weakViewModelA} should be garbage collected");
            
            bottomSheet.IsOpen = false;

            await Task.Delay(1000);
        }
    }
}