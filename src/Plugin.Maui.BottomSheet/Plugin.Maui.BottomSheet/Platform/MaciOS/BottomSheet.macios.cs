using System.Diagnostics.CodeAnalysis;

namespace Plugin.Maui.BottomSheet.Platform.MaciOS;

using Plugin.BottomSheet;
using UIKit;

/// <summary>
/// Container for bottom sheet functionality including handle, header and content on macOS and iOS platforms.
/// </summary>
internal sealed class BottomSheet : IAsyncDisposable, IDisposable
{
    private readonly BottomSheetUIViewController _bottomSheetUIViewController;
    private readonly WeakEventManager _eventManager = new();

    private BottomSheetContent? _bottomSheetContent;

    /// <summary>
    /// Initializes a new instance of the <see cref="BottomSheet"/> class.
    /// </summary>
    /// <param name="mauiContext">The MAUI context for platform services.</param>
    public BottomSheet(IMauiContext mauiContext)
    {
        _bottomSheetUIViewController = new BottomSheetUIViewController(mauiContext);
        _bottomSheetUIViewController.StateChanged += BottomSheetUIViewControllerOnStateChanged;
        _bottomSheetUIViewController.ConfirmDismiss += BottomSheetUIViewControllerOnConfirmDismiss;
        _bottomSheetUIViewController.FrameChanged += BottomSheetUIViewControllerOnFrameChanged;
    }

    /// <summary>
    /// Finalizes an instance of the <see cref="BottomSheet"/> class.
    /// </summary>
    ~BottomSheet()
    {
        Dispose(false);
    }

    /// <summary>
    /// Occurs when the bottom sheet state changes.
    /// </summary>
    public event EventHandler<BottomSheetStateChangedEventArgs> StateChanged
    {
        add => _eventManager.AddEventHandler(value);
        remove => _eventManager.RemoveEventHandler(value);
    }

    /// <summary>
    /// Occurs when the bottom sheet dismissal needs confirmation.
    /// </summary>
    public event EventHandler ConfirmDismiss
    {
        add => _eventManager.AddEventHandler(value);
        remove => _eventManager.RemoveEventHandler(value);
    }

    /// <summary>
    /// Occurs when the bottom sheet frame changes.
    /// </summary>
    public event EventHandler<Rect> FrameChanged
    {
        add => _eventManager.AddEventHandler(value);
        remove => _eventManager.RemoveEventHandler(value);
    }

    /// <summary>
    /// Gets a value indicating whether the bottom sheet is currently being presented.
    /// </summary>
    public bool IsOpen { get; private set; }

    /// <summary>
    /// Opens the bottom sheet with the specified configuration.
    /// </summary>
    /// <param name="bottomSheet">The virtual view containing bottom sheet configuration.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task OpenAsync(IBottomSheet bottomSheet)
    {
        if (bottomSheet.Header is not null)
        {
            SetHeader(bottomSheet.Header, bottomSheet.BottomSheetStyle.HeaderStyle);
        }

        SetContent(bottomSheet.Content);

        if (bottomSheet.ShowHeader)
        {
            ShowHeader();
        }

        //SetPadding(bottomSheet.Padding);
        SetIsCancelable(bottomSheet.IsCancelable);
        SetState(bottomSheet.CurrentState);
        SetStates(bottomSheet.States);
        SetHasHandle(bottomSheet.HasHandle);
        SetIgnoreSafeArea(bottomSheet.IgnoreSafeArea);
        SetCornerRadius(bottomSheet.CornerRadius);
        SetBackgroundColor(bottomSheet.BackgroundColor);
        SetIsModal(bottomSheet.IsModal);
        SetWindowBackgroundColor(bottomSheet.WindowBackgroundColor);
        SetHeaderStyle(bottomSheet.BottomSheetStyle.HeaderStyle);
        SetPeekHeight(bottomSheet.PeekHeight);

        _bottomSheetUIViewController.AddContent(_bottomSheetContent);
        _bottomSheetUIViewController.AttachPresentationDelegate();

        IsOpen = await _bottomSheetUIViewController.OpenAsync(bottomSheet).ConfigureAwait(true);

        SetIsDraggable(bottomSheet.IsDraggable);
    }

    /// <summary>
    /// Shows the header section of the bottom sheet.
    /// </summary>
    public void ShowHeader()
    {
        _bottomSheetUIViewController.ShowHeader();
    }

    /// <summary>
    /// Hides the header section of the bottom sheet.
    /// </summary>
    public void HideHeader()
    {
        _bottomSheetUIViewController.HideHeader();
    }

    /// <summary>
    /// Closes the bottom sheet asynchronously.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task CloseAsync()
    {
        await _bottomSheetUIViewController.CloseAsync().ConfigureAwait(true);

        IsOpen = false;
    }

    /// <summary>
    /// Sets the header configuration and style for the bottom sheet.
    /// </summary>
    /// <param name="header">The header configuration.</param>
    /// <param name="style">The header style settings.</param>
    public void SetHeader(Plugin.Maui.BottomSheet.BottomSheetHeader header, BottomSheetHeaderStyle style)
    {
        _bottomSheetUIViewController.AddHeader(header, style);
    }

    /// <summary>
    /// Sets the header style for the bottom sheet.
    /// </summary>
    /// <param name="style">The header style settings.</param>
    public void SetHeaderStyle(BottomSheetHeaderStyle style)
    {
        _bottomSheetUIViewController.SetHeaderStyle(style);
    }

    /// <summary>
    /// Sets the peek height for the bottom sheet when in peek state.
    /// </summary>
    /// <param name="peekHeight">The peek height value.</param>
    public void SetPeekHeight(double peekHeight)
    {
        _bottomSheetUIViewController.SetPeekHeight(peekHeight);
    }

    /// <summary>
    /// Sets the content for the bottom sheet.
    /// </summary>
    /// <param name="content">The content to display in the bottom sheet.</param>
    public void SetContent(BottomSheetContent? content)
    {
        _bottomSheetContent = content;
    }

    /// <summary>
    /// Sets whether the bottom sheet can be canceled by user interaction.
    /// </summary>
    /// <param name="isCancelable">True if the bottom sheet should be cancelable, false otherwise.</param>
    public void SetIsCancelable(bool isCancelable)
    {
        _bottomSheetUIViewController.ModalInPresentation = !isCancelable;
    }

    /// <summary>
    /// Sets whether the bottom sheet should be presented modally.
    /// </summary>
    /// <param name="isModal">True if the bottom sheet should be modal, false otherwise.</param>
    public void SetIsModal(bool isModal)
    {
        _bottomSheetUIViewController.IsModal = isModal;
    }

    /// <summary>
    /// Sets whether the bottom sheet should display a drag handle.
    /// </summary>
    /// <param name="hasHandle">True if the bottom sheet should show a handle, false otherwise.</param>
    public void SetHasHandle(bool hasHandle)
    {
        _bottomSheetUIViewController.SetPrefersGrabberVisible(hasHandle);
    }

    /// <summary>
    /// Sets the corner radius for the bottom sheet.
    /// </summary>
    /// <param name="cornerRadius">The corner radius value.</param>
    public void SetCornerRadius(float cornerRadius)
    {
        _bottomSheetUIViewController.SetCornerRadius(cornerRadius);
    }

    /// <summary>
    /// Sets the window background color for the bottom sheet.
    /// </summary>
    /// <param name="color">The background color to apply.</param>
    public void SetWindowBackgroundColor(Color color)
    {
        _bottomSheetUIViewController.SetWindowBackgroundColor(color);
    }

    /// <summary>
    /// Sets the current state of the bottom sheet.
    /// </summary>
    /// <param name="state">The state to apply to the bottom sheet.</param>
    public void SetState(BottomSheetState state)
    {
        switch (state)
        {
            case BottomSheetState.Peek:
            {
                if (OperatingSystem.IsMacCatalyst()
                    || (OperatingSystem.IsIOS()
                        && OperatingSystem.IsIOSVersionAtLeast(16)))
                {
#pragma warning disable CA1416
                    var origDetents = new List<UISheetPresentationControllerDetent>(_bottomSheetUIViewController.Detents);
                    var detents = _bottomSheetUIViewController.Detents.ToList();
                    if (detents.Count > 0
                        && detents.Exists(x => x.Identifier == BottomSheetUIViewController.PeekDetentId))
                    {
                        detents.RemoveAll(x => x.Identifier != BottomSheetUIViewController.PeekDetentId);

                        _bottomSheetUIViewController.Detents = detents.ToArray();
                        _bottomSheetUIViewController.Detents = origDetents.ToArray();
                        _bottomSheetUIViewController.SetSelectedDetentIdentifier(UISheetPresentationControllerDetentIdentifier.Unknown);
                    }
#pragma warning restore CA1416
                }
                else
                {
                    _bottomSheetUIViewController.SetSelectedDetentIdentifier(UISheetPresentationControllerDetentIdentifier.Medium);
                }

                break;
            }

            case BottomSheetState.Medium:
                _bottomSheetUIViewController.SetSelectedDetentIdentifier(UISheetPresentationControllerDetentIdentifier.Medium);
                break;
            case BottomSheetState.Large:
                _bottomSheetUIViewController.SetSelectedDetentIdentifier(UISheetPresentationControllerDetentIdentifier.Large);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }
    }

    /// <summary>
    /// Sets the available states for the bottom sheet.
    /// </summary>
    /// <param name="states">The collection of allowed states.</param>
    public void SetStates(IEnumerable<BottomSheetState> states)
    {
        List<UISheetPresentationControllerDetent> detents = [];

        foreach (var state in states)
        {
            switch (state)
            {
                case BottomSheetState.Peek:
                    detents.Add(_bottomSheetUIViewController.PeekDetent);
                    break;
                case BottomSheetState.Medium:
                    detents.Add(_bottomSheetUIViewController.MediumDetent);
                    break;
                case BottomSheetState.Large:
                    detents.Add(_bottomSheetUIViewController.LargeDetent);
                    break;
            }
        }

        _bottomSheetUIViewController.Detents = detents.ToArray();
    }

    /// <summary>
    /// Sets whether the bottom sheet is draggable.
    /// </summary>
    /// <param name="isDraggable">True if the bottom sheet should be draggable, false otherwise.</param>
    public void SetIsDraggable(bool isDraggable)
    {
        _bottomSheetUIViewController.Draggable = isDraggable;
    }

    /// <summary>
    /// Sets the background color for the bottom sheet.
    /// </summary>
    /// <param name="color">The background color to apply.</param>
    public void SetBackgroundColor(Color color)
    {
        _bottomSheetUIViewController.SetBackgroundColor(color);
    }

    /// <summary>
    /// Sets the padding for the bottom sheet content.
    /// </summary>
    /// <param name="padding">The padding thickness to apply.</param>
    public void SetPadding(Thickness padding)
    {
        //_bottomSheetUIViewController.SetPadding(padding);
    }

    /// <summary>
    /// Sets whether the bottom sheet should ignore safe area constraints.
    /// </summary>
    /// <param name="ignoreSafeArea">True if safe area should be ignored, false otherwise.</param>
    public void SetIgnoreSafeArea(bool ignoreSafeArea)
    {
        _bottomSheetUIViewController.SetIgnoreSafeArea(ignoreSafeArea);
    }

    /// <summary>
    /// Releases all resources used by the bottom sheet.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Releases all resources used by the bottom sheet asynchronous.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    [SuppressMessage("Usage", "CA1816: Dispose methods should call SuppressFinalize", Justification = "This is handled by Dispose.")]
    public async ValueTask DisposeAsync()
    {
        if (IsOpen)
        {
            await CloseAsync().ConfigureAwait(true);
            _eventManager.HandleEvent(
                this,
                EventArgs.Empty,
                nameof(ConfirmDismiss));
        }

        Dispose();
    }

    /// <summary>
    /// Releases managed and unmanaged resources.
    /// </summary>
    /// <param name="disposing">True if disposing managed resources.</param>
    private void Dispose(bool disposing)
    {
        if (!disposing)
        {
            return;
        }

        _bottomSheetUIViewController.StateChanged -= BottomSheetUIViewControllerOnStateChanged;
        _bottomSheetUIViewController.ConfirmDismiss -= BottomSheetUIViewControllerOnConfirmDismiss;
        _bottomSheetUIViewController.FrameChanged -= BottomSheetUIViewControllerOnFrameChanged;
        _bottomSheetUIViewController.Dispose();
    }

    /// <summary>
    /// Handles state change events from the UI view controller and forwards them.
    /// </summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="e">The state change event arguments.</param>
    private void BottomSheetUIViewControllerOnStateChanged(object? sender, BottomSheetStateChangedEventArgs e)
    {
        _eventManager.HandleEvent(
            this,
            e,
            nameof(StateChanged));
    }

    /// <summary>
    /// Handles confirm dismiss events from the UI view controller and forwards them.
    /// </summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="e">The event arguments.</param>
    private void BottomSheetUIViewControllerOnConfirmDismiss(object? sender, EventArgs e)
    {
        _eventManager.HandleEvent(
            this,
            e,
            nameof(ConfirmDismiss));
    }

    /// <summary>
    /// Handles frame change events from the UI view controller and forwards them.
    /// </summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="e">The frame change event arguments.</param>
    private void BottomSheetUIViewControllerOnFrameChanged(object? sender, Rect e)
    {
        _eventManager.HandleEvent(
            this,
            e,
            nameof(FrameChanged));
    }
}