namespace Plugin.Maui.BottomSheet.Behaviors.BottomSheetPeekBehavior;

using Microsoft.Maui.Platform;

/// <summary>
/// Represents a behavior that provides support for handling the peek functionality in a bottom sheet.
/// </summary>
/// <remarks>
/// This class interacts with Android platform-specific components to manage the layout and state of a bottom sheet
/// when it is attached to a view. It ensures the proper handling of layout changes and maintains references to
/// the associated bottom sheet.
/// </remarks>
public sealed partial class BottomSheetPeekBehavior
{
    private WeakReference<IBottomSheet>? _weakBottomSheet;

    /// <summary>
    /// Called when the behavior is attached to a view.
    /// </summary>
    /// <param name="bindable">The view to which the behavior is attached.</param>
    /// <param name="platformView">The platform-specific view representation associated with the bindable view.</param>
    protected override void OnAttachedTo(View bindable, Android.Views.View platformView)
    {
        base.OnAttachedTo(bindable, platformView);

        var bottomSheet = bindable.FindBottomSheet();

        if (bottomSheet is not null)
        {
            _weakBottomSheet = new WeakReference<IBottomSheet>(bottomSheet);
        }

        platformView.LayoutChange += OnLayoutChange;
    }

    /// <summary>
    /// Called when the behavior is detached from a view.
    /// </summary>
    /// <param name="bindable">The view from which the behavior is being detached.</param>
    /// <param name="platformView">The platform-specific view representation associated with the bindable view.</param>
    protected override void OnDetachedFrom(View bindable, Android.Views.View platformView)
    {
        base.OnDetachedFrom(bindable, platformView);

        platformView.LayoutChange -= OnLayoutChange;
    }

    /// <summary>
    /// Handles the layout change event of the associated platform view.
    /// </summary>
    /// <param name="sender">The source of the layout change event, usually the platform view.</param>
    /// <param name="e">Event data for the layout change event containing layout information.</param>
    private void OnLayoutChange(object? sender, Android.Views.View.LayoutChangeEventArgs e)
    {
        if (sender is Android.Views.View view
            && _weakBottomSheet?.TryGetTarget(out var bottomSheet) == true
            && view.Context is not null)
        {
            bottomSheet.PeekHeight = Convert.ToInt32(view.Context.FromPixels(view.Height));
        }
    }
}