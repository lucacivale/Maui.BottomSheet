using System.ComponentModel;
using System.Windows.Input;
using Plugin.BottomSheet;

namespace Plugin.Maui.BottomSheet;

/// <summary>
/// Defines the contract for a bottom sheet component that displays additional content or actions
/// originating from the bottom of the user interface.
/// </summary>
public interface IBottomSheet : IView, IPadding
{
    /// <summary>
    /// Triggered when the bottom sheet begins the process of closing, providing an opportunity to react or cancel the action.
    /// </summary>
    event EventHandler Closing;

    /// <summary>
    /// Occurs when the bottom sheet has finished closing.
    /// </summary>
    event EventHandler Closed;

    /// <summary>
    /// Occurs when the bottom sheet is opening.
    /// </summary>
    event EventHandler Opening;

    /// <summary>
    /// Indicates the event or state where the bottom sheet is fully opened, allowing interaction or further processing.
    /// </summary>
    event EventHandler Opened;

    /// <summary>
    /// Occurs when the layout of the bottom sheet has changed.
    /// </summary>
    event EventHandler LayoutChanged;

    /// <summary>
    /// Triggered whenever the state of the bottom sheet changes, allowing subscribers to track transitions between different states.
    /// </summary>
    event EventHandler<BottomSheetStateChangedEventArgs> StateChanged;

    /// <summary>
    /// Gets or sets the parent element or object in a hierarchy, providing access to and context within its logical container or structure.
    /// </summary>
    new Element Parent { get; set; }

    /// <summary>
    /// Gets or sets the data source or context used for data binding in the associated view or control, enabling dynamic updates and interactions based on the underlying data model.
    /// </summary>
    object? BindingContext { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether an operation or action can be canceled, allowing conditional logic to handle cancellation.
    /// </summary>
    bool IsCancelable { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the object has an associated handle, typically used to determine if it can be manipulated or interacted with in a specific manner.
    /// </summary>
    bool HasHandle { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the header section is displayed, allowing customization of the appearance based on the value.
    /// </summary>
    bool ShowHeader { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the bottom sheet is currently open or closed.
    /// </summary>
    bool IsOpen { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the component is displayed as a modal, allowing interaction with it while disabling interaction with other elements.
    /// </summary>
    bool IsModal { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the element can be dragged by the user, allowing for interaction through drag-and-drop functionality.
    /// </summary>
    bool IsDraggable { get; set; }

    /// <summary>
    /// Gets or sets the background color of a user interface element, allowing customization of its visual appearance.
    /// </summary>
    Color BackgroundColor { get; set; }

    /// <summary>
    /// Gets or sets the header section of a component, typically used to display a title or introductory content.
    /// </summary>
    BottomSheetHeader? Header { get; set; }

    /// <summary>
    /// Gets or sets the different possible states the bottom sheet can be in, allowing for state-specific logic or behavior to be implemented.
    /// </summary>
    ICollection<BottomSheetState> States { get; set; }

    /// <summary>
    /// Gets or sets the current operational state, providing information about the current status or mode of the system.
    /// </summary>
    BottomSheetState CurrentState { get; set; }

    /// <summary>
    /// Gets or sets the radius of the corners for a UI element, allowing customization of the element's rounded edges.
    /// </summary>
    float CornerRadius { get; set; }

    /// <summary>
    /// Gets or sets the background color of the window, allowing customization of the visual appearance of the window's backdrop.
    /// </summary>
    Color WindowBackgroundColor { get; set; }

    /// <summary>
    /// Gets or sets the initial height of the bottom sheet when it is partially expanded, controlling how much content is visible.
    /// </summary>
    double PeekHeight { get; set; }

    /// <summary>
    /// Gets or sets the visual configuration and styling options for the bottom sheet, allowing customization of appearance and behavior.
    /// </summary>
    BottomSheetStyle BottomSheetStyle { get; set; }

    /// <summary>
    /// Gets or sets the command when a request to close the bottom sheet is initiated, allowing for custom logic or command execution.
    /// </summary>
    ICommand? ClosingCommand { get; set; }

    /// <summary>
    /// Gets or sets a parameter to be passed to the command executed when the closing action is initiated.
    /// </summary>
    object? ClosingCommandParameter { get; set; }

    /// <summary>
    /// Gets or sets a command executed when a closing action has been completed successfully.
    /// </summary>
    ICommand? ClosedCommand { get; set; }

    /// <summary>
    /// Gets or sets the parameter to be passed to the command associated with the closing action, allowing additional context or data to be provided when the closing event is triggered.
    /// </summary>
    object? ClosedCommandParameter { get; set; }

    /// <summary>
    /// Gets or sets the command when the bottom sheet starts opening, allowing for handling or intercepting the action.
    /// </summary>
    ICommand? OpeningCommand { get; set; }

    /// <summary>
    /// Gets or sets a parameter passed to the command executed when initiating the opening action, allowing additional context or data to be provided.
    /// </summary>
    object? OpeningCommandParameter { get; set; }

    /// <summary>
    /// Gets or sets when the bottom sheet has been successfully opened, enabling the execution of custom logic or actions in response.
    /// </summary>
    ICommand? OpenedCommand { get; set; }

    /// <summary>
    /// Gets or sets the parameter to be passed to the command executed when the associated item or view is opened, enabling contextual processing or logic.
    /// </summary>
    object? OpenedCommandParameter { get; set; }

    /// <summary>
    /// Gets or sets the main content to be displayed, allowing customization or dynamic updates as needed.
    /// </summary>
    BottomSheetContent? Content { get; set; }

    /// <summary>
    /// Gets the primary visual element that encapsulates and organizes child elements within a structured layout.
    /// </summary>
    Grid ContainerView { get; }

    /// <summary>
    /// Gets or sets the size behavior of the bottom sheet, allowing customization of its mode such as fitting to content
    /// or using predefined states.
    /// </summary>
    BottomSheetSizeMode SizeMode { get; set; }

    /// <summary>
    /// Invoked when the bottom sheet begins the opening process.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    void OnOpeningBottomSheet();

    /// <summary>
    /// Invoked after the bottom sheet has been fully opened.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    void OnOpenedBottomSheet();

    /// <summary>
    /// Triggers the closing event for the bottom sheet.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    void OnClosingBottomSheet();

    /// <summary>
    /// Invoked when the bottom sheet is closed.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    void OnClosedBottomSheet();

    /// <summary>
    /// Invoked when the layout of a component changes.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    void OnLayoutChanged();

    /// <summary>
    /// Cancels the current operation or ongoing process.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    void Cancel();
}