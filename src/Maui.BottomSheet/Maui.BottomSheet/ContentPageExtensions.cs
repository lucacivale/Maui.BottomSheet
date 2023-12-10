namespace Maui.BottomSheet;
public static class ContentPageExtensions
{
    public static IBottomSheet ToBottomSheet(this ContentPage contentPage)
    {
        return new BottomSheet()
        {
            ContentTemplate = new DataTemplate(() => contentPage.Content)
        };
    }
}
