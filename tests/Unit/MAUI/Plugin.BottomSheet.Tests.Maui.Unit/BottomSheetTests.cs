using System.Windows.Input;
using NSubstitute;
using Plugin.Maui.BottomSheet;
using Plugin.Maui.BottomSheet.PlatformConfiguration.AndroidSpecific;
using MauiBottomSheet = Plugin.Maui.BottomSheet.BottomSheet;
using MauiThickness = Microsoft.Maui.Thickness;

namespace Plugin.BottomSheet.Tests.Maui.Unit;

using Microsoft.Maui.Controls.PlatformConfiguration;

public class BottomSheetTests
{
    [Fact]
    public void Constructor_SetsDefaultValues()
    {
        // Arrange & Act
        MauiBottomSheet bottomSheet = new MauiBottomSheet();

        // Assert
        Assert.True(bottomSheet.IsModal);
        Assert.Equal(20.0f, bottomSheet.CornerRadius);
        Assert.Equal(Color.FromArgb("#80000000"), bottomSheet.WindowBackgroundColor);
        Assert.True(bottomSheet.IsCancelable);
        Assert.True(bottomSheet.HasHandle);
        Assert.False(bottomSheet.ShowHeader);
        Assert.False(bottomSheet.IsOpen);
        Assert.True(bottomSheet.IsDraggable);
        Assert.Equal(0.0, bottomSheet.PeekHeight);
        Assert.Equal(new MauiThickness(10), bottomSheet.Padding);
        Assert.NotNull(bottomSheet.BottomSheetStyle);
        Assert.Single(bottomSheet.States);
        Assert.Contains(BottomSheetState.Large, bottomSheet.States);
        Assert.Equal(BottomSheetState.Large, bottomSheet.CurrentState);
    }

    [Fact]
    public void IsModal_CanBeSetAndRetrieved()
    {
        // Arrange
        MauiBottomSheet bottomSheet = new MauiBottomSheet
        {
            // Act
            IsModal = false,
        };

        // Assert
        Assert.False(bottomSheet.IsModal);
    }

    [Theory]
    [InlineData(10.0f)]
    [InlineData(0.0f)]
    [InlineData(50.5f)]
    public void CornerRadius_CanBeSetAndRetrieved(float radius)
    {
        // Arrange
        MauiBottomSheet bottomSheet = new MauiBottomSheet
        {
            // Act
            CornerRadius = radius,
        };

        // Assert
        Assert.Equal(radius, bottomSheet.CornerRadius);
    }

    [Fact]
    public void WindowBackgroundColor_CanBeSetAndRetrieved()
    {
        // Arrange
        MauiBottomSheet bottomSheet = new MauiBottomSheet();
        Color? color = Colors.Red;

        // Act
        bottomSheet.WindowBackgroundColor = color;

        // Assert
        Assert.Equal(color, bottomSheet.WindowBackgroundColor);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void IsCancelable_CanBeSetAndRetrieved(bool value)
    {
        // Arrange
        MauiBottomSheet bottomSheet = new MauiBottomSheet
        {
            // Act
            IsCancelable = value,
        };

        // Assert
        Assert.Equal(value, bottomSheet.IsCancelable);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void HasHandle_CanBeSetAndRetrieved(bool value)
    {
        // Arrange
        MauiBottomSheet bottomSheet = new MauiBottomSheet
        {
            // Act
            HasHandle = value,
        };

        // Assert
        Assert.Equal(value, bottomSheet.HasHandle);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void ShowHeader_CanBeSetAndRetrieved(bool value)
    {
        // Arrange
        MauiBottomSheet bottomSheet = new MauiBottomSheet
        {
            // Act
            ShowHeader = value,
        };

        // Assert
        Assert.Equal(value, bottomSheet.ShowHeader);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void IsOpen_CanBeSetAndRetrieved(bool value)
    {
        // Arrange
        MauiBottomSheet bottomSheet = new MauiBottomSheet
        {
            // Act
            IsOpen = value,
        };

        // Assert
        Assert.Equal(value, bottomSheet.IsOpen);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void IsDraggable_CanBeSetAndRetrieved(bool value)
    {
        // Arrange
        MauiBottomSheet bottomSheet = new MauiBottomSheet
        {
            // Act
            IsDraggable = value,
        };

        // Assert
        Assert.Equal(value, bottomSheet.IsDraggable);
    }

    [Theory]
    [InlineData(0.0)]
    [InlineData(100.5)]
    [InlineData(250.0)]
    public void PeekHeight_CanBeSetAndRetrieved(double height)
    {
        // Arrange
        MauiBottomSheet bottomSheet = new MauiBottomSheet
        {
            // Act
            PeekHeight = height,
        };

        // Assert
        Assert.Equal(height, bottomSheet.PeekHeight);
    }

    [Fact]
    public void Header_CanBeSetAndRetrieved()
    {
        // Arrange
        MauiBottomSheet bottomSheet = new MauiBottomSheet();
        BottomSheetHeader header = new BottomSheetHeader();

        // Act
        bottomSheet.Header = header;

        // Assert
        Assert.Equal(header, bottomSheet.Header);
    }

    [Fact]
    public void Content_CanBeSetAndRetrieved()
    {
        // Arrange
        MauiBottomSheet bottomSheet = new MauiBottomSheet();
        BottomSheetContent content = new BottomSheetContent();

        // Act
        bottomSheet.Content = content;

        // Assert
        Assert.Equal(content, bottomSheet.Content);
    }

    [Fact]
    public void Padding_CanBeSetAndRetrieved()
    {
        // Arrange
        MauiBottomSheet bottomSheet = new MauiBottomSheet();
        MauiThickness padding = new MauiThickness(10, 15, 20, 25);

        // Act
        bottomSheet.Padding = padding;

        // Assert
        Assert.Equal(padding, bottomSheet.Padding);
    }

    [Fact]
    public void BottomSheetStyle_CanBeSetAndRetrieved()
    {
        // Arrange
        MauiBottomSheet bottomSheet = new MauiBottomSheet();
        BottomSheetStyle style = new BottomSheetStyle();

        // Act
        bottomSheet.BottomSheetStyle = style;

        // Assert
        Assert.Equal(style, bottomSheet.BottomSheetStyle);
    }

    [Fact]
    public void States_CanBeSetAndRetrieved()
    {
        // Arrange
        MauiBottomSheet bottomSheet = new MauiBottomSheet();
        List<BottomSheetState> states = new List<BottomSheetState> { BottomSheetState.Medium, BottomSheetState.Large };

        // Act
        bottomSheet.States = states;

        // Assert
        Assert.Equal(states, bottomSheet.States);
    }

    [Fact]
    public void CurrentState_CanBeSetAndRetrieved()
    {
        // Arrange
        MauiBottomSheet bottomSheet = new MauiBottomSheet();
        List<BottomSheetState> states = new List<BottomSheetState> { BottomSheetState.Medium, BottomSheetState.Large };
        bottomSheet.States = states;

        // Act
        bottomSheet.CurrentState = BottomSheetState.Medium;

        // Assert
        Assert.Equal(BottomSheetState.Medium, bottomSheet.CurrentState);
    }

    [Fact]
    public void Commands_CanBeSetAndRetrieved()
    {
        // Arrange
        MauiBottomSheet bottomSheet = new MauiBottomSheet();
        ICommand? closingCommand = Substitute.For<ICommand>();
        ICommand? closedCommand = Substitute.For<ICommand>();
        ICommand? openingCommand = Substitute.For<ICommand>();
        ICommand? openedCommand = Substitute.For<ICommand>();
        object parameter = new object();

        // Act
        bottomSheet.ClosingCommand = closingCommand;
        bottomSheet.ClosingCommandParameter = parameter;
        bottomSheet.ClosedCommand = closedCommand;
        bottomSheet.ClosedCommandParameter = parameter;
        bottomSheet.OpeningCommand = openingCommand;
        bottomSheet.OpeningCommandParameter = parameter;
        bottomSheet.OpenedCommand = openedCommand;
        bottomSheet.OpenedCommandParameter = parameter;

        // Assert
        Assert.Equal(closingCommand, bottomSheet.ClosingCommand);
        Assert.Equal(parameter, bottomSheet.ClosingCommandParameter);
        Assert.Equal(closedCommand, bottomSheet.ClosedCommand);
        Assert.Equal(parameter, bottomSheet.ClosedCommandParameter);
        Assert.Equal(openingCommand, bottomSheet.OpeningCommand);
        Assert.Equal(parameter, bottomSheet.OpeningCommandParameter);
        Assert.Equal(openedCommand, bottomSheet.OpenedCommand);
        Assert.Equal(parameter, bottomSheet.OpenedCommandParameter);
    }

    [Fact]
    public void OnOpeningBottomSheet_RaisesEventAndExecutesCommand()
    {
        // Arrange
        MauiBottomSheet bottomSheet = new MauiBottomSheet();
        ICommand? command = Substitute.For<ICommand>();
        object parameter = new object();
        bool eventRaised = false;

        command.CanExecute(parameter).Returns(true);
        bottomSheet.OpeningCommand = command;
        bottomSheet.OpeningCommandParameter = parameter;
        bottomSheet.Opening += (_, _) => eventRaised = true;

        // Act
        ((IBottomSheet)bottomSheet).OnOpeningBottomSheet();

        // Assert
        Assert.True(eventRaised);
        command.Received().Execute(parameter);
    }

    [Fact]
    public void OnOpenedBottomSheet_RaisesEventAndExecutesCommand()
    {
        // Arrange
        MauiBottomSheet bottomSheet = new MauiBottomSheet();
        ICommand? command = Substitute.For<ICommand>();
        object parameter = new object();
        bool eventRaised = false;

        command.CanExecute(parameter).Returns(true);
        bottomSheet.OpenedCommand = command;
        bottomSheet.OpenedCommandParameter = parameter;
        bottomSheet.Opened += (_, _) => eventRaised = true;

        // Act
        ((IBottomSheet)bottomSheet).OnOpenedBottomSheet();

        // Assert
        Assert.True(eventRaised);
        command.Received().Execute(parameter);
    }

    [Fact]
    public void OnClosingBottomSheet_RaisesEventAndExecutesCommand()
    {
        // Arrange
        MauiBottomSheet bottomSheet = new MauiBottomSheet();
        ICommand? command = Substitute.For<ICommand>();
        object parameter = new object();
        bool eventRaised = false;

        command.CanExecute(parameter).Returns(true);
        bottomSheet.ClosingCommand = command;
        bottomSheet.ClosingCommandParameter = parameter;
        bottomSheet.Closing += (_, _) => eventRaised = true;

        // Act
        ((IBottomSheet)bottomSheet).OnClosingBottomSheet();

        // Assert
        Assert.True(eventRaised);
        command.Received().Execute(parameter);
    }

    [Fact]
    public void OnClosedBottomSheet_RaisesEventAndExecutesCommand()
    {
        // Arrange
        MauiBottomSheet bottomSheet = new MauiBottomSheet();
        ICommand? command = Substitute.For<ICommand>();
        object parameter = new object();
        bool eventRaised = false;

        command.CanExecute(parameter).Returns(true);
        bottomSheet.ClosedCommand = command;
        bottomSheet.ClosedCommandParameter = parameter;
        bottomSheet.Closed += (_, _) => eventRaised = true;

        // Act
        ((IBottomSheet)bottomSheet).OnClosedBottomSheet();

        // Assert
        Assert.True(eventRaised);
        command.Received().Execute(parameter);
    }

    [Fact]
    public void Command_DoesNotExecute_WhenCanExecuteReturnsFalse()
    {
        // Arrange
        MauiBottomSheet bottomSheet = new MauiBottomSheet();
        ICommand? command = Substitute.For<ICommand>();
        object parameter = new object();

        command.CanExecute(parameter).Returns(false);
        bottomSheet.OpeningCommand = command;
        bottomSheet.OpeningCommandParameter = parameter;

        // Act
        ((IBottomSheet)bottomSheet).OnOpeningBottomSheet();

        // Assert
        command.DidNotReceive().Execute(Arg.Any<object>());
    }

    [Fact]
    public void Command_DoesNotExecute_WhenCommandIsNull()
    {
        // Arrange
        MauiBottomSheet bottomSheet = new MauiBottomSheet
        {
            OpeningCommand = null,
        };

        // Act & Assert (should not throw)
        ((IBottomSheet)bottomSheet).OnOpeningBottomSheet();
    }

    [Fact]
    public void OnBindingContextChanged_PropagatesBindingContextToChildren()
    {
        // Arrange
        MauiBottomSheet bottomSheet = new MauiBottomSheet();
        BottomSheetHeader header = new BottomSheetHeader();
        BottomSheetContent content = new BottomSheetContent();
        object bindingContext = new object();

        bottomSheet.Header = header;
        bottomSheet.Content = content;

        // Act
        bottomSheet.BindingContext = bindingContext;

        // Assert
        Assert.Equal(bindingContext, header.BindingContext);
        Assert.Equal(bindingContext, content.BindingContext);
    }

    [Fact]
    public void OnBindingContextChanged_HandlesNullChildren()
    {
        // Arrange
        MauiBottomSheet bottomSheet = new MauiBottomSheet();
        object bindingContext = new object();

        bottomSheet.Header = null;
        bottomSheet.Content = null;

        // Act & Assert (should not throw)
        bottomSheet.BindingContext = bindingContext;
    }

    [Fact]
    public void States_ChangingToEmptyList_UpdatesCurrentStateAndDisablesDragging()
    {
        // Arrange
        MauiBottomSheet bottomSheet = new MauiBottomSheet();
        List<BottomSheetState> newStates = new List<BottomSheetState> { BottomSheetState.Medium };

        // Act
        bottomSheet.States = newStates;

        // Assert
        Assert.Equal(BottomSheetState.Medium, bottomSheet.CurrentState);
    }

    [Fact]
    public void CurrentState_OnlyAcceptsValidStates()
    {
        // Arrange
        MauiBottomSheet bottomSheet = new MauiBottomSheet();
        List<BottomSheetState> states = new List<BottomSheetState> { BottomSheetState.Medium, BottomSheetState.Large };
        bottomSheet.States = states;
        bottomSheet.CurrentState = BottomSheetState.Medium;

        // Act - try to set invalid state
        bottomSheet.CurrentState = BottomSheetState.Peek; // Not in the allowed states

        // Assert - should remain unchanged
        Assert.Equal(BottomSheetState.Medium, bottomSheet.CurrentState);
    }

    [Fact]
    public void Events_CanBeSubscribedAndUnsubscribed()
    {
        // Arrange
        MauiBottomSheet bottomSheet = new MauiBottomSheet();
        bool openingRaised = false;
        bool openedRaised = false;
        bool closingRaised = false;
        bool closedRaised = false;

        EventHandler openingHandler = (_, _) => openingRaised = true;
        EventHandler openedHandler = (_, _) => openedRaised = true;
        EventHandler closingHandler = (_, _) => closingRaised = true;
        EventHandler closedHandler = (_, _) => closedRaised = true;

        // Act - Subscribe
        bottomSheet.Opening += openingHandler;
        bottomSheet.Opened += openedHandler;
        bottomSheet.Closing += closingHandler;
        bottomSheet.Closed += closedHandler;
        
        ((IBottomSheet)bottomSheet).OnOpeningBottomSheet();
        ((IBottomSheet)bottomSheet).OnOpenedBottomSheet();
        ((IBottomSheet)bottomSheet).OnClosingBottomSheet();
        ((IBottomSheet)bottomSheet).OnClosedBottomSheet();

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
        
        bottomSheet.Opening -= openingHandler;
        bottomSheet.Opened -= openedHandler;
        bottomSheet.Closing -= closingHandler;
        bottomSheet.Closed -= closedHandler;

        ((IBottomSheet)bottomSheet).OnOpeningBottomSheet();
        ((IBottomSheet)bottomSheet).OnOpenedBottomSheet();
        ((IBottomSheet)bottomSheet).OnClosingBottomSheet();
        ((IBottomSheet)bottomSheet).OnClosedBottomSheet();

        // Assert
        Assert.False(openingRaised);
        Assert.False(openedRaised);
        Assert.False(closingRaised);
        Assert.False(closedRaised);
    }

    [Fact]
    public void AndroidPlatformConfiguration_SetAndGetProperties_ReturnsExpectedValues()
    {
        MauiBottomSheet sheet = new MauiBottomSheet();

        sheet.On<Android>().SetMaxHeight(100);
        sheet.On<Android>().SetMaxWidth(200);
        sheet.On<Android>().SetHalfExpandedRatio(0.3f);
        sheet.SetHalfExpandedRatio(0.3f);
        sheet.On<Android>().SetTheme(99);
        sheet.SetTheme(99);
        sheet.On<Android>().SetMargin(new MauiThickness(10,20));

        Assert.Equal(100, sheet.On<Android>().GetMaxHeight());
        Assert.Equal(200, sheet.On<Android>().GetMaxWidth());
        Assert.Equal(0.3f, sheet.On<Android>().GetHalfExpandedRatio());
        Assert.Equal(99, sheet.On<Android>().GetTheme());
        Assert.Equal(new MauiThickness(10,20), sheet.On<Android>().GetMargin());
            
        Assert.Equal(100, sheet.GetMaxHeight());
        Assert.Equal(200, sheet.GetMaxWidth());
        Assert.Equal(0.3f, sheet.GetHalfExpandedRatio());
        Assert.Equal(99, sheet.GetTheme());
        Assert.Equal(new MauiThickness(10,20), sheet.GetMargin());

        IBottomSheet? sheetMock = Substitute.For<IBottomSheet>();
            
        Assert.Throws<ArgumentException>(() => sheetMock.SetTheme(10));
        Assert.Throws<ArgumentException>(() => sheetMock.GetTheme());
        Assert.Throws<ArgumentException>(() => sheetMock.SetHalfExpandedRatio(0.3f));
        Assert.Throws<ArgumentException>(() => sheetMock.GetHalfExpandedRatio());
        Assert.Throws<ArgumentException>(() => sheetMock.GetMargin());
        Assert.Throws<ArgumentException>(() => sheetMock.GetMaxHeight());
        Assert.Throws<ArgumentException>(() => sheetMock.GetMaxWidth());
    }
    
    [Fact]
    public void SettingEmptyStates_StatesRemainsNonEmpty()
    {
        MauiBottomSheet sheet = new MauiBottomSheet
        {
            States = [],
        };
            
        Assert.NotEmpty(sheet.States);
    }
    
    [Fact]
    public void States_WhenChangedToNewCollection_UpdatesCurrentStateToFirstValidState
        ()
    {
        MauiBottomSheet sheet = new MauiBottomSheet()
        {
            States = [BottomSheetState.Large],
            CurrentState = BottomSheetState.Large,
        };

        sheet.States = [BottomSheetState.Medium];
            
        Assert.Equivalent(sheet.CurrentState, BottomSheetState.Medium);
    }
        
    [Fact]
    public void States_DefaultCurrentState_IsFirstInStatesList()
    {
        MauiBottomSheet sheet = new MauiBottomSheet();
            
        Assert.Equivalent(sheet.States.First(), sheet.CurrentState);
    }
        
    [Fact]
    public void SettingInvalidCurrentState_DoesNotChangeCurrentState()
    {
        MauiBottomSheet sheet = new MauiBottomSheet
        {
            CurrentState = BottomSheetState.Medium,
        };

        Assert.NotEqual(BottomSheetState.Medium, sheet.CurrentState);
        Assert.Equal(BottomSheetState.Large, sheet.CurrentState);
    }
}