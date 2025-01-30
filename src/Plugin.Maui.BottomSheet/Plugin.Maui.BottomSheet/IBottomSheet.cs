namespace Plugin.Maui.BottomSheet;

using System;
using System.Windows.Input;
using Microsoft.Maui;
using Microsoft.Maui.Controls;

/// <summary>
/// Bottom sheets are surfaces containing supplementary content that are anchored to the bottom of the screen.
/// </summary>
public interface IBottomSheet : IView, IPadding, ISafeAreaView
{
    /// <summary>
    /// Event will be invoked when <see cref="IBottomSheet"/> is closing.
    /// </summary>
    event EventHandler Closing;

    /// <summary>
    /// Event will be invoked when <see cref="IBottomSheet"/> is closed.
    /// </summary>
    event EventHandler Closed;

    /// <summary>
    /// Event will be invoked when <see cref="IBottomSheet"/> is opening.
    /// </summary>
    event EventHandler Opening;

    /// <summary>
    /// Event will be invoked when <see cref="IBottomSheet"/> is opened.
    /// </summary>
    event EventHandler Opened;

    /// <summary>
    /// Gets or sets parent.
    /// </summary>
    new Element Parent { get; set; }

    /// <summary>
    /// Gets or sets binding context.
    /// </summary>
    object? BindingContext { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the <see cref="IBottomSheet"/> can be closed with gestures or manually.
    /// </summary>
    bool IsCancelable { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether showing a handle at the top.
    /// </summary>
    bool HasHandle { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether showing the <see cref="BottomSheetHeader"/>.
    /// </summary>
    bool ShowHeader { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the <see cref="IBottomSheet"/>. is open.
    /// </summary>
    bool IsOpen { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the <see cref="IBottomSheet"/>. is modal.
    /// </summary>
    bool IsModal { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether showing the <see cref="IBottomSheet"/>. can be dragged.
    /// </summary>
    /// <remarks>Useful to draw inside the <see cref="IBottomSheet"/>.</remarks>
    bool IsDraggable { get; set; }

    /// <summary>
    /// Gets or sets the Color which will fill the background of an element. This is a bindable property.
    /// </summary>
    Color BackgroundColor { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="BottomSheetHeader"/>.
    /// </summary>
    BottomSheetHeader? Header { get; set; }

    /// <summary>
    /// Gets or sets allowed <see cref="IBottomSheet"/> states.
    /// </summary>
    ICollection<BottomSheetState> States { get; set; }

    /// <summary>
    /// Gets or sets current <see cref="IBottomSheet"/> state.
    /// </summary>
    BottomSheetState CurrentState { get; set; }

    /// <summary>
    /// Gets or sets current corner radius.
    /// </summary>
    float CornerRadius { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="IBottomSheet"/> background color.
    /// </summary>
    Color WindowBackgroundColor { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="BottomSheetPeek"/>.
    /// </summary>
    BottomSheetPeek? Peek { get; set; }

    /// <summary>
    /// Gets or sets the executed command when the <see cref="IBottomSheet"/> is closing.
    /// </summary>
    ICommand? ClosingCommand { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="ClosingCommand"/> parameter.
    /// </summary>
    object? ClosingCommandParameter { get; set; }

    /// <summary>
    /// Gets or sets the executed command when the <see cref="IBottomSheet"/> is closed.
    /// </summary>
    ICommand? ClosedCommand { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="ClosedCommand"/> parameter.
    /// </summary>
    object? ClosedCommandParameter { get; set; }

    /// <summary>
    /// Gets or sets the executed command when the <see cref="IBottomSheet"/> is opening.
    /// </summary>
    ICommand? OpeningCommand { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="OpeningCommand"/> parameter.
    /// </summary>
    object? OpeningCommandParameter { get; set; }

    /// <summary>
    /// Gets or sets the executed command when the <see cref="IBottomSheet"/> is opened.
    /// </summary>
    ICommand? OpenedCommand { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="OpenedCommand"/> parameter.
    /// </summary>
    object? OpenedCommandParameter { get; set; }

    /// <summary>
    /// Gets or sets the content <see cref="DataTemplate"/>.
    /// </summary>
    BottomSheetContent? Content { get; set; }

    /// <summary>
    /// Raise <see cref="Opening"/> event.
    /// </summary>
    internal void OnOpeningBottomSheet();

    /// <summary>
    /// Raise <see cref="Opened"/> event.
    /// </summary>
    internal void OnOpenedBottomSheet();

    /// <summary>
    /// Raise <see cref="Closing"/> event.
    /// </summary>
    internal void OnClosingBottomSheet();

    /// <summary>
    /// Raise <see cref="Closed"/> event.
    /// </summary>
    internal void OnClosedBottomSheet();
}