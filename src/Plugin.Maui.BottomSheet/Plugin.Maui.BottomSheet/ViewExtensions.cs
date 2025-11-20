namespace Plugin.Maui.BottomSheet;

/// <summary>
/// Extension methods for View objects to provide bottom sheet-related functionality.
/// </summary>
internal static class ViewExtensions
{
    internal static Size Measure(this View view)
    {
        Size size = Size.Zero;

        if (view is ICrossPlatformLayout crossPlatformLayout)
        {
            size = crossPlatformLayout.CrossPlatformMeasure(view.Window?.Width ?? double.PositiveInfinity, view.Window?.Height ?? double.NegativeInfinity);
        }
        else
        {
            size.Height = view.Height;
            size.Width = view.Width;

            if (size.Height <= 0
                || size.Width <= 0)
            {
                size = view.Measure(view.Window?.Width ?? double.PositiveInfinity, view.Window?.Height ?? double.NegativeInfinity);
            }
        }

        return size;
    }
}