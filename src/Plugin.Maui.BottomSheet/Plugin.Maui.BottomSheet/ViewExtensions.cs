namespace Plugin.Maui.BottomSheet;

/// <summary>
/// Provides extension methods for measuring the size of View objects in a cross-platform context.
/// </summary>
internal static class ViewExtensions
{
    /// <summary>
    /// Measures the size of a <see cref="View"/> and returns it as a <see cref="Size"/> object.
    /// </summary>
    /// <param name="view">The <see cref="View"/> to be measured.</param>
    /// <returns>The measured size of the <see cref="View"/> as a <see cref="Size"/> object.</returns>
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