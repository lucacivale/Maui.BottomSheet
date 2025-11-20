using System.ComponentModel;
using System.Windows.Input;
using Plugin.BottomSheet;

namespace Plugin.Maui.BottomSheet;

/// <summary>
/// Represents a bottom sheet interface providing supplementary content anchored to the bottom of the screen.
/// </summary>
public interface IBottomSheet : IView, IPadding, ISafeAreaView
{
    /// <summary>
    /// Occurs when the bottom sheet is closing.
    /// </summary>
    event EventHandler Closing;

    /// <summary>
    /// Occurs when the bottom sheet has been closed.
    /// </summary>
    event EventHandler Closed;

    /// <summary>
    /// Occurs when the bottom sheet is opening.
    /// </summary>
    event EventHandler Opening;

    /// <summary>
    /// Occurs when the bottom sheet has been opened.
    /// </summary>
    event EventHandler Opened;

    /// <summary>
    /// Occurs when the bottom sheet has been opened.
    /// </summary>
    event EventHandler LayoutChanged;

    /// <summary>
    /// Gets or sets the parent element of the bottom sheet.
    /// </summary>
    new Element Parent { get; set; }

    /// <summary>
    /// Gets or sets the binding context for data binding operations.
    /// </summary>
    object? BindingContext { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the bottom sheet can be dismissed by user gestures or interactions.
    /// </summary>
    bool IsCancelable { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether a drag handle is displayed at the top of the bottom sheet.
    /// </summary>
    bool HasHandle { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the header section is displayed.
    /// </summary>
    bool ShowHeader { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the bottom sheet is currently open.
    /// </summary>
    bool IsOpen { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the bottom sheet is presented modally.
    /// </summary>
    bool IsModal { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the bottom sheet can be dragged by the user.
    /// </summary>
    /// <remarks>Useful for controlling interaction when drawing inside the bottom sheet.</remarks>
    bool IsDraggable { get; set; }

    /// <summary>
    /// Gets or sets the background color of the bottom sheet.
    /// </summary>
    Color BackgroundColor { get; set; }

    /// <summary>
    /// Gets or sets the header configuration for the bottom sheet.
    /// </summary>
    BottomSheetHeader? Header { get; set; }

    /// <summary>
    /// Gets or sets the collection of allowed states for the bottom sheet.
    /// </summary>
    ICollection<BottomSheetState> States { get; set; }

    /// <summary>
    /// Gets or sets the current state of the bottom sheet.
    /// </summary>
    BottomSheetState CurrentState { get; set; }

    /// <summary>
    /// Gets or sets the corner radius of the bottom sheet.
    /// </summary>
    float CornerRadius { get; set; }

    /// <summary>
    /// Gets or sets the background color of the window behind the bottom sheet.
    /// </summary>
    Color WindowBackgroundColor { get; set; }

    /// <summary>
    /// Gets or sets the height of the bottom sheet when in peek state.
    /// </summary>
    double PeekHeight { get; set; }

    /// <summary>
    /// Gets or sets the style configuration for built-in components.
    /// </summary>
    public BottomSheetStyle BottomSheetStyle { get; set; }

    /// <summary>
    /// Gets or sets the command executed when the bottom sheet is closing.
    /// </summary>
    ICommand? ClosingCommand { get; set; }

    /// <summary>
    /// Gets or sets the parameter passed to the closing command.
    /// </summary>
    object? ClosingCommandParameter { get; set; }

    /// <summary>
    /// Gets or sets the command executed when the bottom sheet has been closed.
    /// </summary>
    ICommand? ClosedCommand { get; set; }

    /// <summary>
    /// Gets or sets the parameter passed to the closed command.
    /// </summary>
    object? ClosedCommandParameter { get; set; }

    /// <summary>
    /// Gets or sets the command executed when the bottom sheet is opening.
    /// </summary>
    ICommand? OpeningCommand { get; set; }

    /// <summary>
    /// Gets or sets the parameter passed to the opening command.
    /// </summary>
    object? OpeningCommandParameter { get; set; }

    /// <summary>
    /// Gets or sets the command executed when the bottom sheet has been opened.
    /// </summary>
    ICommand? OpenedCommand { get; set; }

    /// <summary>
    /// Gets or sets the parameter passed to the opened command.
    /// </summary>
    object? OpenedCommandParameter { get; set; }

    /// <summary>
    /// Gets or sets the content configuration for the bottom sheet.
    /// </summary>
    BottomSheetContent? Content { get; set; }

    Grid ContainerView { get; }

    /// <summary>
    /// Triggers the opening event for the bottom sheet.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    void OnOpeningBottomSheet();

    /// <summary>
    /// Triggers the opened event for the bottom sheet.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    void OnOpenedBottomSheet();

    /// <summary>
    /// Triggers the closing event for the bottom sheet.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    void OnClosingBottomSheet();

    /// <summary>
    /// Triggers the closed event for the bottom sheet.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    void OnClosedBottomSheet();

    /// <summary>
    /// Triggers the closed event for the bottom sheet.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    void OnLayoutChanged();

    [EditorBrowsable(EditorBrowsableState.Never)]
    void Cancel();
}