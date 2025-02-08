namespace Plugin.Maui.BottomSheet;

// ReSharper disable once InconsistentNaming

/// <summary>
/// <see cref="IBottomSheet"/> extension methods.
/// </summary>
internal static class IBottomSheetExtensions
{
    /// <summary>
    /// Get the <see cref="ContentPage"/> parent of <see cref="IBottomSheet"/>.
    /// </summary>
    /// <param name="bottomSheet"><see cref="IBottomSheet"/>.</param>
    /// <returns><see cref="ContentPage"/> parent.</returns>
    internal static ContentPage? GetPageParent(this IBottomSheet bottomSheet)
    {
        ContentPage? page = null;

        var parent = bottomSheet.Parent;
        while (parent is not null)
        {
            if (parent is ContentPage contentPage)
            {
                page = contentPage;
                break;
            }

            parent = parent.Parent;
        }

        return page;
    }
}