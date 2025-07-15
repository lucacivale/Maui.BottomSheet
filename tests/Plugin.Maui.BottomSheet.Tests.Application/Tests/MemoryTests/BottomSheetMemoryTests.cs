using System.Windows.Input;
using NSubstitute;
using Plugin.Maui.BottomSheet.Tests.Application.Mocks;
using Xunit.Abstractions;

namespace Plugin.Maui.BottomSheet.Tests.Application.Tests.MemoryTests;

public class BottomSheetMemoryTests : MemoryBaseTest<EmptyContentPage, EmptyBottomSheet>
{
    public BottomSheetMemoryTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }

    [UIFact]
    public async Task BottomSheet_DoesNotLeak_WhenOpenedAndClosed()
    {
        if (TryGetTarget(out var bottomSheet))
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
        if (TryGetTarget(out var bottomSheet))
        {
            bottomSheet.IsOpen = true;

            await Task.Delay(1000);
        }
    }
    
    [UIFact]
    public async Task BottomSheet_DoesNotLeak_WithEventsAndCommands()
    {
        if (TryGetTarget(out var bottomSheet))
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
    
    [UIFact]
    public async Task BottomSheet_DoesNotLeak_WhenIgnoreSafeAreaIsToggled()
    {
        if (TryGetTarget(out var bottomSheet))
        {
            bottomSheet.IgnoreSafeArea = true;
            bottomSheet.IsOpen = true;
            await Task.Delay(1000);
            
            bottomSheet.IsOpen = false;
            await Task.Delay(1000);
        }
    }

    [UIFact]
    public async Task BottomSheet_DoesNotLeak_WhenIsCancelableIsToggled()
    {
        if (TryGetTarget(out var bottomSheet))
        {
            bottomSheet.IsCancelable = false;
            bottomSheet.IsOpen = true;
            await Task.Delay(1000);
            
            bottomSheet.IsOpen = false;
            await Task.Delay(1000);
        }
    }

    [UIFact]
    public async Task BottomSheet_DoesNotLeak_WhenHasHandleIsSet()
    {
        if (TryGetTarget(out var bottomSheet))
        {
            bottomSheet.HasHandle = true;
            bottomSheet.IsOpen = true;
            await Task.Delay(1000);
            
            bottomSheet.IsOpen = false;
            await Task.Delay(1000);
        }
    }

    [UIFact]
    public async Task BottomSheet_DoesNotLeak_WhenShowHeaderIsToggled()
    {
        if (TryGetTarget(out var bottomSheet))
        {
            bottomSheet.ShowHeader = false;
            bottomSheet.IsOpen = true;
            await Task.Delay(1000);

            bottomSheet.IsOpen = false;
            await Task.Delay(1000);
        }
    }

    [UIFact]
    public async Task BottomSheet_DoesNotLeak_WhenIsModalIsToggled()
    {
        if (TryGetTarget(out var bottomSheet))
        {
            bottomSheet.IsModal = true;
            bottomSheet.IsOpen = true;
            await Task.Delay(1000);

            bottomSheet.IsOpen = false;
            await Task.Delay(1000);
        }
    }

    [UIFact]
    public async Task BottomSheet_DoesNotLeak_WhenIsDraggableIsToggled()
    {
        if (TryGetTarget(out var bottomSheet))
        {
            bottomSheet.IsDraggable = false;
            bottomSheet.IsOpen = true;
            await Task.Delay(1000);

            bottomSheet.IsOpen = false;
            await Task.Delay(1000);
        }
    }

    [UIFact]
    public async Task BottomSheet_DoesNotLeak_WhenCornerRadiusIsChanged()
    {
        if (TryGetTarget(out var bottomSheet))
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
        if (TryGetTarget(out var bottomSheet))
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
        if (TryGetTarget(out var bottomSheet))
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
        if (TryGetTarget(out var bottomSheet))
        {
            bottomSheet.Padding = new Thickness(20, 20);
            bottomSheet.IsOpen = true;
            await Task.Delay(1000);

            bottomSheet.IsOpen = false;
            await Task.Delay(1000);
        }
    }

    [UIFact]
    public async Task BottomSheet_DoesNotLeak_WhenBottomSheetStyleIsSet()
    {
        if (TryGetTarget(out var bottomSheet))
        {
            bottomSheet.BottomSheetStyle = new BottomSheetStyle();
            bottomSheet.IsOpen = true;
            await Task.Delay(1000);

            bottomSheet.IsOpen = false;
            await Task.Delay(1000);
        }
    }

    [UIFact]
    public async Task BottomSheet_DoesNotLeak_WhenHeaderIsSet()
    {
        if (TryGetTarget(out var bottomSheet))
        {
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
        if (TryGetTarget(out var bottomSheet))
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
        if (TryGetTarget(out var bottomSheet))
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
        if (TryGetTarget(out var bottomSheet))
        {
            bottomSheet.CurrentState = BottomSheetState.Medium;
            bottomSheet.IsOpen = true;
            await Task.Delay(1000);

            bottomSheet.IsOpen = false;
            await Task.Delay(1000);
        }
    }
}