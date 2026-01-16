namespace Plugin.Maui.BottomSheet;

/// <summary>
/// Provides extension methods for the Element class in the .NET MAUI framework.
/// </summary>
internal static class ElementExtensions
{
    /// <summary>
    /// Retrieves the parent page of the specified element by traversing its ancestor hierarchy.
    /// Returns null if no parent page is found.
    /// </summary>
    /// <param name="element">The element whose parent page is to be located.</param>
    /// <returns>The parent page of the element if found, otherwise null.</returns>
    internal static Page? GetPageParent(this Element element)
    {
        Page? page = null;
        Element? parent = element;

        if (element is IPageContainer<Page> pageContainer)
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