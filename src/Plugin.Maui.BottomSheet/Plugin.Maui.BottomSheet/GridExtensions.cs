using System.Diagnostics.CodeAnalysis;

namespace Plugin.Maui.BottomSheet;

/// <summary>
/// Provides extension methods for the <see cref="Grid"/> class, enabling additional
/// functionality such as removing views and retrieving views based on custom logic.
/// </summary>
internal static class GridExtensions
{
    /// <summary>
    /// Attempts to retrieve a child view of the specified type and row index from the grid.
    /// </summary>
    /// <typeparam name="TView">The type of the view to retrieve. Must derive from <see cref="View"/>.</typeparam>
    /// <param name="grid">The grid from which to retrieve the view.</param>
    /// <param name="row">The row index of the view to retrieve.</param>
    /// <param name="view">
    /// When this method returns, contains the view of type <typeparamref name="TView"/> located in the specified row index,
    /// if found; otherwise, null.
    /// </param>
    /// <returns>
    /// true if a view of the specified type is found in the given row; otherwise, false.
    /// </returns>
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