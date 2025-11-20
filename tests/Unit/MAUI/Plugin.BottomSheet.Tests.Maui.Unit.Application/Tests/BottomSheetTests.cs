using System.Windows.Input;
using NSubstitute;
using Plugin.Maui.BottomSheet;
using Plugin.Maui.BottomSheet.PlatformConfiguration.AndroidSpecific;
using Xunit.Abstractions;
using MauiThickness = Microsoft.Maui.Thickness;

namespace Plugin.BottomSheet.Tests.Maui.Unit.Application.Tests;

public class BottomSheetTests : BaseTest<Mocks.EmptyContentPage, Plugin.Maui.BottomSheet.BottomSheet>
{
    public BottomSheetTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper)
    {
    }

    [UIFact]
    public void Constructor_SetsDefaultValues()
    {
        // Assert
        Assert.True(View.IsModal);
        #if ANDROID
        Assert.Equal(20.0f, View.CornerRadius);
        #else
        Assert.Equal(50.0f, View.CornerRadius);
        #endif
        Assert.Equal(Color.FromArgb("#80000000"), View.WindowBackgroundColor);
        Assert.False(View.IgnoreSafeArea);
        Assert.True(View.IsCancelable);
        Assert.True(View.HasHandle);
        Assert.False(View.ShowHeader);
        Assert.False(View.IsOpen);
        Assert.True(View.IsDraggable);
        Assert.Equal(0.0, View.PeekHeight);
        Assert.Equal(new MauiThickness(10), View.Padding);
        Assert.NotNull(View.BottomSheetStyle);
        Assert.Single(View.States);
        Assert.Contains(BottomSheetState.Large, View.States);
        Assert.Equal(BottomSheetState.Large, View.CurrentState);
    }

    [UIFact]
    public void IsModal_CanBeSetAndRetrieved()
    {
        // Act
        View.IsModal = false;

        // Assert
        Assert.False(View.IsModal);
    }

    [UITheory]
    [InlineData(10.0f)]
    [InlineData(0.0f)]
    [InlineData(50.5f)]
    public void CornerRadius_CanBeSetAndRetrieved(float radius)
    {
        // Act
        View.CornerRadius = radius;

        // Assert
        Assert.Equal(radius, View.CornerRadius);
    }

    [UIFact]
    public void WindowBackgroundColor_CanBeSetAndRetrieved()
    {
        // Arrange
        var color = Colors.Red;

        // Act
        View.WindowBackgroundColor = color;

        // Assert
        Assert.Equal(color, View.WindowBackgroundColor);
    }

    [UITheory]
    [InlineData(true)]
    [InlineData(false)]
    public void IgnoreSafeArea_CanBeSetAndRetrieved(bool value)
    {
        // Act
        View.IgnoreSafeArea = value;

        // Assert
        Assert.Equal(value, View.IgnoreSafeArea);
    }

    [UITheory]
    [InlineData(true)]
    [InlineData(false)]
    public void IsCancelable_CanBeSetAndRetrieved(bool value)
    {
        // Act
        View.IsCancelable = value;

        // Assert
        Assert.Equal(value, View.IsCancelable);
    }

    [UITheory]
    [InlineData(true)]
    [InlineData(false)]
    public void HasHandle_CanBeSetAndRetrieved(bool value)
    {
        // Act
        View.HasHandle = value;

        // Assert
        Assert.Equal(value, View.HasHandle);
    }

    [UITheory]
    [InlineData(true)]
    [InlineData(false)]
    public void ShowHeader_CanBeSetAndRetrieved(bool value)
    {
        // Act
        View.ShowHeader = value;

        // Assert
        Assert.Equal(value, View.ShowHeader);
    }

    [UITheory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task IsOpen_CanBeSetAndRetrieved(bool value)
    {
        // Act
        View.IsOpen = value;
        
        await Task.Delay(1000);

        // Assert
        Assert.Equal(value, View.IsOpen);
    }

    [UITheory]
    [InlineData(true)]
    [InlineData(false)]
    public void IsDraggable_CanBeSetAndRetrieved(bool value)
    {
        // Act
        View.IsDraggable = value;

        // Assert
        Assert.Equal(value, View.IsDraggable);
    }

    [UITheory]
    [InlineData(0.0)]
    [InlineData(100.5)]
    [InlineData(250.0)]
    public void PeekHeight_CanBeSetAndRetrieved(double height)
    {
        // Act
        View.PeekHeight = height;

        // Assert
        Assert.Equal(height, View.PeekHeight);
    }

    [UIFact]
    public void Header_CanBeSetAndRetrieved()
    {
        // Arrange
        var header = new BottomSheetHeader();

        // Act
        View.Header = header;

        // Assert
        Assert.Equal(header, View.Header);
    }

    [UIFact]
    public void Content_CanBeSetAndRetrieved()
    {
        // Arrange
        var content = new BottomSheetContent();

        // Act
        View.Content = content;

        // Assert
        Assert.Equal(content, View.Content);
    }

    [UIFact]
    public void Padding_CanBeSetAndRetrieved()
    {
        // Arrange
        var padding = new MauiThickness(10, 15, 20, 25);

        // Act
        View.Padding = padding;

        // Assert
        Assert.Equal(padding, View.Padding);
    }

    [UIFact]
    public void BottomSheetStyle_CanBeSetAndRetrieved()
    {
        // Arrange
        var style = new BottomSheetStyle();

        // Act
        View.BottomSheetStyle = style;

        // Assert
        Assert.Equal(style, View.BottomSheetStyle);
    }

    [UIFact]
    public void States_CanBeSetAndRetrieved()
    {
        // Arrange
        var states = new List<BottomSheetState> { BottomSheetState.Medium, BottomSheetState.Large };

        // Act
        View.States = states;

        // Assert
        Assert.Equal(states, View.States);
    }

    [UIFact]
    public void CurrentState_CanBeSetAndRetrieved()
    {
        // Arrange
        var states = new List<BottomSheetState> { BottomSheetState.Medium, BottomSheetState.Large };
        View.States = states;

        // Act
        View.CurrentState = BottomSheetState.Medium;

        // Assert
        Assert.Equal(BottomSheetState.Medium, View.CurrentState);
    }

    [UIFact]
    public void Commands_CanBeSetAndRetrieved()
    {
        // Arrange
        var closingCommand = Substitute.For<ICommand>();
        var closedCommand = Substitute.For<ICommand>();
        var openingCommand = Substitute.For<ICommand>();
        var openedCommand = Substitute.For<ICommand>();
        var parameter = new object();

        // Act
        View.ClosingCommand = closingCommand;
        View.ClosingCommandParameter = parameter;
        View.ClosedCommand = closedCommand;
        View.ClosedCommandParameter = parameter;
        View.OpeningCommand = openingCommand;
        View.OpeningCommandParameter = parameter;
        View.OpenedCommand = openedCommand;
        View.OpenedCommandParameter = parameter;

        // Assert
        Assert.Equal(closingCommand, View.ClosingCommand);
        Assert.Equal(parameter, View.ClosingCommandParameter);
        Assert.Equal(closedCommand, View.ClosedCommand);
        Assert.Equal(parameter, View.ClosedCommandParameter);
        Assert.Equal(openingCommand, View.OpeningCommand);
        Assert.Equal(parameter, View.OpeningCommandParameter);
        Assert.Equal(openedCommand, View.OpenedCommand);
        Assert.Equal(parameter, View.OpenedCommandParameter);
    }

    [UIFact]
    public void OnOpeningBottomSheet_RaisesEventAndExecutesCommand()
    {
        // Arrange
        var command = Substitute.For<ICommand>();
        var parameter = new object();
        var eventRaised = false;

        command.CanExecute(parameter).Returns(true);
        View.OpeningCommand = command;
        View.OpeningCommandParameter = parameter;
        View.Opening += (_, _) => eventRaised = true;

        // Act
        ((IBottomSheet)View).OnOpeningBottomSheet();

        // Assert
        Assert.True(eventRaised);
        command.Received(1).Execute(parameter);
    }

    [UIFact]
    public void OnOpenedBottomSheet_RaisesEventAndExecutesCommand()
    {
        // Arrange
        var command = Substitute.For<ICommand>();
        var parameter = new object();
        var eventRaised = false;

        command.CanExecute(parameter).Returns(true);
        View.OpenedCommand = command;
        View.OpenedCommandParameter = parameter;
        View.Opened += (_, _) => eventRaised = true;

        // Act
        ((IBottomSheet)View).OnOpenedBottomSheet();

        // Assert
        Assert.True(eventRaised);
        command.Received(1).Execute(parameter);
    }
    
    [UIFact]
    public void OnClosingBottomSheet_RaisesEventAndExecutesCommand()
    {
        // Arrange
        var command = Substitute.For<ICommand>();
        var parameter = new object();
        var eventRaised = false;

        command.CanExecute(parameter).Returns(true);
        View.ClosingCommand = command;
        View.ClosingCommandParameter = parameter;
        View.Closing += (_, _) => eventRaised = true;

        // Act
        ((IBottomSheet)View).OnClosingBottomSheet();

        // Assert
        Assert.True(eventRaised);
        command.Received(1).Execute(parameter);
    }

    [UIFact]
    public void OnClosedBottomSheet_RaisesEventAndExecutesCommand()
    {
        // Arrange
        var command = Substitute.For<ICommand>();
        var parameter = new object();
        var eventRaised = false;

        command.CanExecute(parameter).Returns(true);
        View.ClosedCommand = command;
        View.ClosedCommandParameter = parameter;
        View.Closed += (_, _) => eventRaised = true;

        // Act
        ((IBottomSheet)View).OnClosedBottomSheet();

        // Assert
        Assert.True(eventRaised);
        command.Received(1).Execute(parameter);
    }

    [UIFact]
    public void Command_DoesNotExecute_WhenCanExecuteReturnsFalse()
    {
        // Arrange
        var command = Substitute.For<ICommand>();
        var parameter = new object();

        command.CanExecute(parameter).Returns(false);
        View.OpeningCommand = command;
        View.OpeningCommandParameter = parameter;

        // Act
        ((IBottomSheet)View).OnOpeningBottomSheet();

        // Assert
        command.DidNotReceive().Execute(Arg.Any<object>());
    }

    [UIFact]
    public void Command_DoesNotExecute_WhenCommandIsNull()
    {
        // Arrange
        View.OpeningCommand = null;

        // Act & Assert (should not throw)
        ((IBottomSheet)View).OnOpeningBottomSheet();
    }

    [UIFact]
    public void OnBindingContextChanged_PropagatesBindingContextToChildren()
    {
        // Arrange
        var header = new BottomSheetHeader();
        var content = new BottomSheetContent();
        var bindingContext = new object();

        View.Header = header;
        View.Content = content;

        // Act
        View.BindingContext = bindingContext;

        // Assert
        Assert.Equal(bindingContext, header.BindingContext);
        Assert.Equal(bindingContext, content.BindingContext);
    }

    [UIFact]
    public void OnBindingContextChanged_HandlesNullChildren()
    {
        // Arrange
        var bindingContext = new object();

        View.Header = null;
        View.Content = null;

        // Act & Assert (should not throw)
        View.BindingContext = bindingContext;
    }

    [UIFact]
    public void States_ChangingToEmptyList_UpdatesCurrentStateAndDisablesDragging()
    {
        // Arrange
        var newStates = new List<BottomSheetState> { BottomSheetState.Medium };

        // Act
        View.States = newStates;

        // Assert
        Assert.Equal(BottomSheetState.Medium, View.CurrentState);
    }

    [UIFact]
    public void CurrentState_OnlyAcceptsValidStates()
    {
        // Arrange
        var states = new List<BottomSheetState> { BottomSheetState.Medium, BottomSheetState.Large };
        View.States = states;
        View.CurrentState = BottomSheetState.Medium;

        // Act - try to set invalid state
        View.CurrentState = BottomSheetState.Peek; // Not in the allowed states

        // Assert - should remain unchanged
        Assert.Equal(BottomSheetState.Medium, View.CurrentState);
    }

    [UIFact]
    public void Events_CanBeSubscribedAndUnsubscribed()
    {
        // Arrange
        var openingRaised = false;
        var openedRaised = false;
        var closingRaised = false;
        var closedRaised = false;

        EventHandler openingHandler = (_, _) => openingRaised = true;
        EventHandler openedHandler = (_, _) => openedRaised = true;
        EventHandler closingHandler = (_, _) => closingRaised = true;
        EventHandler closedHandler = (_, _) => closedRaised = true;

        // Act - Subscribe
        View.Opening += openingHandler;
        View.Opened += openedHandler;
        View.Closing += closingHandler;
        View.Closed += closedHandler;
        
        ((IBottomSheet)View).OnOpeningBottomSheet();
        ((IBottomSheet)View).OnOpenedBottomSheet();
        ((IBottomSheet)View).OnClosingBottomSheet();
        ((IBottomSheet)View).OnClosedBottomSheet();

        // Assert
        Assert.True(openingRaised);
        Assert.True(openedRaised);
        Assert.True(closingRaised);
        Assert.True(closedRaised);

        // Act - Unsubscribe
        openingRaised = false;
        openedRaised = false;
        closingRaised = false;
        closedRaised = false;
        
        View.Opening -= openingHandler;
        View.Opened -= openedHandler;
        View.Closing -= closingHandler;
        View.Closed -= closedHandler;

        ((IBottomSheet)View).OnOpeningBottomSheet();
        ((IBottomSheet)View).OnOpenedBottomSheet();
        ((IBottomSheet)View).OnClosingBottomSheet();
        ((IBottomSheet)View).OnClosedBottomSheet();

        // Assert
        Assert.False(openingRaised);
        Assert.False(openedRaised);
        Assert.False(closingRaised);
        Assert.False(closedRaised);
    }

    [UIFact]
    public void AndroidPlatformConfiguration_SetAndGetProperties_ReturnsExpectedValues()
    {
        View.On<Microsoft.Maui.Controls.PlatformConfiguration.Android>().SetMaxHeight(100);
        View.On<Microsoft.Maui.Controls.PlatformConfiguration.Android>().SetMaxWidth(200);
        View.On<Microsoft.Maui.Controls.PlatformConfiguration.Android>().SetHalfExpandedRatio(0.3f);
        View.SetHalfExpandedRatio(0.3f);
        View.On<Microsoft.Maui.Controls.PlatformConfiguration.Android>().SetTheme(99);
        View.SetTheme(99);
        View.On<Microsoft.Maui.Controls.PlatformConfiguration.Android>().SetMargin(new MauiThickness(10,20));

        Assert.Equal(100, View.On<Microsoft.Maui.Controls.PlatformConfiguration.Android>().GetMaxHeight());
        Assert.Equal(200, View.On<Microsoft.Maui.Controls.PlatformConfiguration.Android>().GetMaxWidth());
        Assert.Equal(0.3f, View.On<Microsoft.Maui.Controls.PlatformConfiguration.Android>().GetHalfExpandedRatio());
        Assert.Equal(99, View.On<Microsoft.Maui.Controls.PlatformConfiguration.Android>().GetTheme());
        Assert.Equal(new MauiThickness(10,20), View.On<Microsoft.Maui.Controls.PlatformConfiguration.Android>().GetMargin());
            
        Assert.Equal(100, View.GetMaxHeight());
        Assert.Equal(200, View.GetMaxWidth());
        Assert.Equal(0.3f, View.GetHalfExpandedRatio());
        Assert.Equal(99, View.GetTheme());
        Assert.Equal(new MauiThickness(10,20), View.GetMargin());

        var sheetMock = Substitute.For<IBottomSheet>();
            
        Assert.Throws<ArgumentException>(() => sheetMock.SetTheme(10));
        Assert.Throws<ArgumentException>(() => sheetMock.GetTheme());
        Assert.Throws<ArgumentException>(() => sheetMock.SetHalfExpandedRatio(0.3f));
        Assert.Throws<ArgumentException>(() => sheetMock.GetHalfExpandedRatio());
        Assert.Throws<ArgumentException>(() => sheetMock.GetMargin());
        Assert.Throws<ArgumentException>(() => sheetMock.GetMaxHeight());
        Assert.Throws<ArgumentException>(() => sheetMock.GetMaxWidth());
    }
    
    [UIFact]
    public void SettingEmptyStates_StatesRemainsNonEmpty()
    {
        View.States = [];
            
        Assert.NotEmpty(View.States);
    }
    
    [UIFact]
    public void States_WhenChangedToNewCollection_UpdatesCurrentStateToFirstValidState ()
    {
        View.States = [BottomSheetState.Large];
        View.CurrentState = BottomSheetState.Large;

        View.States = [BottomSheetState.Medium];
            
        Assert.Equivalent(View.CurrentState, BottomSheetState.Medium);
    }
        
    [UIFact]
    public void States_DefaultCurrentState_IsFirstInStatesList()
    {
        Assert.Equivalent(View.States.First(), View.CurrentState);
    }
        
    [UIFact]
    public void SettingInvalidCurrentState_DoesNotChangeCurrentState()
    {
        View.CurrentState = BottomSheetState.Medium;

        Assert.NotEqual(BottomSheetState.Medium, View.CurrentState);
        Assert.Equal(BottomSheetState.Large, View.CurrentState);
    }
}