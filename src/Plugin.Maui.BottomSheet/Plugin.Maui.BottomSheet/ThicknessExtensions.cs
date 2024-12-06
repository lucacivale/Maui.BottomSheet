namespace Plugin.Maui.BottomSheet;

/// <summary>
/// <see cref="Thickness"/> extension methods.
/// </summary>
public static class ThicknessExtensions
{
    /// <summary>
    /// <see cref="Thickness"/> with horizontal(left and right) values set.
    /// </summary>
    /// <param name="thickness"><see cref="Thickness"/>.</param>
    /// <returns>Horizontal thickness.</returns>
    public static Thickness HorizontalThickness(this Thickness thickness)
    {
        return new Thickness(
            thickness.Left,
            0,
            thickness.Right,
            0);
    }
}