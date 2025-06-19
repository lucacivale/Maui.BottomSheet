namespace Plugin.Maui.BottomSheet;

using System.ComponentModel;
using System.Windows.Input;

/// <summary>
/// Implementation of a bottom sheet control that provides supplementary content anchored to the bottom of the screen.
/// </summary>
public class BottomSheet : View, IBottomSheet, IElementConfiguration<BottomSheet>
{
    /// <summary>
    /// Bindable property for the modal presentation mode.
    /// </summary>
    public static readonly BindableProperty IsModalProperty =
        BindableProperty.Create(
            nameof(IsModal),
            typeof(bool),
            typeof(BottomSheet),
            defaultValue: true);

    /// <summary>
    /// Bindable property for the corner radius of the bottom sheet.
    /// </summary>
    public static readonly BindableProperty CornerRadiusProperty =
        BindableProperty.Create(
            nameof(CornerRadius),
            typeof(float),
            typeof(BottomSheet),
            defaultValue: 20.0f);

    /// <summary>
    /// Bindable property for the window background color behind the bottom sheet.
    /// </summary>
    public static readonly BindableProperty WindowBackgroundColorProperty =
        BindableProperty.Create(
            nameof(WindowBackgroundColor),
            typeof(Color),
            typeof(BottomSheet),
            defaultValueCreator: _ => Color.FromArgb("#80000000"));

    /// <summary>
    /// Bindable property for the safe area ignore setting.
    /// </summary>
    public static readonly BindableProperty IgnoreSafeAreaProperty =
        BindableProperty.Create(
            nameof(IgnoreSafeArea),
            typeof(bool),
            typeof(BottomSheet));

    /// <summary>
    /// Bindable property for the collection of available bottom sheet states.
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
            validateValue: (_, value) =>
            {
                var result = true;
                var states = (List<BottomSheetState>)value;

                if (states.Count == 0)
                {
                    result = false;
                }

                return result;
            });

    /// <summary>
    /// Bindable property for the current state of the bottom sheet.
    /// </summary>
    public static readonly BindableProperty CurrentStateProperty =
        BindableProperty.Create(
            nameof(CurrentState),
            typeof(BottomSheetState),
            typeof(BottomSheet),
            defaultBindingMode: BindingMode.TwoWay,
            defaultValueCreator: bindable =>
            {
                var bottomSheet = (BottomSheet)bindable;

                return bottomSheet.States.Order().First();
            },
            validateValue: (bindable, value) =>
            {
                var bottomSheet = (BottomSheet)bindable;
                var state = (BottomSheetState)value;

                return bottomSheet.States.IsStateAllowed(state);
            });

    /// <summary>
    /// Bindable property for the cancelable setting.
    /// </summary>
    public static readonly BindableProperty IsCancelableProperty =
        BindableProperty.Create(
            nameof(IsCancelable),
            typeof(bool),
            typeof(BottomSheet),
            defaultValue: true,
            defaultBindingMode: BindingMode.TwoWay);

    /// <summary>
    /// Bindable property for the drag handle visibility.
    /// </summary>
    public static readonly BindableProperty HasHandleProperty =
        BindableProperty.Create(
            nameof(HasHandle),
            typeof(bool),
            typeof(BottomSheet),
            defaultValue: true,
            defaultBindingMode: BindingMode.TwoWay);

    /// <summary>
    /// Bindable property for the header visibility.
    /// </summary>
    public static readonly BindableProperty ShowHeaderProperty =
        BindableProperty.Create(
            nameof(ShowHeader),
            typeof(bool),
            typeof(BottomSheet),
            defaultBindingMode: BindingMode.TwoWay);

    /// <summary>
    /// Bindable property for the open state of the bottom sheet.
    /// </summary>
    public static readonly BindableProperty IsOpenProperty =
        BindableProperty.Create(
            nameof(IsOpen),
            typeof(bool),
            typeof(BottomSheet),
            defaultBindingMode: BindingMode.TwoWay);

    /// <summary>
    /// Bindable property for the draggable setting.
    /// </summary>
    public static readonly BindableProperty IsDraggableProperty =
        BindableProperty.Create(
            nameof(IsDraggable),
            typeof(bool),
            typeof(BottomSheet),
            defaultValue: true,
            defaultBindingMode: BindingMode.TwoWay);

    /// <summary>
    /// Bindable property for the peek height when in peek state.
    /// </summary>
    public static readonly BindableProperty PeekHeightProperty =
        BindableProperty.Create(
            nameof(PeekHeight),
            typeof(double),
            typeof(BottomSheet),
            defaultValue: 0.00);

    /// <summary>
    /// Bindable property for the header configuration.
    /// </summary>
    public static readonly BindableProperty HeaderProperty =
        BindableProperty.Create(
            nameof(Header),
            typeof(BottomSheetHeader),
            typeof(BottomSheet),
            propertyChanged: OnHeaderChanged);

    /// <summary>
    /// Bindable property for the content configuration.
    /// </summary>
    public static readonly BindableProperty ContentProperty =
        BindableProperty.Create(
            nameof(Content),
            typeof(BottomSheetContent),
            typeof(BottomSheet),
            propertyChanged: OnContentChanged);

    /// <summary>
    /// Bindable property for the padding around the content.
    /// </summary>
    public static readonly BindableProperty PaddingProperty =
        BindableProperty.Create(
            nameof(Padding),
            typeof(Thickness),
            typeof(BottomSheet),
            defaultValue: new Thickness(5));

    /// <summary>
    /// Bindable property for the command executed when closing.
    /// </summary>
    public static readonly BindableProperty ClosingCommandProperty =
        BindableProperty.Create(
            nameof(ClosingCommand),
            typeof(ICommand),
            typeof(BottomSheet));

    /// <summary>
    /// Bindable property for the closing command parameter.
    /// </summary>
    public static readonly BindableProperty ClosingCommandParameterProperty =
        BindableProperty.Create(
            nameof(ClosingCommandParameter),
            typeof(object),
            typeof(BottomSheet));

    /// <summary>
    /// Bindable property for the command executed when closed.
    /// </summary>
    public static readonly BindableProperty ClosedCommandProperty =
        BindableProperty.Create(
            nameof(ClosedCommand),
            typeof(ICommand),
            typeof(BottomSheet));

    /// <summary>
    /// Bindable property for the closed command parameter.
    /// </summary>
    public static readonly BindableProperty ClosedCommandParameterProperty =
        BindableProperty.Create(
            nameof(ClosedCommandParameter),
            typeof(object),
            typeof(BottomSheet));

    /// <summary>
    /// Bindable property for the command executed when opening.
    /// </summary>
    public static readonly BindableProperty OpeningCommandProperty =
        BindableProperty.Create(
            nameof(OpeningCommand),
            typeof(ICommand),
            typeof(BottomSheet));

    /// <summary>
    /// Bindable property for the opening command parameter.
    /// </summary>
    public static readonly BindableProperty OpeningCommandParameterProperty =
        BindableProperty.Create(
            nameof(OpeningCommandParameter),
            typeof(object),
            typeof(BottomSheet));

    /// <summary>
    /// Bindable property for the command executed when opened.
    /// </summary>
    public static readonly BindableProperty OpenedCommandProperty =
        BindableProperty.Create(
            nameof(OpenedCommand),
            typeof(ICommand),
            typeof(BottomSheet));

    /// <summary>
    /// Bindable property for the opened command parameter.
    /// </summary>
    public static readonly BindableProperty OpenedCommandParameterProperty =
        BindableProperty.Create(
            nameof(OpenedCommandParameter),
            typeof(object),
            typeof(BottomSheet));

    /// <summary>
    /// Bindable property for the bottom sheet style configuration.
    /// </summary>
    public static readonly BindableProperty BottomSheetStyleProperty =
        BindableProperty.Create(
            nameof(BottomSheetStyle),
            typeof(BottomSheetStyle),
            typeof(BottomSheet),
            defaultValue: new BottomSheetStyle());

    private readonly WeakEventManager _eventManager = new();
    private readonly Lazy<PlatformConfigurationRegistry<BottomSheet>> _platformConfigurationRegistry;

    /// <summary>
    /// Initializes a new instance of the <see cref="BottomSheet"/> class.
    /// </summary>
    public BottomSheet()
    {
        _platformConfigurationRegistry = new Lazy<PlatformConfigurationRegistry<BottomSheet>>(() => new PlatformConfigurationRegistry<BottomSheet>(this));
    }

    /// <inheritdoc/>
    public event EventHandler Closing
    {
        add => _eventManager.AddEventHandler(value);
        remove => _eventManager.RemoveEventHandler(value);
    }

    /// <inheritdoc/>
    public event EventHandler Closed
    {
        add => _eventManager.AddEventHandler(value);
        remove => _eventManager.RemoveEventHandler(value);
    }

    /// <inheritdoc/>
    public event EventHandler Opening
    {
        add => _eventManager.AddEventHandler(value);
        remove => _eventManager.RemoveEventHandler(value);
    }

    /// <inheritdoc/>
    public event EventHandler Opened
    {
        add => _eventManager.AddEventHandler(value);
        remove => _eventManager.RemoveEventHandler(value);
    }

    /// <summary>
    /// Gets or sets a value indicating whether the bottom sheet ignores safe area constraints.
    /// </summary>
    public bool IgnoreSafeArea { get => (bool)GetValue(IgnoreSafeAreaProperty); set => SetValue(IgnoreSafeAreaProperty, value); }

    /// <inheritdoc/>
    public bool IsCancelable { get => (bool)GetValue(IsCancelableProperty); set => SetValue(IsCancelableProperty, value); }

    /// <inheritdoc/>
    public bool HasHandle { get => (bool)GetValue(HasHandleProperty); set => SetValue(HasHandleProperty, value); }

    /// <inheritdoc/>
    public bool ShowHeader { get => (bool)GetValue(ShowHeaderProperty); set => SetValue(ShowHeaderProperty, value); }

    /// <inheritdoc/>
    public bool IsOpen { get => (bool)GetValue(IsOpenProperty); set => SetValue(IsOpenProperty, value); }

    /// <inheritdoc/>
    public bool IsModal { get => (bool)GetValue(IsModalProperty); set => SetValue(IsModalProperty, value); }

    /// <inheritdoc/>
    public bool IsDraggable { get => (bool)GetValue(IsDraggableProperty); set => SetValue(IsDraggableProperty, value); }

    /// <inheritdoc/>
    public float CornerRadius { get => (float)GetValue(CornerRadiusProperty); set => SetValue(CornerRadiusProperty, value); }

    /// <inheritdoc/>
    public Color WindowBackgroundColor { get => (Color)GetValue(WindowBackgroundColorProperty); set => SetValue(WindowBackgroundColorProperty, value); }

    /// <inheritdoc/>
    public double PeekHeight { get => (double)GetValue(PeekHeightProperty); set => SetValue(PeekHeightProperty, value); }

    /// <inheritdoc/>
    public BottomSheetHeader? Header { get => (BottomSheetHeader?)GetValue(HeaderProperty); set => SetValue(HeaderProperty, value); }

    /// <inheritdoc/>
    public BottomSheetState CurrentState { get => (BottomSheetState)GetValue(CurrentStateProperty); set => SetValue(CurrentStateProperty, value); }

    /// <inheritdoc/>
    public ICommand? ClosingCommand { get => (ICommand?)GetValue(ClosingCommandProperty); set => SetValue(ClosingCommandProperty, value); }

    /// <inheritdoc/>
    public object? ClosingCommandParameter { get => (object?)GetValue(ClosingCommandParameterProperty); set => SetValue(ClosingCommandParameterProperty, value); }

    /// <inheritdoc/>
    public ICommand? ClosedCommand { get => (ICommand?)GetValue(ClosedCommandProperty); set => SetValue(ClosedCommandProperty, value); }

    /// <inheritdoc/>
    public object? ClosedCommandParameter { get => (object?)GetValue(ClosedCommandParameterProperty); set => SetValue(ClosedCommandParameterProperty, value); }

    /// <inheritdoc/>
    public ICommand? OpeningCommand { get => (ICommand?)GetValue(OpeningCommandProperty); set => SetValue(OpeningCommandProperty, value); }

    /// <inheritdoc/>
    public object? OpeningCommandParameter { get => (object?)GetValue(OpeningCommandParameterProperty); set => SetValue(OpeningCommandParameterProperty, value); }

    /// <inheritdoc/>
    public ICommand? OpenedCommand { get => (ICommand?)GetValue(OpenedCommandProperty); set => SetValue(OpenedCommandProperty, value); }

    /// <inheritdoc/>
    public object? OpenedCommandParameter { get => (object?)GetValue(OpenedCommandParameterProperty); set => SetValue(OpenedCommandParameterProperty, value); }

    /// <inheritdoc/>
    public BottomSheetContent? Content { get => (BottomSheetContent?)GetValue(ContentProperty); set => SetValue(ContentProperty, value); }

    /// <inheritdoc/>
    public Thickness Padding { get => (Thickness)GetValue(PaddingProperty); set => SetValue(PaddingProperty, value); }

    /// <inheritdoc/>
    public BottomSheetStyle BottomSheetStyle { get => (BottomSheetStyle)GetValue(BottomSheetStyleProperty); set => SetValue(BottomSheetStyleProperty, value); }

    /// <inheritdoc/>
    [TypeConverter(typeof(StringToBottomSheetStateTypeConverter))]
    public ICollection<BottomSheetState> States
    {
        get => (ICollection<BottomSheetState>)GetValue(StatesProperty);
        set => SetValue(StatesProperty, value);
    }

    /// <summary>
    /// Gets the platform-specific configuration for this bottom sheet.
    /// </summary>
    /// <typeparam name="T">The platform type.</typeparam>
    /// <returns>The platform element configuration.</returns>
    public IPlatformElementConfiguration<T, BottomSheet> On<T>()
        where T : IConfigPlatform
    {
        return _platformConfigurationRegistry.Value.On<T>();
    }

#pragma warning disable CA1033
    /// <inheritdoc/>
    void IBottomSheet.OnOpeningBottomSheet()
    {
        RaiseEvent(nameof(Opening), EventArgs.Empty);
        ExecuteCommand(OpeningCommand, OpeningCommandParameter);
    }

    /// <inheritdoc/>
    void IBottomSheet.OnOpenedBottomSheet()
    {
        RaiseEvent(nameof(Opened), EventArgs.Empty);
        ExecuteCommand(OpenedCommand, OpenedCommandParameter);
    }

    /// <inheritdoc/>
    void IBottomSheet.OnClosingBottomSheet()
    {
        RaiseEvent(nameof(Closing), EventArgs.Empty);
        ExecuteCommand(ClosingCommand, ClosingCommandParameter);
    }

    /// <inheritdoc/>
    void IBottomSheet.OnClosedBottomSheet()
    {
        RaiseEvent(nameof(Closed), EventArgs.Empty);
        ExecuteCommand(ClosedCommand, ClosedCommandParameter);
    }
#pragma warning restore CA1033

    /// <summary>
    /// Called when the binding context changes, propagating the change to child elements.
    /// </summary>
    protected override void OnBindingContextChanged()
    {
        base.OnBindingContextChanged();

        if (Header is not null)
        {
            Header.BindingContext = BindingContext;
        }

        if (Content is not null)
        {
            Content.BindingContext = BindingContext;
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
    /// <param name="bindable">The bindable object.</param>
    /// <param name="oldvalue">The old value.</param>
    /// <param name="newvalue">The new value.</param>
    private static void OnStatesPropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
        => ((BottomSheet)bindable).OnStatesPropertyChanged((List<BottomSheetState>)newvalue);

    /// <summary>
    /// Handles changes to the Header property.
    /// </summary>
    /// <param name="bindable">The bindable object.</param>
    /// <param name="oldvalue">The old value.</param>
    /// <param name="newvalue">The new value.</param>
    private static void OnHeaderChanged(BindableObject bindable, object oldvalue, object newvalue)
        => ((BottomSheet)bindable).OnHeaderChanged((BottomSheetHeader)newvalue);

    /// <summary>
    /// Handles changes to the Content property.
    /// </summary>
    /// <param name="bindable">The bindable object.</param>
    /// <param name="oldvalue">The old value.</param>
    /// <param name="newvalue">The new value.</param>
    private static void OnContentChanged(BindableObject bindable, object oldvalue, object newvalue)
        => ((BottomSheet)bindable).OnContentChanged((BottomSheetContent)newvalue);

    /// <summary>
    /// Raises the specified event with the given event arguments.
    /// </summary>
    /// <param name="eventName">The name of the event to raise.</param>
    /// <param name="eventArgs">The event arguments.</param>
    private void RaiseEvent(string eventName, EventArgs eventArgs)
    {
        _eventManager.HandleEvent(this, eventArgs, eventName);
    }

    /// <summary>
    /// Handles the internal logic when the States property changes.
    /// </summary>
    /// <param name="newvalue">The new states collection.</param>
    private void OnStatesPropertyChanged(List<BottomSheetState> newvalue)
    {
        if (!newvalue.IsStateAllowed(CurrentState))
        {
            CurrentState = newvalue[0];
        }

        if (newvalue.Count == 0)
        {
            IsDraggable = false;
        }
    }

    /// <summary>
    /// Handles the internal logic when the Header property changes.
    /// </summary>
    /// <param name="newValue">The new header value.</param>
    private void OnHeaderChanged(BottomSheetHeader newValue)
    {
        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        if (newValue is not null)
        {
            newValue.Parent = this;
            newValue.BindingContext = BindingContext;
        }
    }

    /// <summary>
    /// Handles the internal logic when the Content property changes.
    /// </summary>
    /// <param name="newValue">The new content value.</param>
    private void OnContentChanged(BottomSheetContent newValue)
    {
        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        if (newValue is not null)
        {
            newValue.Parent = this;
            newValue.BindingContext = BindingContext;
        }
    }
}