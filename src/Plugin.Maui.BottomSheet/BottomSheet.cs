using Microsoft.Maui.Controls.Shapes;
using Plugin.BottomSheet;
using System.ComponentModel;
using System.Windows.Input;
using MauiThickness = Microsoft.Maui.Thickness;

namespace Plugin.Maui.BottomSheet;

/// <summary>
/// A UI component that provides a collapsible and expandable container, typically used to display additional content at the bottom of the screen.
/// </summary>
[ContentProperty(nameof(Content))]
public partial class BottomSheet : View, IBottomSheet, IElementConfiguration<BottomSheet>
{
    /// <summary>
    /// Represents a bindable property that determines whether the bottom sheet is displayed in modal mode.
    /// </summary>
    public static readonly BindableProperty IsModalProperty =
        BindableProperty.Create(
            nameof(IsModal),
            typeof(bool),
            typeof(BottomSheet),
            defaultValue: true);

    /// <summary>
    /// Bindable property that specifies the corner radius of an element.
    /// </summary>
    public static readonly BindableProperty CornerRadiusProperty =
        BindableProperty.Create(
            nameof(CornerRadius),
            typeof(float),
            typeof(BottomSheet),
            #if ANDROID
            defaultValue: 20.0f);
            #else
            defaultValue: 50.0f);
            #endif

    /// <summary>
    /// Bindable property for the background color of a window.
    /// </summary>
    public static readonly BindableProperty WindowBackgroundColorProperty =
        BindableProperty.Create(
            nameof(WindowBackgroundColor),
            typeof(Color),
            typeof(BottomSheet),
            defaultValueCreator: _ => Color.FromArgb("#80000000"));

    /// <summary>
    /// Bindable property that represents the states of an object or control.
    /// </summary>
    public static readonly BindableProperty StatesProperty =
        BindableProperty.Create(
            nameof(States),
            typeof(List<BottomSheetState>),
            typeof(BottomSheet),
            propertyChanged: OnStatesPropertyChanged,
            defaultBindingMode: BindingMode.TwoWay,
            defaultValueCreator: _ => new List<BottomSheetState>()
            {
                BottomSheetState.Large,
            },
            validateValue: (bindable, value) =>
            {
                bool result = true;
                List<BottomSheetState>? states = (List<BottomSheetState>)value;

                if (states.Count == 0)
                {
                    result = false;
                }

                if (bindable is BottomSheet bottomSheet
                    && states.IsStateAllowed(bottomSheet.CurrentState) == false)
                {
                    System.Diagnostics.Trace.TraceError("The current state is not allowed in the states collection.");
                }

                return result;
            });

    /// <summary>
    /// Bindable property that represents the current state of the object.
    /// </summary>
    public static readonly BindableProperty CurrentStateProperty =
        BindableProperty.Create(
            nameof(CurrentState),
            typeof(BottomSheetState),
            typeof(BottomSheet),
            propertyChanged: CurrentStatePropertyChanged,
            defaultBindingMode: BindingMode.TwoWay,
            defaultValueCreator: bindable =>
            {
                BottomSheet? bottomSheet = (BottomSheet)bindable;

                return bottomSheet.States.Order().First();
            },
            validateValue: (bindable, value) =>
            {
                BottomSheet? bottomSheet = (BottomSheet)bindable;
                BottomSheetState state = (BottomSheetState)value;

                return bottomSheet.States.IsStateAllowed(state);
            });

    /// <summary>
    /// Bindable property indicating whether the user can cancel a bottom sheet.
    /// </summary>
    public static readonly BindableProperty IsCancelableProperty =
        BindableProperty.Create(
            nameof(IsCancelable),
            typeof(bool),
            typeof(BottomSheet),
            defaultValue: true,
            defaultBindingMode: BindingMode.TwoWay);

    /// <summary>
    /// Bindable property indicating the presence of a handle component.
    /// </summary>
    public static readonly BindableProperty HasHandleProperty =
        BindableProperty.Create(
            nameof(HasHandle),
            typeof(bool),
            typeof(BottomSheet),
            defaultValue: true,
            defaultBindingMode: BindingMode.TwoWay,
            propertyChanged: OnHasHandlePropertyChanged);

    /// <summary>
    /// Bindable property that indicates whether the header is displayed.
    /// </summary>
    public static readonly BindableProperty ShowHeaderProperty =
        BindableProperty.Create(
            nameof(ShowHeader),
            typeof(bool),
            typeof(BottomSheet),
            defaultBindingMode: BindingMode.TwoWay,
            propertyChanged: OnShowHeaderPropertyChanged);

    /// <summary>
    /// Bindable property that indicates whether the element is open or closed.
    /// </summary>
    public static readonly BindableProperty IsOpenProperty =
        BindableProperty.Create(
            nameof(IsOpen),
            typeof(bool),
            typeof(BottomSheet),
            defaultBindingMode: BindingMode.TwoWay);

    /// <summary>
    /// Bindable property that indicates whether the user can drag the element.
    /// </summary>
    public static readonly BindableProperty IsDraggableProperty =
        BindableProperty.Create(
            nameof(IsDraggable),
            typeof(bool),
            typeof(BottomSheet),
            defaultValue: true,
            defaultBindingMode: BindingMode.TwoWay);

    /// <summary>
    /// Bindable property for defining the peek height of a component.
    /// </summary>
    public static readonly BindableProperty PeekHeightProperty =
        BindableProperty.Create(
            nameof(PeekHeight),
            typeof(double),
            typeof(BottomSheet),
            defaultValue: 0.00);

    /// <summary>
    /// Bindable property for defining the header content.
    /// </summary>
    public static readonly BindableProperty HeaderProperty =
        BindableProperty.Create(
            nameof(Header),
            typeof(BottomSheetHeader),
            typeof(BottomSheet),
            propertyChanged: OnHeaderChanged);

    /// <summary>
    /// Bindable property that represents the content of a UI element or container.
    /// </summary>
    public static readonly BindableProperty ContentProperty =
        BindableProperty.Create(
            nameof(Content),
            typeof(BottomSheetContent),
            typeof(BottomSheet),
            propertyChanged: OnContentChanged);

    /// <summary>
    /// Bindable property that specifies the padding values for an element.
    /// </summary>
    public static readonly BindableProperty PaddingProperty =
        BindableProperty.Create(
            nameof(Padding),
            typeof(MauiThickness),
            typeof(BottomSheet),
            defaultValue: new MauiThickness(10),
            propertyChanged: OnPaddingPropertyChanged);

    /// <summary>
    /// Bindable property for the command to execute during the closing operation.
    /// </summary>
    public static readonly BindableProperty ClosingCommandProperty =
        BindableProperty.Create(
            nameof(ClosingCommand),
            typeof(ICommand),
            typeof(BottomSheet));

    /// <summary>
    /// Bindable property for the parameter passed to the closing command.
    /// </summary>
    public static readonly BindableProperty ClosingCommandParameterProperty =
        BindableProperty.Create(
            nameof(ClosingCommandParameter),
            typeof(object),
            typeof(BottomSheet));

    /// <summary>
    /// Bindable property that represents the command to be executed when a close action is triggered.
    /// </summary>
    public static readonly BindableProperty ClosedCommandProperty =
        BindableProperty.Create(
            nameof(ClosedCommand),
            typeof(ICommand),
            typeof(BottomSheet));

    /// <summary>
    /// Bindable property for the parameter passed to the command executed when the closed event is triggered.
    /// </summary>
    public static readonly BindableProperty ClosedCommandParameterProperty =
        BindableProperty.Create(
            nameof(ClosedCommandParameter),
            typeof(object),
            typeof(BottomSheet));

    /// <summary>
    /// Bindable property for the command executed during the opening action.
    /// </summary>
    public static readonly BindableProperty OpeningCommandProperty =
        BindableProperty.Create(
            nameof(OpeningCommand),
            typeof(ICommand),
            typeof(BottomSheet));

    /// <summary>
    /// Bindable property for the parameter passed to the command executed when opening occurs.
    /// </summary>
    public static readonly BindableProperty OpeningCommandParameterProperty =
        BindableProperty.Create(
            nameof(OpeningCommandParameter),
            typeof(object),
            typeof(BottomSheet));

    /// <summary>
    /// Bindable property that represents the command to execute when an item is opened.
    /// </summary>
    public static readonly BindableProperty OpenedCommandProperty =
        BindableProperty.Create(
            nameof(OpenedCommand),
            typeof(ICommand),
            typeof(BottomSheet));

    /// <summary>
    /// Bindable property for the parameter passed to the command when an item is opened.
    /// </summary>
    public static readonly BindableProperty OpenedCommandParameterProperty =
        BindableProperty.Create(
            nameof(OpenedCommandParameter),
            typeof(object),
            typeof(BottomSheet));

    /// <summary>
    /// Bindable property for customizing the style of the bottom sheet.
    /// </summary>
    public static readonly BindableProperty BottomSheetStyleProperty =
        BindableProperty.Create(
            nameof(BottomSheetStyle),
            typeof(BottomSheetStyle),
            typeof(BottomSheet),
            propertyChanged: OnBottomSheetStylePropertyChanged,
            defaultValue: new BottomSheetStyle());

    /// <summary>
    /// Bindable property that indicates whether the element is open or closed.
    /// </summary>
    public static readonly BindableProperty SizeModeProperty =
        BindableProperty.Create(
            nameof(SizeMode),
            typeof(BottomSheetSizeMode),
            typeof(BottomSheet),
            defaultBindingMode: BindingMode.TwoWay);

    /// <summary>
    /// Gets the constant row index used to position the handle element in the BottomSheet layout.
    /// </summary>
    internal const int HandleRow = 0;

    /// <summary>
    /// Gets the row identifier used to position and manage the header within the container view of the BottomSheet.
    /// </summary>
    internal const int HeaderRow = 1;

    /// <summary>
    /// Gets the row index within the layout grid where the bottom sheet's main content is displayed.
    /// </summary>
    internal const int ContentRow = 2;

    private readonly WeakEventManager _eventManager = new();

    private readonly Lazy<PlatformConfigurationRegistry<BottomSheet>> _platformConfigurationRegistry;

    /// <summary>
    /// Initializes a new instance of the <see cref="BottomSheet"/> class.
    /// Represents a component providing bottom sheet functionality in the user interface.
    /// </summary>
    /// <remarks>
    /// Typically used to display additional content or options in a sliding panel
    /// from the bottom of the screen.
    /// </remarks>
    public BottomSheet()
    {
        _platformConfigurationRegistry = new Lazy<PlatformConfigurationRegistry<BottomSheet>>(() => new PlatformConfigurationRegistry<BottomSheet>(this));
        Loaded += OnLoaded;

        ContainerView.Padding = Padding;
    }

    /// <summary>
    /// Indicates whether the object or process is in the closing state.
    /// </summary>
    public event EventHandler Closing
    {
        add => _eventManager.AddEventHandler(value);
        remove => _eventManager.RemoveEventHandler(value);
    }

    /// <summary>
    /// Indicates whether the current state or context is closed.
    /// </summary>
    public event EventHandler Closed
    {
        add => _eventManager.AddEventHandler(value);
        remove => _eventManager.RemoveEventHandler(value);
    }

    /// <summary>
    /// Represents the current state or status of opening, which could signify
    /// availability, accessibility, or a defined condition within a process or system.
    /// </summary>
    public event EventHandler Opening
    {
        add => _eventManager.AddEventHandler(value);
        remove => _eventManager.RemoveEventHandler(value);
    }

    /// <summary>
    /// Represents the state indicating whether an item, such as a file or window, is currently open.
    /// </summary>
    public event EventHandler Opened
    {
        add => _eventManager.AddEventHandler(value);
        remove => _eventManager.RemoveEventHandler(value);
    }

    /// <summary>
    /// Gets or sets event that is triggered when the layout of an element has been updated or modified.
    /// </summary>
    public event EventHandler LayoutChanged
    {
        add => _eventManager.AddEventHandler(value);
        remove => _eventManager.RemoveEventHandler(value);
    }

    /// <summary>
    /// Occurs when the state of the bottom sheet changes.
    /// </summary>
    public event EventHandler<BottomSheetStateChangedEventArgs> StateChanged
    {
        add => _eventManager.AddEventHandler(value);
        remove => _eventManager.RemoveEventHandler(value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the user can cancel a bottom sheet.
    /// </summary>
    public bool IsCancelable
    {
        get => (bool)GetValue(IsCancelableProperty);
        set => SetValue(IsCancelableProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the object has a handle associated with it.
    /// </summary>
    public bool HasHandle
    {
        get => (bool)GetValue(HasHandleProperty);
        set => SetValue(HasHandleProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the header is displayed.
    /// </summary>
    public bool ShowHeader
    {
        get => (bool)GetValue(ShowHeaderProperty);
        set => SetValue(ShowHeaderProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the associated element is open.
    /// </summary>
    public bool IsOpen
    {
        get => (bool)GetValue(IsOpenProperty);
        set => SetValue(IsOpenProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the current presentation is modal.
    /// </summary>
    public bool IsModal
    {
        get => (bool)GetValue(IsModalProperty);
        set => SetValue(IsModalProperty, value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the user can drag the element.
    /// </summary>
    public bool IsDraggable
    {
        get => (bool)GetValue(IsDraggableProperty);
        set => SetValue(IsDraggableProperty, value);
    }

    /// <summary>
    /// Gets or sets the radius of the corners for a visual element.
    /// </summary>
    public float CornerRadius
    {
        get => (float)GetValue(CornerRadiusProperty);
        set => SetValue(CornerRadiusProperty, value);
    }

    /// <summary>
    /// Gets or sets the background color of the window.
    /// </summary>
    public Color WindowBackgroundColor
    {
        get => (Color)GetValue(WindowBackgroundColorProperty);
        set => SetValue(WindowBackgroundColorProperty, value);
    }

    /// <summary>
    /// Gets or sets the height of the bottom sheet when it is in its peek state.
    /// </summary>
    public double PeekHeight
    {
        get => (double)GetValue(PeekHeightProperty);
        set => SetValue(PeekHeightProperty, value);
    }

    /// <summary>
    /// Gets or sets the header text or content of a control.
    /// </summary>
    public BottomSheetHeader? Header
    {
        get => (BottomSheetHeader?)GetValue(HeaderProperty);
        set => SetValue(HeaderProperty, value);
    }

    /// <summary>
    /// Gets or sets the current state of the object or component.
    /// </summary>
    public BottomSheetState CurrentState
    {
        get => (BottomSheetState)GetValue(CurrentStateProperty);
        set => SetValue(CurrentStateProperty, value);
    }

    /// <summary>
    /// Gets or sets the command executed when a close action is performed.
    /// </summary>
    public ICommand? ClosingCommand
    {
        get => (ICommand?)GetValue(ClosingCommandProperty);
        set => SetValue(ClosingCommandProperty, value);
    }

    /// <summary>
    /// Gets or sets a parameter to the command executed when closing.
    /// </summary>
    public object? ClosingCommandParameter
    {
        get => (object?)GetValue(ClosingCommandParameterProperty);
        set => SetValue(ClosingCommandParameterProperty, value);
    }

    /// <summary>
    /// Gets or sets the command to be executed when the associated view is closed.
    /// </summary>
    public ICommand? ClosedCommand
    {
        get => (ICommand?)GetValue(ClosedCommandProperty);
        set => SetValue(ClosedCommandProperty, value);
    }

    /// <summary>
    /// Gets or sets the command parameter associated with the closed action.
    /// </summary>
    public object? ClosedCommandParameter
    {
        get => (object?)GetValue(ClosedCommandParameterProperty);
        set => SetValue(ClosedCommandParameterProperty, value);
    }

    /// <summary>
    /// Gets or sets the command to execute when opening occurs.
    /// </summary>
    public ICommand? OpeningCommand
    {
        get => (ICommand?)GetValue(OpeningCommandProperty);
        set => SetValue(OpeningCommandProperty, value);
    }

    /// <summary>
    /// Gets or sets the parameter to be passed to the command executed when opening occurs.
    /// </summary>
    public object? OpeningCommandParameter
    {
        get => (object?)GetValue(OpeningCommandParameterProperty);
        set => SetValue(OpeningCommandParameterProperty, value);
    }

    /// <summary>
    /// Gets or sets the command that is executed when the associated object is opened.
    /// </summary>
    public ICommand? OpenedCommand
    {
        get => (ICommand?)GetValue(OpenedCommandProperty);
        set => SetValue(OpenedCommandProperty, value);
    }

    /// <summary>
    /// Gets or sets a parameter to pass to the command when the associated item is opened.
    /// </summary>
    public object? OpenedCommandParameter
    {
        get => (object?)GetValue(OpenedCommandParameterProperty);
        set => SetValue(OpenedCommandParameterProperty, value);
    }

    /// <summary>
    /// Gets or sets the content of the control.
    /// </summary>
    public BottomSheetContent? Content
    {
        get => (BottomSheetContent?)GetValue(ContentProperty);
        set => SetValue(ContentProperty, value);
    }

    /// <summary>
    /// Gets or sets the padding values applied to the content of the BottomSheet.
    /// </summary>
    public MauiThickness Padding
    {
        get => (MauiThickness)GetValue(PaddingProperty);
        set => SetValue(PaddingProperty, value);
    }

    /// <summary>
    /// Gets or sets the style of the bottom sheet,
    /// allowing customization of its appearance and behavior.
    /// </summary>
    public BottomSheetStyle BottomSheetStyle
    {
        get => (BottomSheetStyle)GetValue(BottomSheetStyleProperty);
        set => SetValue(BottomSheetStyleProperty, value);
    }

    /// <summary>
    /// Gets or sets the collection of states that define the behavior of the bottom sheet.
    /// </summary>
    [TypeConverter(typeof(StringToBottomSheetStateTypeConverter))]
    public ICollection<BottomSheetState> States
    {
        get => (ICollection<BottomSheetState>)GetValue(StatesProperty);
        set => SetValue(StatesProperty, value);
    }

    /// <summary>
    /// Gets the container view element.
    /// </summary>
    // Do not move initialization into constructor. See issue #200. Global implicit styles set BottomSheet properties before constructor is finished.
    public Grid ContainerView { get; } = new()
    {
        RowDefinitions = new RowDefinitionCollection(
            new RowDefinition(GridLength.Auto),
            new RowDefinition(GridLength.Auto),
            new RowDefinition(GridLength.Star)),
    };

    /// <summary>
    /// Gets or sets a property that defines the sizing behavior of the bottom sheet,
    /// determining whether it adapts to predefined states or adjusts to fit its content.
    /// </summary>
    public BottomSheetSizeMode SizeMode
    {
        get => (BottomSheetSizeMode)GetValue(SizeModeProperty);
        set => SetValue(SizeModeProperty, value);
    }

    /// <summary>
    /// Activates the current instance and sets its state to "On".
    /// </summary>
    /// <typeparam name="T">The tyepe.</typeparam>
    /// <returns>A boolean value indicating whether the operation was successful.</returns>
    public IPlatformElementConfiguration<T, BottomSheet> On<T>()
        where T : IConfigPlatform
    {
        return _platformConfigurationRegistry.Value.On<T>();
    }

    /// <summary>
    /// Represents the method or event triggered when a bottom sheet is about to be opened.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public void OnOpeningBottomSheet()
    {
        ContainerView.Parent = this;
        ContainerView.BindingContext = BindingContext;

        CreateLayout();

        RaiseEvent(nameof(Opening), EventArgs.Empty);
        ExecuteCommand(OpeningCommand, OpeningCommandParameter);
    }

    /// <summary>
    /// Represents the event handler or callback triggered when a bottom sheet is opened.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public void OnOpenedBottomSheet()
    {
        RaiseEvent(nameof(Opened), EventArgs.Empty);
        ExecuteCommand(OpenedCommand, OpenedCommandParameter);
    }

    /// <summary>
    /// Performs necessary operations when the bottom sheet is in the process of closing.
    /// This method can be used to handle cleanup, state saving, or related actions before the bottom sheet is fully closed.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public void OnClosingBottomSheet()
    {
        RaiseEvent(nameof(Closing), EventArgs.Empty);
        ExecuteCommand(ClosingCommand, ClosingCommandParameter);
    }

    /// <summary>
    /// Triggers when the bottom sheet is closed, allowing handling of related events or cleanup tasks.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public void OnClosedBottomSheet()
    {
        RemoveHandle();

        if (Header is not null)
        {
            RemoveHeader(Header);
        }

        if (Content is not null)
        {
            RemoveContent(Content);
        }

        ContainerView.Parent = null;
        ContainerView.BindingContext = null;
        ContainerView.DisconnectHandlers();

        RaiseEvent(nameof(Closed), EventArgs.Empty);
        ExecuteCommand(ClosedCommand, ClosedCommandParameter);
    }

    /// <summary>
    /// Handles layout changes and performs the necessary updates or adjustments
    /// in response to the modification of the layout dimensions or properties.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public void OnLayoutChanged()
    {
        RaiseEvent(nameof(LayoutChanged), EventArgs.Empty);
    }

    /// <summary>
    /// Cancels the current bottom sheet.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public void Cancel()
    {
        Handler?.Invoke(nameof(Cancel));
    }

    /// <summary>
    /// Called when the binding context of the element changes.
    /// Override this method to perform actions when the binding context is updated.
    /// </summary>
    protected override void OnBindingContextChanged()
    {
        base.OnBindingContextChanged();

        ContainerView.BindingContext = BindingContext;

        if (Header is not null)
        {
            Header.BindingContext = ContainerView.BindingContext;
        }

        if (Content is not null)
        {
            Content.BindingContext = ContainerView.BindingContext;
        }
    }

    /// <summary>
    /// Executes the specified command with the given parameter if the command can be executed.
    /// </summary>
    /// <param name="command">The command to execute.</param>
    /// <param name="commandParameter">The parameter to pass to the command.</param>
    private static void ExecuteCommand(ICommand? command, object? commandParameter)
    {
        if (command?.CanExecute(commandParameter) == true)
        {
            command.Execute(commandParameter);
        }
    }

    /// <summary>
    /// Handles changes to the States property.
    /// </summary>
    /// <param name="bindable">The object to which the property is bound.</param>
    /// <param name="oldValue">The previous value of the property.</param>
    /// <param name="newValue">The new value of the property.</param>
    private static void OnStatesPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        => ((BottomSheet)bindable).OnStatesPropertyChanged((List<BottomSheetState>)newValue);

    /// <summary>
    /// Handles changes to the <see cref="BottomSheet.HeaderProperty"/>.
    /// </summary>
    /// <param name="bindable">The bindable object on which the property change occurred.</param>
    /// <param name="oldValue">The previous value of the Header property.</param>
    /// <param name="newValue">The new value of the Header property.</param>
    private static void OnHeaderChanged(BindableObject bindable, object oldValue, object newValue)
        => ((BottomSheet)bindable).OnHeaderChanged((BottomSheetHeader)oldValue, (BottomSheetHeader)newValue);

    /// <summary>
    /// Responds to changes in the Content property of the <see cref="BottomSheet"/>.
    /// </summary>
    /// <param name="bindable">The bindable object whose property has changed.</param>
    /// <param name="oldValue">The previous value of the Content property.</param>
    /// <param name="newValue">The new value of the Content property.</param>
    private static void OnContentChanged(BindableObject bindable, object oldValue, object newValue)
        => ((BottomSheet)bindable).OnContentChanged((BottomSheetContent)oldValue, (BottomSheetContent)newValue);

    /// <summary>
    /// Handles changes to the <see cref="ShowHeaderProperty"/> bindable property.
    /// </summary>
    /// <param name="bindable">
    /// The <see cref="BindableObject"/> instance on which the property change occurred.
    /// </param>
    /// <param name="oldValue">
    /// The old value of the property before the change.
    /// </param>
    /// <param name="newValue">
    /// The new value of the property after the change.
    /// </param>
    private static void OnShowHeaderPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        => ((BottomSheet)bindable).OnShowHeaderPropertyChanged();

    /// <summary>
    /// Handles changes to the <see cref="BottomSheetStyleProperty"/>.
    /// </summary>
    /// <param name="bindable">The instance of <see cref="BindableObject"/> where the property change occurred.</param>
    /// <param name="oldValue">The old value of the <see cref="BottomSheetStyle"/> property before the change.</param>
    /// <param name="newValue">The new value of the <see cref="BottomSheetStyle"/> property after the change.</param>
    private static void OnBottomSheetStylePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        => ((BottomSheet)bindable).OnBottomSheetStylePropertyChanged((BottomSheetStyle)oldValue, (BottomSheetStyle)newValue);

    /// <summary>
    /// Handles the property changed event for the <see cref="HasHandleProperty"/> BindableProperty.
    /// </summary>
    /// <param name="bindable">The object that the property belongs to.</param>
    /// <param name="oldValue">The previous value of the property.</param>
    /// <param name="newValue">The new value of the property.</param>
    private static void OnHasHandlePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        => ((BottomSheet)bindable).OnHasHandlePropertyChanged();

    /// <summary>
    /// Handles changes to the <see cref="PaddingProperty"/> bindable property.
    /// This method is invoked when the <see cref="Padding"/> value is modified.
    /// </summary>
    /// <param name="bindable">The object on which the property was changed.</param>
    /// <param name="oldValue">The previous value of the property.</param>
    /// <param name="newValue">The new value of the property.</param>
    private static void OnPaddingPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        => ((BottomSheet)bindable).OnPaddingPropertyChanged();

    /// <summary>
    /// Handles the property change event for the <see cref="BottomSheet.CurrentStateProperty"/>.
    /// Updates the state of the <see cref="BottomSheet"/> when the <see cref="BottomSheetState"/> changes.
    /// </summary>
    /// <param name="bindable">
    /// The <see cref="BindableObject"/> that contains the <see cref="BottomSheet"/> instance whose property changed.
    /// </param>
    /// <param name="oldValue">
    /// The previous value of the <see cref="BottomSheetState"/> before the change.
    /// </param>
    /// <param name="newValue">
    /// The new value of the <see cref="BottomSheetState"/> after the change.
    /// </param>
    private static void CurrentStatePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        => ((BottomSheet)bindable).CurrentStatePropertyChanged((BottomSheetState)oldValue, (BottomSheetState)newValue);

    /// <summary>
    /// Handles changes to the current state of the <see cref="BottomSheet"/> control.
    /// Invoked when the <see cref="StatesProperty"/> changes.
    /// </summary>
    /// <param name="oldValue">
    /// The previous <see cref="BottomSheetState"/> before the change.
    /// </param>
    /// <param name="newValue">
    /// The new <see cref="BottomSheetState"/> after the change.
    /// </param>
    private void CurrentStatePropertyChanged(BottomSheetState oldValue, BottomSheetState newValue)
    {
        _eventManager.HandleEvent(
            this,
            new BottomSheetStateChangedEventArgs(oldValue, newValue),
            nameof(StateChanged));
    }

    /// <summary>
    /// Handles the change in the padding property of the <see cref="BottomSheet"/>
    /// and updates the layout padding and the handle margin accordingly.
    /// </summary>
    private void OnPaddingPropertyChanged()
    {
        ContainerView.Padding = Padding;

        if (ContainerView.Children.FirstOrDefault(child => ContainerView.GetRow(child) == HandleRow) is View handle)
        {
            handle.Margin = new(
                handle.Margin.Left,
                handle.Margin.Top - Padding.Top,
                handle.Margin.Right,
                handle.Margin.Bottom);
        }
    }

    /// <summary>
    /// Triggers the specified event and notifies all registered event handlers.
    /// </summary>
    /// <param name="eventName">The name of the event to be raised.</param>
    /// <param name="eventArgs">The arguments associated with the event being raised.</param>
    private void RaiseEvent(string eventName, EventArgs eventArgs)
    {
        _eventManager.HandleEvent(this, eventArgs, eventName);
    }

    /// <summary>
    /// Handles the logic to update the state of the <see cref="BottomSheet"/> when the associated states collection changes.
    /// </summary>
    /// <param name="newValue">The updated collection of allowed <see cref="BottomSheetState"/> values.</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Minor Code Smell", "S6608:Prefer indexing instead of \"Enumerable\" methods on types implementing \"IList\"", Justification = "Improced readability.")]
    private void OnStatesPropertyChanged(List<BottomSheetState> newValue)
    {
        CurrentState = newValue.Order().First();
    }

    /// <summary>
    /// Called when the Header property of the <see cref="BottomSheet"/> changes.
    /// </summary>
    /// <param name="oldValue">The previous value of the Header property.</param>
    /// <param name="newValue">The new value of the Header property.</param>
    private void OnHeaderChanged(BottomSheetHeader? oldValue, BottomSheetHeader? newValue)
    {
        if (oldValue is not null)
        {
            RemoveHeader(oldValue);
        }

        if (newValue is not null
            && IsOpen)
        {
            OnShowHeaderPropertyChanged();
        }
    }

    /// <summary>
    /// Handles the internal logic when the content of the bottom sheet changes.
    /// </summary>
    /// <param name="oldValue">The previous content of the bottom sheet.</param>
    /// <param name="newValue">The new content of the bottom sheet.</param>
    private void OnContentChanged(BottomSheetContent? oldValue, BottomSheetContent? newValue)
    {
        if (oldValue is not null)
        {
            RemoveContent(oldValue);
        }

        if (newValue is not null
            && IsOpen)
        {
            AddContent(newValue);
        }
    }

    /// <summary>
    /// Handles changes to the ShowHeader property and updates the visibility and state of the Header in the BottomSheet.
    /// </summary>
    /// <param name="bindable">
    /// The source of the binding, which is the BottomSheet instance.
    /// </param>
    /// <param name="oldValue">
    /// The previous value of the ShowHeader property.
    /// </param>
    /// <param name="newValue">
    /// The new value of the ShowHeader property.
    /// </param>
    private void OnShowHeaderPropertyChanged()
    {
        if (Header is not null)
        {
            if (ShowHeader == false)
            {
                RemoveHeader(Header);
            }
            else if (IsOpen)
            {
                AddHeader(Header);
            }
        }
    }

    /// <summary>
    /// Handles changes to the BottomSheet's style property.
    /// Unsubscribes from the <see cref="BottomSheetStyle"/> property change events of the old value,
    /// and subscribes to the new style's property change events.
    /// Applies the new style to the Header if defined.
    /// </summary>
    /// <param name="oldValue">The previous style of type <see cref="BottomSheetStyle"/>.</param>
    /// <param name="newValue">The new style of type <see cref="BottomSheetStyle"/>.</param>
    private void OnBottomSheetStylePropertyChanged(BottomSheetStyle? oldValue, BottomSheetStyle? newValue)
    {
        if (oldValue is not null)
        {
            oldValue.PropertyChanged -= OnStylePropertyChanged;
        }

        if (newValue is not null)
        {
            if (Header is not null)
            {
                Header.Style = newValue.HeaderStyle;
            }

            newValue.PropertyChanged += OnStylePropertyChanged;
        }
    }

    /// <summary>
    /// Handles property change events for the style-related properties of the <see cref="BottomSheetStyle"/> class.
    /// </summary>
    /// <param name="sender">
    /// The source of the property changed event.
    /// </param>
    /// <param name="e">
    /// An instance of <see cref="PropertyChangedEventArgs"/> that contains the event data,
    /// including the name of the property that changed.
    /// </param>
    private void OnStylePropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (Header is not null
            && e.PropertyName == nameof(BottomSheetStyle.HeaderStyle))
        {
            Header.Style = BottomSheetStyle.HeaderStyle;
        }
    }

    /// <summary>
    /// Handles the loaded event for the <see cref="BottomSheet"/> component.
    /// Ensures that necessary event handlers, such as <see cref="OnUnloaded"/> and
    /// </summary>
    /// <param name="sender">
    /// The source of the event, typically the <see cref="BottomSheet"/> instance.
    /// </param>
    /// <param name="e">
    /// Contains the event data associated with the loaded event.
    /// </param>
    private void OnLoaded(object? sender, EventArgs e)
    {
        Unloaded += OnUnloaded;
    }

    /// <summary>
    /// Disconnects the handler manually to address scenarios where MAUI does not call the disconnect handler.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The event data.</param>
    private void OnUnloaded(object? sender, EventArgs e)
    {
        IsOpen = false;
        Unloaded -= OnUnloaded;
    }

    /// <summary>
    /// Handles the change in the <see cref="BottomSheet.HasHandle"/> property
    /// and updates the BottomSheet's handle UI accordingly.
    /// </summary>
    private void OnHasHandlePropertyChanged()
    {
        if (HasHandle == false)
        {
            RemoveHandle();
        }
        else
        {
            AddHandle();
        }
    }

    /// <summary>
    /// Creates and arranges the layout elements for the bottom sheet, including the handle, header,
    /// and content, based on their respective configurations.
    /// </summary>
    private void CreateLayout()
    {
        bool isInModalPage = Parent is Page page
            && Navigation.ModalStack.Contains(page);

        if (HasHandle)
        {
            if (isInModalPage)
            {
                RemoveHandle();
            }

            AddHandle();
        }

        if (Header is not null
            && ShowHeader)
        {
            if (isInModalPage)
            {
                RemoveHeader(Header);
            }

            AddHeader(Header);
        }

        if (Content is not null)
        {
            if (isInModalPage)
            {
                RemoveContent(Content);
            }

            AddContent(Content);
        }
    }

    /// <summary>
    /// Adds a handle element to the bottom sheet layout at the specified row.
    /// </summary>
    private void AddHandle()
    {
        // ReSharper disable once RedundantArgumentDefaultValue
        ContainerView.Add(CreateHandle(), 0, HandleRow);
    }

    /// <summary>
    /// Adds the specified <see cref="BottomSheetHeader"/> content to the bottom sheet layout at the header row position.
    /// </summary>
    /// <param name="header">The <see cref="BottomSheetHeader"/> instance to be added to the layout.</param>
    private void AddHeader(BottomSheetHeader header)
    {
        header.Parent = ContainerView;
        header.BindingContext = BindingContext;
        header.Style = BottomSheetStyle.HeaderStyle;
        header.CloseButtonClicked += BottomSheetHeaderOnCloseButtonClicked;

        ContainerView.Add(header.CreateContent(), 0, HeaderRow);
    }

    /// <summary>
    /// Adds content to the current context.
    /// </summary>
    /// <param name="content">The content to be added.</param>
    private void AddContent(BottomSheetContent content)
    {
        content.Parent = ContainerView;
        content.BindingContext = ContainerView.BindingContext;

        ContainerView.Add(content.CreateContent(), 0, ContentRow);
    }

    /// <summary>
    /// Removes the handle view from the BottomSheet layout, if it exists.
    /// This method checks for a child element in the layout that corresponds to the handle row
    /// and removes it from the layout.
    /// </summary>
    private void RemoveHandle()
    {
        ContainerView.Remove(ContainerView.Children.FirstOrDefault(child => ContainerView.GetRow(child) == HandleRow));
    }

    /// <summary>
    /// Removes the header from the bottom sheet container and cleans up related resources.
    /// </summary>
    /// <param name="header">The instance of <see cref="BottomSheetHeader"/> to be removed.</param>
    /// <remarks>
    /// This method detaches the header from the container, unsubscribes from the close button click event,
    /// and performs cleanup by invoking the remove logic specific to the header.
    /// </remarks>
    private void RemoveHeader(BottomSheetHeader header)
    {
        ContainerView.Remove(ContainerView.Children.FirstOrDefault(child => ContainerView.GetRow(child) == HeaderRow));
        header.CloseButtonClicked -= BottomSheetHeaderOnCloseButtonClicked;
        header.Remove();
    }

    private void RemoveContent(BottomSheetContent content)
    {
        ContainerView.Remove(ContainerView.Children.FirstOrDefault(child => ContainerView.GetRow(child) == ContentRow));
        content.Remove();
    }

    /// <summary>
    /// Creates and returns a new handle for the bottom sheet.
    /// The handle is designed as a draggable UI element to allow user interaction for opening or closing the sheet.
    /// </summary>
    /// <returns>A <see cref="Border"/> element configured as the handle, including margin, dimensions, and styling characteristics.</returns>
    private Border CreateHandle()
    {
        return new()
        {
            AutomationId = AutomationIds.Handle,
            Margin = new(0, 10 - Padding.Top, 0, 10),
            WidthRequest = 40,
            HeightRequest = 7.5,
            Content = new BoxView()
            {
                WidthRequest = 40,
                Color = Colors.Gray,
            },
            StrokeShape = new RoundRectangle()
            {
                CornerRadius = new(20),
            },
            Stroke = Colors.Gray,
        };
    }

    /// <summary>
    /// Handles the <see cref="BottomSheetHeader.CloseButtonClicked"/> event
    /// for the bottom sheet header to close or dismiss the bottom sheet.
    /// </summary>
    /// <param name="sender">The source of the event, typically the <see cref="BottomSheetHeader"/> instance.</param>
    /// <param name="e">The event data associated with the button click event.</param>
    private void BottomSheetHeaderOnCloseButtonClicked(object? sender, EventArgs e)
    {
        Cancel();
    }
}