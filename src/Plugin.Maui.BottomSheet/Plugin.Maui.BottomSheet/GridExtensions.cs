namespace Plugin.Maui.BottomSheet;

internal static class GridExtensions
{
    internal static void Remove(this Grid grid, View? view)
    {
        if (view is not null)
        {
            grid.Children.Remove(view);
        }
    }
}