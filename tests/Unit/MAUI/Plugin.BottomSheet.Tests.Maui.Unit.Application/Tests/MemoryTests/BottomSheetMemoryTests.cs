using System.Windows.Input;
using NSubstitute;
using Plugin.BottomSheet.Tests.Maui.Unit.Application.Mocks;
using Plugin.Maui.BottomSheet;
using Xunit.Abstractions;
using MauiThickness = Microsoft.Maui.Thickness;

namespace Plugin.BottomSheet.Tests.Maui.Unit.Application.Tests.MemoryTests;

public sealed class BottomSheetMemoryTests : MemoryBaseTest<EmptyContentPage, EmptyBottomSheet>
{
    public BottomSheetMemoryTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }

    [UIFact]
    public async Task BottomSheet_DoesNotLeak_WhenOpenedAndClosed()
    {
        if (TryGetTarget(out EmptyBottomSheet? bottomSheet))
        {
            bottomSheet.IsOpen = true;

            await Task.Delay(1000);
        
            bottomSheet.IsOpen = false;

            await Task.Delay(1000);
        }
    }
    
    [UIFact]
    public async Task BottomSheet_DoesNotLeak_WhenOpenedAndPageIsPopped()
    {
        if (TryGetTarget(out EmptyBottomSheet? bottomSheet))
        {
            bottomSheet.IsOpen = true;

            await Task.Delay(1000);
        }
    }
    
    [UIFact]
    public async Task BottomSheet_DoesNotLeak_WithEventsAndCommands()
    {
        if (TryGetTarget(out EmptyBottomSheet? bottomSheet))
        {
            ICommand command = Substitute.For<ICommand>();
            
            bottomSheet.OpeningCommand = command;
            bottomSheet.OpeningCommandParameter = new { };
            bottomSheet.ClosingCommand = command;
            bottomSheet.ClosingCommandParameter = new { };
            bottomSheet.OpenedCommand = command;
            bottomSheet.OpenedCommandParameter = new { };
            bottomSheet.ClosedCommand = command;
            bottomSheet.ClosedCommandParameter = new { };
            
            // Subscribe to events to create potential memory leaks
            bottomSheet.Opening += (_, _) => {};
            bottomSheet.Opened += (_, _) => {};
            bottomSheet.Closing += (_, _) => {};
            bottomSheet.Closed += (_, _) => {};
            
            // Simulate opening and closing
            bottomSheet.IsOpen = true;
            await Task.Delay(1000);

            bottomSheet.IsOpen = false;
            await Task.Delay(1000);
        }
    }

    [UITheory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task BottomSheet_DoesNotLeak_WhenIsCancelableIsToggled(bool isCancelable)
    {
        if (TryGetTarget(out EmptyBottomSheet? bottomSheet))
        {
            bottomSheet.IsCancelable = isCancelable;
            bottomSheet.IsOpen = true;
            await Task.Delay(1000);
            
            bottomSheet.IsOpen = false;
            await Task.Delay(1000);
        }
    }

    [UITheory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task BottomSheet_DoesNotLeak_WhenHasHandleIsSet(bool hasHandle)
    {
        if (TryGetTarget(out EmptyBottomSheet? bottomSheet))
        {
            bottomSheet.HasHandle = hasHandle;
            bottomSheet.IsOpen = true;
            await Task.Delay(1000);
            
            bottomSheet.IsOpen = false;
            await Task.Delay(1000);
        }
    }

    [UITheory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task BottomSheet_DoesNotLeak_WhenShowHeaderIsToggled(bool showHeader)
    {
        if (TryGetTarget(out EmptyBottomSheet? bottomSheet))
        {
            bottomSheet.ShowHeader = showHeader;
            bottomSheet.IsOpen = true;
            await Task.Delay(1000);

            bottomSheet.IsOpen = false;
            await Task.Delay(1000);
        }
    }

    [UITheory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task BottomSheet_DoesNotLeak_WhenIsModalIsToggled(bool isModal)
    {
        if (TryGetTarget(out EmptyBottomSheet? bottomSheet))
        {
            bottomSheet.IsModal = isModal;
            bottomSheet.IsOpen = true;
            await Task.Delay(1000);

            bottomSheet.IsOpen = false;
            await Task.Delay(1000);
        }
    }

    [UITheory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task BottomSheet_DoesNotLeak_WhenIsDraggableIsToggled(bool isDraggable)
    {
        if (TryGetTarget(out EmptyBottomSheet? bottomSheet))
        {
            bottomSheet.IsDraggable = isDraggable;
            bottomSheet.IsOpen = true;
            await Task.Delay(1000);

            bottomSheet.IsOpen = false;
            await Task.Delay(1000);
        }
    }

    [UIFact]
    public async Task BottomSheet_DoesNotLeak_WhenCornerRadiusIsChanged()
    {
        if (TryGetTarget(out EmptyBottomSheet? bottomSheet))
        {
            bottomSheet.CornerRadius = 20f;
            bottomSheet.IsOpen = true;
            await Task.Delay(1000);

            bottomSheet.IsOpen = false;
            await Task.Delay(1000);
        }
    }

    [UIFact]
    public async Task BottomSheet_DoesNotLeak_WhenWindowBackgroundColorIsChanged()
    {
        if (TryGetTarget(out EmptyBottomSheet? bottomSheet))
        {
            bottomSheet.WindowBackgroundColor = Colors.Red;
            bottomSheet.IsOpen = true;
            await Task.Delay(1000);

            bottomSheet.IsOpen = false;
            await Task.Delay(1000);
        }
    }

    [UIFact]
    public async Task BottomSheet_DoesNotLeak_WhenPeekHeightIsChanged()
    {
        if (TryGetTarget(out EmptyBottomSheet? bottomSheet))
        {
            bottomSheet.PeekHeight = 200;
            bottomSheet.IsOpen = true;
            await Task.Delay(1000);

            bottomSheet.IsOpen = false;
            await Task.Delay(1000);
        }
    }

    [UIFact]
    public async Task BottomSheet_DoesNotLeak_WhenPaddingIsChanged()
    {
        if (TryGetTarget(out EmptyBottomSheet? bottomSheet))
        {
            bottomSheet.Padding = new MauiThickness(20, 20);
            bottomSheet.IsOpen = true;
            await Task.Delay(1000);

            bottomSheet.IsOpen = false;
            await Task.Delay(1000);
        }
    }

    [UIFact]
    public async Task BottomSheet_DoesNotLeak_WhenBottomSheetStyleIsSet()
    {
        if (TryGetTarget(out EmptyBottomSheet? bottomSheet))
        {
            bottomSheet.BottomSheetStyle = new BottomSheetStyle();
            bottomSheet.IsOpen = true;
            await Task.Delay(1000);

            bottomSheet.IsOpen = false;
            await Task.Delay(1000);
        }
    }

    [UITheory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task BottomSheet_DoesNotLeak_WhenHeaderIsSet(bool showHeader)
    {
        if (TryGetTarget(out EmptyBottomSheet? bottomSheet))
        {
            bottomSheet.ShowHeader = showHeader;
            bottomSheet.Header = new BottomSheetHeader();
            bottomSheet.IsOpen = true;
            await Task.Delay(1000);

            bottomSheet.IsOpen = false;
            await Task.Delay(1000);
        }
    }

    [UIFact]
    public async Task BottomSheet_DoesNotLeak_WhenContentIsSet()
    {
        if (TryGetTarget(out EmptyBottomSheet? bottomSheet))
        {
            bottomSheet.Content = new BottomSheetContent()
            {
                Content = new Label(),
            };
            bottomSheet.IsOpen = true;
            await Task.Delay(1000);

            bottomSheet.IsOpen = false;
            await Task.Delay(1000);
        }
    }

    [UIFact]
    public async Task BottomSheet_DoesNotLeak_WhenStatesAreSet()
    {
        if (TryGetTarget(out EmptyBottomSheet? bottomSheet))
        {
            bottomSheet.States = new List<BottomSheetState> { BottomSheetState.Medium, BottomSheetState.Large };
            bottomSheet.IsOpen = true;
            await Task.Delay(1000);

            bottomSheet.IsOpen = false;
            await Task.Delay(1000);
        }
    }
    
    [UIFact]
    public async Task BottomSheet_DoesNotLeak_WhenCurrentStateIsChanged()
    {
        if (TryGetTarget(out EmptyBottomSheet? bottomSheet))
        {
            bottomSheet.CurrentState = BottomSheetState.Medium;
            bottomSheet.IsOpen = true;
            await Task.Delay(1000);

            bottomSheet.IsOpen = false;
            await Task.Delay(1000);
        }
    }
}