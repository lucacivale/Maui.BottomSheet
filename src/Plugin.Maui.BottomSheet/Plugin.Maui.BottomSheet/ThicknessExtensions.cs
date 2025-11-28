namespace Plugin.Maui.BottomSheet;

/// <summary>
/// Provides extension methods for converting instances of <see cref="Thickness"/>
/// from the Plugin.Maui.BottomSheet namespace to <see cref="Thickness"/>
/// within the Plugin.BottomSheet namespace.
/// </summary>
internal static class ThicknessExtensions
{
    /// <summary>
    /// Converts a <see cref="Plugin.BottomSheet.Thickness"/> to a <see cref="Thickness"/> instance.
    /// </summary>
    /// <param name="thickness">The source <see cref="Thickness"/> object to convert.</param>
    /// <returns>A new <see cref="Plugin.BottomSheet.Thickness"/> instance representing the same values as the source.</returns>
    public static Plugin.BottomSheet.Thickness ToThickness(this Thickness thickness)
    {
        return new Plugin.BottomSheet.Thickness(thickness.Left, thickness.Top, thickness.Right, thickness.Bottom);
    }
}