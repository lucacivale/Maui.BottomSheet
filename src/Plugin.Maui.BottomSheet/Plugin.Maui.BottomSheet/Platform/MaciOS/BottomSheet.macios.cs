namespace Plugin.Maui.BottomSheet.Platform.MaciOS;

using UIKit;

/// <summary>
/// BottomSheet container view including Handle, Header and Content.
/// </summary>
internal sealed class BottomSheet : IDisposable
{
    private readonly BottomSheetUIViewController _bottomSheetUIViewController;
    private readonly WeakEventManager _eventManager = new();

    private BottomSheetContent? _bottomSheetContent;

    /// <summary>
    /// Initializes a new instance of the <see cref="BottomSheet"/> class.
    /// </summary>
    /// <param name="mauiContext">Context.</param>
    public BottomSheet(IMauiContext mauiContext)
    {
        _bottomSheetUIViewController = new BottomSheetUIViewController(mauiContext);
        _bottomSheetUIViewController.Dismissed += BottomSheetUIViewControllerOnDismissed;
        _bottomSheetUIViewController.StateChanged += BottomSheetUIViewControllerOnStateChanged;
    }

    /// <summary>
    /// Finalizes an instance of the <see cref="BottomSheet"/> class.
    /// </summary>
    ~BottomSheet()
    {
        Dispose(false);
    }

    /// <summary>
    /// BottomSheet state changed.
    /// </summary>
    public event EventHandler<BottomSheetStateChangedEventArgs> StateChanged
    {
        add => _eventManager.AddEventHandler(value);
        remove => _eventManager.RemoveEventHandler(value);
    }

    /// <summary>
    /// BottomSheet dismissed.
    /// </summary>
    public event EventHandler Dismissed
    {
        add => _eventManager.AddEventHandler(value);
        remove => _eventManager.RemoveEventHandler(value);
    }

    /// <summary>
    /// Gets a value indicating whether the sheet is being presented.
    /// </summary>
    public bool IsOpen { get; private set; }

    /// <summary>
    /// Open the bottom sheet.
    /// </summary>
    /// <param name="bottomSheet">Virtual view.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
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

        SetPadding(bottomSheet.Padding);
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
    /// Show header.
    /// </summary>
    public void ShowHeader()
    {
        _bottomSheetUIViewController.ShowHeader();
    }

    /// <summary>
    /// Hide header.
    /// </summary>
    public void HideHeader()
    {
        _bottomSheetUIViewController.HideHeader();
    }

    /// <summary>
    /// Close the bottom sheet.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task CloseAsync()
    {
        await _bottomSheetUIViewController.CloseAsync().ConfigureAwait(true);

        IsOpen = false;
    }

    /// <summary>
    /// Set the <see cref="Plugin.Maui.BottomSheet.BottomSheetHeader"/>.
    /// </summary>
    /// <param name="header">Header.</param>
    /// <param name="style">Style.</param>
    public void SetHeader(Plugin.Maui.BottomSheet.BottomSheetHeader header, BottomSheetHeaderStyle style)
    {
        _bottomSheetUIViewController.AddHeader(header, style);
    }

    /// <summary>
    /// Set header style.
    /// </summary>
    /// <param name="style">Style.</param>
    public void SetHeaderStyle(BottomSheetHeaderStyle style)
    {
        _bottomSheetUIViewController.SetHeaderStyle(style);
    }

    /// <summary>
    /// Set peek height.
    /// </summary>
    /// <param name="peekHeight">Peek height.</param>
    public void SetPeekHeight(double peekHeight)
    {
        _bottomSheetUIViewController.SetPeekHeight(peekHeight);
    }

    /// <summary>
    /// Set the <see cref="Plugin.Maui.BottomSheet.BottomSheetContent"/>.
    /// </summary>
    /// <param name="content">Content.</param>
    public void SetContent(BottomSheetContent? content)
    {
        _bottomSheetContent = content;
    }

    /// <summary>
    /// Sets whether this dialog is cancelable.
    /// </summary>
    /// <param name="isCancelable">Whether the dialog should be canceled when touched outside the window or by slide.</param>
    public void SetIsCancelable(bool isCancelable)
    {
        _bottomSheetUIViewController.ModalInPresentation = !isCancelable;
    }

    /// <summary>
    /// Set modal.
    /// </summary>
    /// <param name="isModal">Is modal.</param>
    public void SetIsModal(bool isModal)
    {
        _bottomSheetUIViewController.IsModal = isModal;
    }

    /// <summary>
    /// Sets whether this dialog shows a handle.
    /// </summary>
    /// <param name="hasHandle">Whether the dialog should show a handle.</param>
    public void SetHasHandle(bool hasHandle)
    {
        _bottomSheetUIViewController.SetPrefersGrabberVisible(hasHandle);
    }

    /// <summary>
    /// Sets corner radius.
    /// </summary>
    /// <param name="cornerRadius">Corner radius.</param>
    public void SetCornerRadius(float cornerRadius)
    {
        _bottomSheetUIViewController.SetCornerRadius(cornerRadius);
    }

    /// <summary>
    /// Sets window background color.
    /// </summary>
    /// <param name="color">Color.</param>
    public void SetWindowBackgroundColor(Color color)
    {
        _bottomSheetUIViewController.SetWindowBackgroundColor(color);
    }

    /// <summary>
    /// Set current state.
    /// </summary>
    /// <param name="state">State to apply.</param>
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
    /// Set allowed bottom sheet states.
    /// </summary>
    /// <param name="states">Allowed states.</param>
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
    /// Sets whether this dialog is draggable.
    /// </summary>
    /// <param name="isDraggable">Whether the dialog should is draggable.</param>
    public void SetIsDraggable(bool isDraggable)
    {
        _bottomSheetUIViewController.Draggable = isDraggable;
    }

    /// <summary>
    /// Sets the background color.
    /// </summary>
    /// <param name="color">Color.</param>
    public void SetBackgroundColor(Color color)
    {
        _bottomSheetUIViewController.SetBackgroundColor(color);
    }

    /// <summary>
    /// Sets padding.
    /// </summary>
    /// <param name="padding">Thickness.</param>
    public void SetPadding(Thickness padding)
    {
        _bottomSheetUIViewController.SetPadding(padding);
    }

    /// <summary>
    /// Set ignore safe area.
    /// </summary>
    /// <param name="ignoreSafeArea">Ignore safe area.</param>
    public void SetIgnoreSafeArea(bool ignoreSafeArea)
    {
        _bottomSheetUIViewController.SetIgnoreSafeArea(ignoreSafeArea);
    }

    /// <summary>
    /// Dispose <see cref="BottomSheet"/>.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
    {
        if (!disposing)
        {
            return;
        }

        _bottomSheetUIViewController.Dismissed -= BottomSheetUIViewControllerOnDismissed;
        _bottomSheetUIViewController.StateChanged -= BottomSheetUIViewControllerOnStateChanged;
        _bottomSheetUIViewController.Dispose();
    }

    private void BottomSheetUIViewControllerOnStateChanged(object? sender, BottomSheetStateChangedEventArgs e)
    {
        _eventManager.HandleEvent(
            this,
            e,
            nameof(StateChanged));
    }

    private void BottomSheetUIViewControllerOnDismissed(object? sender, EventArgs e)
    {
        _eventManager.HandleEvent(
            this,
            e,
            nameof(Dismissed));
    }
}