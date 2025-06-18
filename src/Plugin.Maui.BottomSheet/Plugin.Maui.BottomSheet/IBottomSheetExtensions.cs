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