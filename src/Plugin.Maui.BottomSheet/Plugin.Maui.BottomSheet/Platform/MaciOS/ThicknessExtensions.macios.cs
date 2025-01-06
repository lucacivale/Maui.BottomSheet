namespace Plugin.Maui.BottomSheet.Platform.MaciOS;

/// <summary>
/// <see cref="Thickness"/> extension methods.
/// </summary>
internal static class ThicknessExtensions
{
    /// <summary>
    /// Compare two <see cref="Thickness"/> structs and check if bottom safe area insets are added.
    /// </summary>
    /// <param name="thickness">Current value.</param>
    /// <param name="orig">Original value to compare.</param>
    /// <returns>Contains safe area insets.</returns>
    public static bool BottomContainsSafeArea(this Thickness thickness, Thickness orig)
    {
        var thicknessWithoutBottomInset = new Thickness(
            thickness.Left,
            thickness.Top,
            thickness.Right,
            thickness.Bottom - WindowUtils.CurrentSafeAreaInsets().Bottom);

        return orig.Equals(thicknessWithoutBottomInset);
    }
}