namespace Plugin.Maui.BottomSheet.Behaviors.BottomSheetPeekBehavior;

using Microsoft.Maui.Platform;

/// <summary>
/// Android implementation.
/// </summary>
public sealed partial class BottomSheetPeekBehavior
{
    private WeakReference<IBottomSheet>? _weakBottomSheet;

    /// <inheritdoc/>
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

    /// <inheritdoc/>
    protected override void OnDetachedFrom(View bindable, Android.Views.View platformView)
    {
        base.OnDetachedFrom(bindable, platformView);

        platformView.LayoutChange -= OnLayoutChange;
    }

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