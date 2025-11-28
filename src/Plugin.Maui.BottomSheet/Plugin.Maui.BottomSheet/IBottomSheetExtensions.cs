namespace Plugin.Maui.BottomSheet;

/// <summary>
/// Provides a set of extension methods for the IBottomSheet interface
/// to enable additional functionality such as traversing the visual
/// tree to locate the parent page.
/// </summary>
internal static class IBottomSheetExtensions
{
    /// <summary>
    /// Retrieves the parent page of the specified bottom sheet by navigating through its visual tree.
    /// </summary>
    /// <param name="bottomSheet">The bottom sheet instance from which to locate the parent page.</param>
    /// <returns>The parent page if found, otherwise null.</returns>
    internal static Page? GetPageParent(this IBottomSheet bottomSheet)
    {
        Page? page = null;
        Element? parent = bottomSheet.Parent;

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