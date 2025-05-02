using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Android.Content;
using Plugin.Maui.BottomSheet.Navigation;

namespace Plugin.Maui.BottomSheet.Platform.Android;

/// <inheritdoc />
internal sealed class BottomSheetDialog : Google.Android.Material.BottomSheet.BottomSheetDialog
{
    private readonly BottomSheetCallback _bottomSheetCallback;

    private IBottomSheet? _virtualBottomSheet;
    private int _lastOpenState;

    /// <summary>
    /// Initializes a new instance of the <see cref="BottomSheetDialog"/> class.
    /// </summary>
    /// <param name="context"><see cref="Context"/>.</param>
    /// <param name="theme">Theme Id.</param>
    /// <param name="virtualBottomSheet"><see cref="IBottomSheet"/>.</param>
    public BottomSheetDialog(
        Context context,
        int theme,
        IBottomSheet virtualBottomSheet,
        BottomSheetCallback bottomSheetCallback)
        : base(context, theme)
    {
        _virtualBottomSheet = virtualBottomSheet;

        _bottomSheetCallback = bottomSheetCallback;
        _bottomSheetCallback.StateChanged += BottomSheetCallbackOnStateChanged;
        
        Behavior.AddBottomSheetCallback(_bottomSheetCallback);
    }

    /// <inheritdoc />
    public override void Show()
    {
        base.Show();

        _lastOpenState = Behavior.State;
    }

    /// <inheritdoc />
    [SuppressMessage("Design", "CA1031: Do not catch general exception types", Justification = "Catch all exceptions to prevent crash.")]
    public async override void Cancel()
    {
        try
        {
            if (_virtualBottomSheet?.ShouldConfirmNavigation() == true
                && await ConfirmNavigationAsync().ConfigureAwait(true) == false)
            {
                Behavior.State = _lastOpenState;
                return;
            }

            _bottomSheetCallback.StateChanged -= BottomSheetCallbackOnStateChanged;
            Behavior.RemoveBottomSheetCallback(_bottomSheetCallback);
            base.Cancel();
        }
        catch
        {
            Trace.TraceError("BottomSheetDialog cancelled failed.");
        }
    }

    /// <inheritdoc />
    public override void Dismiss()
    {
        _bottomSheetCallback.StateChanged -= BottomSheetCallbackOnStateChanged;
        Behavior.RemoveBottomSheetCallback(_bottomSheetCallback);

        base.Dismiss();
    }

    /// <summary>
    /// Determines whether <see cref="BottomSheetDialog"/> accepts being navigated away from.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public Task<bool> ConfirmNavigationAsync()
    {
        return _virtualBottomSheet is null ? Task.FromResult(false) : MvvmHelpers.ConfirmNavigationAsync(_virtualBottomSheet, BottomSheetNavigationParameters.Empty());
    }

    /// <inheritdoc />
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _virtualBottomSheet = null;
        }

        base.Dispose(disposing);
    }
    
    private void BottomSheetCallbackOnStateChanged(object? sender, BottomSheetStateChangedEventArgs e)
    {
        _lastOpenState = e.State.ToPlatformState();
    }
}