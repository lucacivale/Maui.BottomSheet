namespace Maui.BottomSheet;
public static class ViewExtensions
{
    public static IBottomSheet ToBottomSheet(this View view)
    {
        return new BottomSheet()
        {
            ContentTemplate = new DataTemplate(view.GetType())
        };
    }
}
