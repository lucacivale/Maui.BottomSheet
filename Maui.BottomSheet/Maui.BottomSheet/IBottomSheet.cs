using System.Windows.Input;

namespace Maui.BottomSheet;

public interface IBottomSheet : IView, IBindable
{
	#region Appearance
	bool HasHandle { get; set; }
	bool ShowHeader { get; set; }
	bool IsOpen { get; set; }
	bool IsDraggable { get; set; }
	BottomSheetHeaderAppearanceMode HeaderAppearance { get; set; }
	string TopLeftButtonText { get; set; }
	string TopRightButtonText { get; set; }
	string TitleText { get; set; }
	BottomSheetState SheetStates { get; set; }
	BottomSheetState SelectedSheetState { get; set; }
	#endregion

	#region Commands
	ICommand? TopRightButtonCommand { get; set; }
	object? TopRightButtonCommandParameter { get; set; }

	ICommand? TopLeftButtonCommand { get; set; }
	object? TopLeftButtonCommandParameter { get; set; }

	ICommand? ClosingCommand { get; set; }
	object? ClosingCommandParameter { get; set; }

	ICommand? ClosedCommand { get; set; }
	object? ClosedCommandParameter { get; set; }

	ICommand? OpeningCommand { get; set; }
	object? OpeningCommandParameter { get; set; }

	ICommand? OpenedCommand { get; set; }
	object? OpenedCommandParameter { get; set; }
	#endregion

	#region Event Handler
	event EventHandler Closing;
	event EventHandler Closed;
	event EventHandler Opening;
	event EventHandler Opened;
	#endregion

	#region DataTemplates
	DataTemplate? TitleViewTemplate { get; set; }
	DataTemplate? ContentTemplate { get; set; }
	#endregion

	#region Peek
	BottomSheetPeek? Peek { get; set; }
	#endregion
}

