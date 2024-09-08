namespace Maui.BottomSheet;
public static class ContentPageExtensions
{
    public static IBottomSheet ToBottomSheet(this ContentPage contentPage)
    {
        return new BottomSheet()
        {
            BackgroundColor = contentPage.BackgroundColor,
            ContentTemplate = new DataTemplate(() => contentPage.Content)
        };
    }
}
