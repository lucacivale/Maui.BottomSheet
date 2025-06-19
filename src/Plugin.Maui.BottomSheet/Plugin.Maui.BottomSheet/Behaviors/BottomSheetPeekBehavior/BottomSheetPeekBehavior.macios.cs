namespace Plugin.Maui.BottomSheet.Behaviors.BottomSheetPeekBehavior;

using System.Diagnostics.CodeAnalysis;
using UIKit;

/// <summary>
/// Represents a behavior for controlling the bottom sheet's peek height functionality
/// within a Maui application on iOS platforms.
/// </summary>
public sealed partial class BottomSheetPeekBehavior
{
    private WeakReference<IBottomSheet>? _weakBottomSheet;

    /// <summary>
    /// Called when the behavior is attached to a <see cref="View"/> and its associated platform-specific <see cref="UIView"/>.
    /// Initializes required references or settings for the bottom sheet behavior.
    /// </summary>
    /// <param name="bindable">The <see cref="View"/> to which the behavior is attached.</param>
    /// <param name="platformView">The platform-specific <see cref="UIView"/> representation of the bindable view.</param>
    // ReSharper disable once RedundantNullableFlowAttribute
    protected override void OnAttachedTo([NotNull]View bindable, UIView platformView)
    {
        base.OnAttachedTo(bindable, platformView);
        var bottomSheetPage = bindable.FindBottomSheetPage();

        if (bottomSheetPage?.BottomSheet is not null)
        {
            _weakBottomSheet = new WeakReference<IBottomSheet>(bottomSheetPage.BottomSheet);
            bottomSheetPage.BottomSheet.PeekHeight = CalculateHeight(bindable);
        }

        bindable.MeasureInvalidated += OnMeasureInvalidated;
    }

    /// <summary>
    /// Called when the behavior is detached from a <see cref="View"/> and its associated <see cref="UIView"/>.
    /// This method cleans up resources and unsubscribes from events associated with the behavior.
    /// </summary>
    /// <param name="bindable">The <see cref="View"/> from which the behavior is detached.</param>
    /// <param name="platformView">The platform-specific <see cref="UIView"/> associated with the <see cref="View"/>.</param>
    protected override void OnDetachedFrom(View bindable, UIView platformView)
    {
        base.OnDetachedFrom(bindable, platformView);

        bindable.MeasureInvalidated -= OnMeasureInvalidated;
    }

    /// <summary>
    /// Calculates the height of a given view by measuring it with no constraints (infinite width and height).
    /// </summary>
    /// <param name="view">The view for which the height is to be calculated.</param>
    /// <returns>The calculated height of the specified view.</returns>
    private static double CalculateHeight(View view)
    {
        var size = view.Measure(double.PositiveInfinity, double.PositiveInfinity);
        return size.Height;
    }

    /// <summary>
    /// Handles the MeasureInvalidated event for a <see cref="View"/>. Updates the PeekHeight
    /// of the associated bottom sheet when the view's measure is invalidated.
    /// </summary>
    /// <param name="sender">The sender of the event, expected to be a <see cref="View"/>.</param>
    /// <param name="e">Event arguments for the MeasureInvalidated event.</param>
    private void OnMeasureInvalidated(object? sender, EventArgs e)
    {
        if (sender is View view
            && _weakBottomSheet?.TryGetTarget(out var bottomSheet) == true)
        {
            bottomSheet.PeekHeight = CalculateHeight(view);
        }
    }
}