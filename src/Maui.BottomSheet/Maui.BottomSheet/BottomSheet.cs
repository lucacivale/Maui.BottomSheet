using System.Windows.Input;

namespace Maui.BottomSheet;

public class BottomSheet : View, IBottomSheet
{
	readonly WeakEventManager eventManager = new();

	public BottomSheet() : base()
	{
	}


	#region Binable Properties
	public static readonly BindableProperty HasHandleProperty = BindableProperty.Create(
		nameof(HasHandle),
		typeof(bool),
		typeof(BottomSheet),
		defaultValue: false);

	public static readonly BindableProperty IsOpenProperty = BindableProperty.Create(
		nameof(IsOpen),
		typeof(bool),
		typeof(BottomSheet),
		defaultValue: false,
		defaultBindingMode: BindingMode.TwoWay,
		propertyChanging: OnIsOpenPropertyChanging,
		propertyChanged: OnIsOpenPropertyChanged);

	public static readonly BindableProperty ShowHeaderProperty = BindableProperty.Create(
		nameof(ShowHeader),
		typeof(bool),
		typeof(BottomSheet),
		defaultValue: false);

	public static readonly BindableProperty IsDraggableProperty = BindableProperty.Create(
		nameof(IsDraggable),
		typeof(bool),
		typeof(BottomSheet),
		defaultValue: true);

	public static readonly BindableProperty HeaderAppearanceProperty = BindableProperty.Create(
		nameof(HeaderAppearance),
		typeof(BottomSheetHeaderAppearanceMode),
		typeof(BottomSheet),
		defaultValue: BottomSheetHeaderAppearanceMode.None);

	public static readonly BindableProperty SheetStatesProperty = BindableProperty.Create(
		nameof(SheetStates),
		typeof(BottomSheetState),
		typeof(BottomSheet),
		defaultValue: BottomSheetState.Medium);

	public static readonly BindableProperty SelectedSheetStateProperty = BindableProperty.Create(
		nameof(SelectedSheetState),
		typeof(BottomSheetState),
		typeof(BottomSheet),
		defaultBindingMode: BindingMode.TwoWay);

	public static readonly BindableProperty TopLeftButtonTextProperty = BindableProperty.Create(
		nameof(TopLeftButtonText),
		typeof(string),
		typeof(BottomSheet),
		defaultValue: string.Empty);

	public static readonly BindableProperty TopRightButtonTextProperty = BindableProperty.Create(
		nameof(TopRightButtonText),
		typeof(string),
		typeof(BottomSheet),
		defaultValue: string.Empty);

	public static readonly BindableProperty TitleTextProperty = BindableProperty.Create(
		nameof(TitleText),
		typeof(string),
		typeof(BottomSheet),
		defaultValue: string.Empty);

	public static readonly BindableProperty TopRightButtonCommandProperty = BindableProperty.Create(
		nameof(TopRightButtonCommand),
		typeof(ICommand),
		typeof(BottomSheet));

	public static readonly BindableProperty TopRightButtonCommandParameterProperty = BindableProperty.Create(
		nameof(TopRightButtonCommandParameter),
		typeof(object),
		typeof(BottomSheet));

	public static readonly BindableProperty TopLeftButtonCommandProperty = BindableProperty.Create(
		nameof(TopLeftButtonCommand),
		typeof(ICommand),
		typeof(BottomSheet));

	public static readonly BindableProperty TopLeftButtonCommandParameterProperty = BindableProperty.Create(
		nameof(TopLeftButtonCommandParameter),
		typeof(object),
		typeof(BottomSheet));

	public static readonly BindableProperty PeekProperty = BindableProperty.Create(
		nameof(Peek),
		typeof(BottomSheetPeek),
		typeof(BottomSheet),
		defaultBindingMode: BindingMode.TwoWay,
		propertyChanged: PeekPropertyChanged);

	public static readonly BindableProperty ClosingCommandProperty = BindableProperty.Create(
		nameof(ClosingCommand),
		typeof(ICommand),
		typeof(BottomSheet));

	public static readonly BindableProperty ClosingCommandParameterProperty = BindableProperty.Create(
		nameof(ClosingCommandParameter),
		typeof(object),
		typeof(BottomSheet));

	public static readonly BindableProperty ClosedCommandProperty = BindableProperty.Create(
		nameof(ClosedCommand),
		typeof(ICommand),
		typeof(BottomSheet));

	public static readonly BindableProperty ClosedCommandParameterProperty = BindableProperty.Create(
		nameof(ClosedCommandParameter),
		typeof(object),
		typeof(BottomSheet));

	public static readonly BindableProperty OpeningCommandProperty = BindableProperty.Create(
		nameof(OpeningCommand),
		typeof(ICommand),
		typeof(BottomSheet));

	public static readonly BindableProperty OpeningCommandParameterProperty = BindableProperty.Create(
		nameof(OpeningCommandParameter),
		typeof(object),
		typeof(BottomSheet));

	public static readonly BindableProperty OpenedCommandProperty = BindableProperty.Create(
		nameof(OpenedCommand),
		typeof(ICommand),
		typeof(BottomSheet));

	public static readonly BindableProperty OpenedCommandParameterProperty = BindableProperty.Create(
		nameof(OpenedCommandParameter),
		typeof(object),
		typeof(BottomSheet));
	#endregion

	#region Properties

	#region Appearance
	public BottomSheetPeek? Peek { get => (BottomSheetPeek?)GetValue(PeekProperty); set => SetValue(PeekProperty, value); }
	public bool HasHandle { get => (bool)GetValue(HasHandleProperty); set => SetValue(HasHandleProperty, value); }
	public bool IsOpen { get => (bool)GetValue(IsOpenProperty); set => SetValue(IsOpenProperty, value); }
	public bool IsDraggable { get => (bool)GetValue(IsDraggableProperty); set => SetValue(IsDraggableProperty, value); }
	public bool ShowHeader { get => (bool)GetValue(ShowHeaderProperty); set => SetValue(ShowHeaderProperty, value); }
	public BottomSheetHeaderAppearanceMode HeaderAppearance { get => (BottomSheetHeaderAppearanceMode)GetValue(HeaderAppearanceProperty); set => SetValue(HeaderAppearanceProperty, value); }
	public BottomSheetState SheetStates { get => (BottomSheetState)GetValue(SheetStatesProperty); set => SetValue(SheetStatesProperty, value); }
	public string TopLeftButtonText { get => (string)GetValue(TopLeftButtonTextProperty); set => SetValue(TopLeftButtonTextProperty, value); }
	public string TopRightButtonText { get => (string)GetValue(TopRightButtonTextProperty); set => SetValue(TopRightButtonTextProperty, value); }
	public string TitleText { get => (string)GetValue(TitleTextProperty); set => SetValue(TitleTextProperty, value); }
	public BottomSheetState SelectedSheetState { get => (BottomSheetState)GetValue(SelectedSheetStateProperty); set => SetValue(SelectedSheetStateProperty, value); }
	#endregion

	#region DataTemplates
	public DataTemplate? ContentTemplate { get; set; }
	public DataTemplate? TitleViewTemplate { get; set; }
	#endregion

	#endregion

	#region Commands
	public ICommand? TopRightButtonCommand { get => (ICommand)GetValue(TopRightButtonCommandProperty); set => SetValue(TopRightButtonCommandProperty, value); }
	public object? TopRightButtonCommandParameter { get => GetValue(TopRightButtonCommandParameterProperty); set => SetValue(TopRightButtonCommandParameterProperty, value); }

	public object? TopLeftButtonCommandParameter { get => GetValue(TopLeftButtonCommandParameterProperty); set => SetValue(TopLeftButtonCommandParameterProperty, value); }
	public ICommand? TopLeftButtonCommand { get => (ICommand)GetValue(TopLeftButtonCommandProperty); set => SetValue(TopLeftButtonCommandProperty, value); }

	public ICommand? ClosingCommand { get => (ICommand)GetValue(ClosingCommandProperty); set => SetValue(ClosingCommandProperty, value); }
	public object? ClosingCommandParameter { get => GetValue(ClosingCommandParameterProperty); set => SetValue(ClosingCommandParameterProperty, value); }

	public ICommand? ClosedCommand { get => (ICommand)GetValue(ClosedCommandProperty); set => SetValue(ClosedCommandProperty, value); }
	public object? ClosedCommandParameter { get => GetValue(ClosedCommandParameterProperty); set => SetValue(ClosedCommandParameterProperty, value); }

	public ICommand? OpeningCommand { get => (ICommand)GetValue(OpeningCommandProperty); set => SetValue(OpeningCommandProperty, value); }
	public object? OpeningCommandParameter { get => GetValue(OpeningCommandParameterProperty); set => SetValue(OpeningCommandParameterProperty, value); }

	public ICommand? OpenedCommand { get => (ICommand)GetValue(OpenedCommandProperty); set => SetValue(OpenedCommandProperty, value); }
	public object? OpenedCommandParameter { get => GetValue(OpenedCommandParameterProperty); set => SetValue(OpenedCommandParameterProperty, value); }
	#endregion

	#region Event Handlers
	public event EventHandler Closing
	{
		add => eventManager.AddEventHandler(value);
		remove => eventManager.RemoveEventHandler(value);
	}
	public event EventHandler Closed
	{
		add => eventManager.AddEventHandler(value);
		remove => eventManager.RemoveEventHandler(value);
	}
	public event EventHandler Opening
	{
		add => eventManager.AddEventHandler(value);
		remove => eventManager.RemoveEventHandler(value);
	}
	public event EventHandler Opened
	{
		add => eventManager.AddEventHandler(value);
		remove => eventManager.RemoveEventHandler(value);
	}
	#endregion

	#region Events
	private static void OnIsOpenPropertyChanging(BindableObject bindable, object oldValue, object newValue)
		=> ((BottomSheet)bindable).OnIsOpenPropertyChanging((bool)newValue);

	private void OnIsOpenPropertyChanging(bool newValue)
	{
		string eventName = newValue == true ? nameof(Opening) : nameof(Closing);

		eventManager.HandleEvent(this, EventArgs.Empty, eventName);

		ICommand? command = newValue == true ? OpeningCommand : ClosingCommand;
		object? commandParameter = newValue == true ? OpeningCommandParameter : ClosingCommandParameter;

		if (command?.CanExecute(commandParameter) == true)
		{
			command.Execute(commandParameter);
		}
	}

	private static void OnIsOpenPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		=> ((BottomSheet)bindable).OnIsOpenPropertyChanged((bool)newValue);

	private void OnIsOpenPropertyChanged(bool newValue)
	{
		string eventName = newValue == true ? nameof(Opened) : nameof(Closed);

		eventManager.HandleEvent(this, EventArgs.Empty, eventName);

		ICommand? command = newValue == true ? OpenedCommand : ClosedCommand;
		object? commandParameter = newValue == true ? OpenedCommandParameter : ClosedCommandParameter;

		if (command?.CanExecute(commandParameter) == true)
		{
			command.Execute(commandParameter);
		}
	}

	private static void PeekPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		=> ((BottomSheet)bindable).PeekPropertyChanged((BottomSheetPeek)newValue);

	private void PeekPropertyChanged(BottomSheetPeek newValue)
	{
		newValue.BindingContext = this.BindingContext;
		newValue.PropertyChanged += Peek_PropertyChanged;
	}

	private void Peek_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
	{
		if (e.PropertyName == nameof(BottomSheetPeek.PeekHeight)
			|| e.PropertyName == nameof(BottomSheetPeek.IgnoreSafeArea))
		{
			if (Handler is BottomSheetHandler sheetHandler)
			{
				BottomSheetHandler.MapPeek(sheetHandler, this);
			}
		}
	}
	#endregion
}

