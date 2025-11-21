namespace Plugin.BottomSheet.Android;

/// <summary>
/// Provides extension methods for performing operations on <see cref="View"/> objects in Android.
/// </summary>
internal static class ViewExtensions
{
    /// <summary>
    /// Removes the specified view from its parent if it is currently attached to a view manager.
    /// </summary>
    /// <param name="view">The view to be removed from its parent.</param>
    internal static void RemoveFromParent(this View view)
    {
        if (view.Parent is IViewManager viewManager)
        {
            viewManager.RemoveView(view);
        }
    }
}
