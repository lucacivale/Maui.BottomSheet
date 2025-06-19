namespace Plugin.Maui.BottomSheet;

// ReSharper disable once InconsistentNaming

/// <summary>
/// Extension methods for the IBottomSheet interface.
/// </summary>
internal static class IBottomSheetExtensions
{
    /// <summary>
    /// Gets the parent page of the bottom sheet by traversing the visual tree.
    /// </summary>
    /// <param name="bottomSheet">The bottom sheet instance.</param>
    /// <returns>The parent page if found, otherwise null.</returns>
    internal static Page? GetPageParent(this IBottomSheet bottomSheet)
    {
        Page? page = null;

        var parent = bottomSheet.Parent;

        if (parent is IPageContainer<Page> pageContainer)
        {
            page = pageContainer.CurrentPage;
        }
        else if (parent is FlyoutPage flyoutPage)
        {
            page = flyoutPage.IsPresented ? flyoutPage.Flyout : flyoutPage.Detail;

            if (page is IPageContainer<Page> container)
            {
                page = container.CurrentPage;
            }
        }
        else
        {
            while (parent is not null)
            {
                if (parent is ContentPage contentPage)
                {
                    page = contentPage;
                    break;
                }

                parent = parent.Parent;
            }
        }

        return page;
    }
}