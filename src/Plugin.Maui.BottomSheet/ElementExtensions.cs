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

        if (parent is Shell || parent is NavigationPage)
        {
            page = GetCurrentPageFromNavigation(parent);
        }
        else if (parent is FlyoutPage flyoutPage)
        {
            page = flyoutPage.IsPresented ? flyoutPage.Flyout : flyoutPage.Detail;

            if (page is Shell
                || page is NavigationPage)
            {
                page = GetCurrentPageFromNavigation(page);
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

    /// <summary>
    /// Retrieves the currently active page from the navigation stack of the specified element.
    /// This method determines the top page from either the navigation stack or the modal stack.
    /// </summary>
    /// <param name="element">The element whose navigation stack is to be inspected.</param>
    /// <returns>The currently active page if found, otherwise null.</returns>
    private static Page GetCurrentPageFromNavigation(Element element)
    {
        Page currentPage;
        if (element is Shell shell)
        {
            currentPage = shell.CurrentPage;
        }
        else
        {
            currentPage = ((NavigationPage)element).CurrentPage;
        }

        return currentPage;
    }
}