namespace Plugin.Maui.BottomSheet;

internal static class ThicknessExtensions
{
    public static Plugin.BottomSheet.Thickness ToThickness(this Thickness thickness)
    {
        return new Plugin.BottomSheet.Thickness(thickness.Left, thickness.Top, thickness.Right, thickness.Bottom);
    }
}