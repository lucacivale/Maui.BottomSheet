namespace Plugin.BottomSheet.Android;

/// <summary>
/// Provides extension methods for converting <see cref="Thickness"/> values to and from pixel values
/// based on the device display metrics.
/// </summary>
internal static class ThicknessExtensions
{
    /// <summary>
    /// Converts the specified <see cref="Thickness"/> instance to pixel values using the given <see cref="Context"/>.
    /// </summary>
    /// <param name="thickness">The <see cref="Thickness"/> instance to be converted.</param>
    /// <param name="context">The Android <see cref="Context"/> used to convert the thickness values to pixels.</param>
    /// <returns>A new <see cref="Thickness"/> instance where each value (left, top, right, and bottom) is expressed in pixels.</returns>
    public static Thickness ToPixels(this Thickness thickness, Context context)
    {
        return new Thickness(
            context.ToPixels(thickness.Left),
            context.ToPixels(thickness.Top),
            context.ToPixels(thickness.Right),
            context.ToPixels(thickness.Bottom));
    }

    /// <summary>
    /// Converts a <see cref="Thickness"/> value from pixel units to device-independent units
    /// based on the current display metrics of the provided <see cref="Context"/>.
    /// </summary>
    /// <param name="thickness">The <see cref="Thickness"/> value to be converted from pixels.</param>
    /// <param name="context">The <see cref="Context"/> used to retrieve display metrics for conversion.</param>
    /// <returns>
    /// A <see cref="Thickness"/> instance where the thickness values for the left, top, right, and bottom
    /// have been converted to device-independent units.
    /// </returns>
    public static Thickness FromPixels(this Thickness thickness, Context context)
    {
        return new Thickness(
            context.FromPixels(thickness.Left),
            context.FromPixels(thickness.Top),
            context.FromPixels(thickness.Right),
            context.FromPixels(thickness.Bottom));
    }
}
