namespace Plugin.Maui.BottomSheet;

using System.ComponentModel;
using System.Windows.Input;

/// <inheritdoc cref="IBottomSheet" />
public class BottomSheet : View, IBottomSheet
{
    /// <summary>
    /// Bindable property.
    /// </summary>
    public static readonly BindableProperty IgnoreSafeAreaProperty =
        BindableProperty.Create(
            nameof(IgnoreSafeArea),
            typeof(bool),
            typeof(BottomSheet));

    /// <summary>
    /// Bindable property.
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
    /// Bindable property.
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
    /// Bindable property.
    /// </summary>
    public static readonly BindableProperty IsCancelableProperty =
        BindableProperty.Create(
            nameof(IsCancelable),
            typeof(bool),
            typeof(BottomSheet),
            defaultValue: true,
            defaultBindingMode: BindingMode.TwoWay);

    /// <summary>
    /// Bindable property.
    /// </summary>
    public static readonly BindableProperty HasHandleProperty =
        BindableProperty.Create(
            nameof(HasHandle),
            typeof(bool),
            typeof(BottomSheet),
            defaultValue: true,
            defaultBindingMode: BindingMode.TwoWay);

    /// <summary>
    /// Bindable property.
    /// </summary>
    public static readonly BindableProperty ShowHeaderProperty =
        BindableProperty.Create(
            nameof(ShowHeader),
            typeof(bool),
            typeof(BottomSheet),
            defaultBindingMode: BindingMode.TwoWay);

    /// <summary>
    /// Bindable property.
    /// </summary>
    public static readonly BindableProperty IsOpenProperty =
        BindableProperty.Create(
            nameof(IsOpen),
            typeof(bool),
            typeof(BottomSheet),
            defaultBindingMode: BindingMode.TwoWay);

    /// <summary>
    /// Bindable property.
    /// </summary>
    public static readonly BindableProperty IsDraggableProperty =
        BindableProperty.Create(
            nameof(IsDraggable),
            typeof(bool),
            typeof(BottomSheet),
            defaultValue: true,
            defaultBindingMode: BindingMode.TwoWay);

    /// <summary>
    /// Bindable property.
    /// </summary>
    public static readonly BindableProperty PeekProperty =
        BindableProperty.Create(
            nameof(Peek),
            typeof(BottomSheetPeek),
            typeof(BottomSheet),
            propertyChanged: OnPeekChanged);

    /// <summary>
    /// Bindable property.
    /// </summary>
    public static readonly BindableProperty HeaderProperty =
        BindableProperty.Create(
            nameof(Header),
            typeof(BottomSheetHeader),
            typeof(BottomSheet),
            propertyChanged: OnHeaderChanged);

    /// <summary>
    /// Bindable property.
    /// </summary>
    public static readonly BindableProperty ContentProperty =
        BindableProperty.Create(
            nameof(Content),
            typeof(BottomSheetContent),
            typeof(BottomSheet),
            propertyChanged: OnContentChanged);

    /// <summary>
    /// Bindable property.
    /// </summary>
    public static readonly BindableProperty PaddingProperty =
        BindableProperty.Create(
            nameof(Peek),
            typeof(Thickness),
            typeof(BottomSheet),
            defaultValue: new Thickness(5));

    /// <summary>
    /// Bindable property.
    /// </summary>
    public static readonly BindableProperty ClosingCommandProperty =
        BindableProperty.Create(
            nameof(ClosingCommand),
            typeof(ICommand),
            typeof(BottomSheet));

    /// <summary>
    /// Bindable property.
    /// </summary>
    public static readonly BindableProperty ClosingCommandParameterProperty =
        BindableProperty.Create(
            nameof(ClosingCommandParameter),
            typeof(object),
            typeof(BottomSheet));

    /// <summary>
    /// Bindable property.
    /// </summary>
    public static readonly BindableProperty ClosedCommandProperty =
        BindableProperty.Create(
            nameof(ClosedCommand),
            typeof(ICommand),
            typeof(BottomSheet));

    /// <summary>
    /// Bindable property.
    /// </summary>
    public static readonly BindableProperty ClosedCommandParameterProperty =
        BindableProperty.Create(
            nameof(ClosedCommandParameter),
            typeof(object),
            typeof(BottomSheet));

    /// <summary>
    /// Bindable property.
    /// </summary>
    public static readonly BindableProperty OpeningCommandProperty =
        BindableProperty.Create(
            nameof(OpeningCommand),
            typeof(ICommand),
            typeof(BottomSheet));

    /// <summary>
    /// Bindable property.
    /// </summary>
    public static readonly BindableProperty OpeningCommandParameterProperty =
        BindableProperty.Create(
            nameof(OpeningCommandParameter),
            typeof(object),
            typeof(BottomSheet));

    /// <summary>
    /// Bindable property.
    /// </summary>
    public static readonly BindableProperty OpenedCommandProperty =
        BindableProperty.Create(
            nameof(OpenedCommand),
            typeof(ICommand),
            typeof(BottomSheet));

    /// <summary>
    /// Bindable property.
    /// </summary>
    public static readonly BindableProperty OpenedCommandParameterProperty =
        BindableProperty.Create(
            nameof(OpenedCommandParameter),
            typeof(object),
            typeof(BottomSheet));

    private readonly WeakEventManager _eventManager = new();

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
    /// Gets or sets a value indicating whether the <see cref="IBottomSheet"/> ignores safe areas.
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
    public bool IsDraggable { get => (bool)GetValue(IsDraggableProperty); set => SetValue(IsDraggableProperty, value); }

    /// <inheritdoc/>
    public BottomSheetPeek? Peek { get => (BottomSheetPeek?)GetValue(PeekProperty); set => SetValue(PeekProperty, value); }

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
    [TypeConverter(typeof(StringToBottomSheetStateTypeConverter))]
    public ICollection<BottomSheetState> States
    {
        get => (ICollection<BottomSheetState>)GetValue(StatesProperty);
        set => SetValue(StatesProperty, value);
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

    /// <inheritdoc/>
    protected override void OnParentChanged()
    {
        base.OnParentChanged();

        if (Header is not null)
        {
            Header.Parent = Parent;
        }

        if (Peek is not null)
        {
            Peek.Parent = Parent;
        }

        if (Content is not null)
        {
            Content.Parent = Parent;
        }
    }

    /// <inheritdoc/>
    protected override void OnBindingContextChanged()
    {
        base.OnBindingContextChanged();

        if (Header is not null)
        {
            Header.BindingContext = BindingContext;
        }

        if (Peek is not null)
        {
            Peek.BindingContext = BindingContext;
        }

        if (Content is not null)
        {
            Content.BindingContext = BindingContext;
        }
    }

    private static void ExecuteCommand(ICommand? command, object? commandParameter)
    {
        if (command?.CanExecute(commandParameter) == true)
        {
            command.Execute(commandParameter);
        }
    }

    private static void OnStatesPropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
        => ((BottomSheet)bindable).OnStatesPropertyChanged((List<BottomSheetState>)newvalue);

    private static void OnHeaderChanged(BindableObject bindable, object oldvalue, object newvalue)
        => ((BottomSheet)bindable).OnHeaderChanged((BottomSheetHeader)newvalue);

    private static void OnPeekChanged(BindableObject bindable, object oldvalue, object newvalue)
        => ((BottomSheet)bindable).OnPeekChanged((BottomSheetPeek)newvalue);

    private static void OnContentChanged(BindableObject bindable, object oldvalue, object newvalue)
        => ((BottomSheet)bindable).OnContentChanged((BottomSheetContent)newvalue);

    private void RaiseEvent(string eventName, EventArgs eventArgs)
    {
        _eventManager.HandleEvent(this, eventArgs, eventName);
    }

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

    private void OnHeaderChanged(BottomSheetHeader newValue)
    {
        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        if (newValue is not null)
        {
            newValue.Parent = this;
            newValue.BindingContext = BindingContext;
        }
    }

    private void OnPeekChanged(BottomSheetPeek newValue)
    {
        // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        if (newValue is not null)
        {
            newValue.Parent = this;
            newValue.BindingContext = BindingContext;
        }
    }

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