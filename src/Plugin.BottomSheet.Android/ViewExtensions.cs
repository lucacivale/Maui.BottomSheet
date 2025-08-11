namespace Plugin.BottomSheet.Android;

internal static class ViewExtensions
{
    internal static void RemoveFromParent(this View view)
    {
        if (view.Parent is IViewManager viewManager)
        {
            viewManager.RemoveView(view);
        }
    }
}
