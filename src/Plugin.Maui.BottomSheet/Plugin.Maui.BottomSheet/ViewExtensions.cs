namespace Plugin.Maui.BottomSheet;

/// <summary>
/// <see cref="View"/> extension methods.
/// </summary>
internal static class ViewExtensions
{
    /// <summary>
    /// Finds <see cref="IBottomSheet"/> parent of view.
    /// </summary>
    /// <param name="view"><see cref="View"/>.</param>
    /// <returns>Found <see cref="IBottomSheet"/> instance.</returns>
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
    /// Finds BottomSheetPage parent of <see cref="View"/>.
    /// </summary>
    /// <param name="view"><see cref="View"/>.</param>
    /// <returns>BottomSheetPage parent.</returns>
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
}