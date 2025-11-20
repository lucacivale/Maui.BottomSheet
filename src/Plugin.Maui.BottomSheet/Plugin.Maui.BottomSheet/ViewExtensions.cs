namespace Plugin.Maui.BottomSheet;

/// <summary>
/// Extension methods for View objects to provide bottom sheet-related functionality.
/// </summary>
internal static class ViewExtensions
{
    /// <summary>
    /// Traverses the visual tree upward to find the first parent that implements IBottomSheet.
    /// </summary>
    /// <param name="view">The view to start the search from.</param>
    /// <returns>The first IBottomSheet parent found, or null if none exists.</returns>
    internal static IBottomSheet? FindBottomSheet(this View view)
    {
        IBottomSheet? bottomSheet = null;
        var parent = view.Parent;

        while (parent is not null)
        {
            if (parent is BottomSheet sheet)
            {
                bottomSheet = sheet;
                break;
            }

            parent = parent.Parent;
        }

        return bottomSheet;
    }

    #if MACCATALYST || IOS
    /// <summary>
    /// Traverses the visual tree upward to find the first parent that is a BottomSheetPage (macOS/iOS platforms only).
    /// </summary>
    /// <param name="view">The view to start the search from.</param>
    /// <returns>The first BottomSheetPage parent found, or null if none exists.</returns>
    internal static Platform.MaciOS.BottomSheetPage? FindBottomSheetPage(this View view)
    {
        Platform.MaciOS.BottomSheetPage? bottomSheetPage = null;
        var parent = view.Parent;

        while (parent is not null)
        {
            if (parent is Platform.MaciOS.BottomSheetPage sheetPage)
            {
                bottomSheetPage = sheetPage;
                break;
            }

            parent = parent.Parent;
        }

        return bottomSheetPage;
    }
    #endif

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