namespace Plugin.Maui.BottomSheet.Behaviors.BottomSheetPeekBehavior;

using System.Diagnostics.CodeAnalysis;
using UIKit;

/// <summary>
/// iOS MacCatalyst implementation.
/// </summary>
public sealed partial class BottomSheetPeekBehavior
{
    private WeakReference<IBottomSheet>? _weakBottomSheet;

    /// <inheritdoc/>
    // ReSharper disable once RedundantNullableFlowAttribute
    protected override void OnAttachedTo([NotNull]View bindable, UIKit.UIView platformView)
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

    /// <inheritdoc/>
    protected override void OnDetachedFrom(View bindable, UIView platformView)
    {
        base.OnDetachedFrom(bindable, platformView);

        bindable.MeasureInvalidated -= OnMeasureInvalidated;
    }

    private static double CalculateHeight(View view)
    {
#if NET9_0
        var size = view.Measure(double.PositiveInfinity, double.PositiveInfinity);
        return size.Height;
#elif NET8_0
        var size = view.Measure(double.PositiveInfinity, double.PositiveInfinity, MeasureFlags.IncludeMargins);
        return size.Request.Height;
#endif
    }

    private void OnMeasureInvalidated(object? sender, EventArgs e)
    {
        if (sender is View view
            && _weakBottomSheet?.TryGetTarget(out var bottomSheet) == true)
        {
            bottomSheet.PeekHeight = CalculateHeight(view);
        }
    }
}