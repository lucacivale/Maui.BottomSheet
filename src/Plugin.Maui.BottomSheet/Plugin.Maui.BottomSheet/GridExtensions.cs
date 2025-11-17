using System.Diagnostics.CodeAnalysis;

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

    internal static bool TryGetView<TView>(this Grid grid, int row, [NotNullWhen(true)] out TView? view)
        where TView : View
    {
        bool ret = false;
        view = null;

        if (grid.Children.FirstOrDefault(child => grid.GetRow(child) == row) is TView gridView)
        {
            view = gridView;
            ret = true;
        }

        return ret;
    }
}