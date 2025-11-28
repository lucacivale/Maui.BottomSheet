using System.Runtime.CompilerServices;
using Android.Util;

namespace Plugin.BottomSheet.Android;

/// <summary>
/// A set of extension methods for the Android <c>Context</c> class to assist with
/// converting between density-independent pixels (dp) and pixels (px), based on the device's
/// display density.
/// </summary>
internal static class ContextExtensions
{
    /// <summary>
    /// Represents the pixel density of the current display, used for converting
    /// between density-independent pixels (dp) and actual pixels. This field is initialized
    /// to <see cref="float.MinValue"/> and updated when display metrics are retrieved
    /// via the <c>EnsureMetrics</c> method.
    /// </summary>
    /// <remarks>
    /// The value is typically derived from the screen density provided by
    /// Android's <c>DisplayMetrics</c> and is defaulted to 1 if metrics are unavailable.
    /// </remarks>
    private static float _displayDensity = float.MinValue;

    /// <summary>
    /// Converts a value from density-independent pixels (dp) to device-specific pixels (px) using the display metrics of the provided context.
    /// </summary>
    /// <param name="self">The <see cref="Context"/> instance providing display metrics.</param>
    /// <param name="dp">The value in density-independent pixels (dp) to be converted to pixels (px).</param>
    /// <returns>The equivalent value in pixels (px) for the specified dp value.</returns>
    public static float ToPixels(this Context self, double dp)
    {
        EnsureMetrics(self);

        return ToPixelsUsingMetrics(dp);
    }

    /// <summary>
    /// Converts a pixel value to its equivalent in density-independent pixels (dp) based on the current display metrics.
    /// </summary>
    /// <param name="self">The <see cref="Context"/> used to access display metrics and determine the screen density.</param>
    /// <param name="pixels">The pixel value to be converted to density-independent pixels.</param>
    /// <returns>The equivalent value in density-independent pixels (dp).</returns>
    public static double FromPixels(this Context self, double pixels)
    {
        EnsureMetrics(self);

        return FromPixelsUsingMetrics(pixels);
    }

    /// <summary>
    /// Ensures that the display metrics have been initialized for the provided context.
    /// This method sets the display density value used for pixel and density calculations.
    /// </summary>
    /// <param name="context">The Android context used to retrieve display metrics.</param>
    private static void EnsureMetrics(Context context)
    {
        if (Math.Abs(_displayDensity - float.MinValue) > 0)
        {
            return;
        }

        using DisplayMetrics? metrics = context.Resources?.DisplayMetrics;

        _displayDensity = metrics?.Density ?? 1;
    }

    /// Converts a value in density-independent pixels (dp) to a value in pixels using the current display density.
    /// <param name="dp">The density-independent pixels (dp) value to be converted.</param>
    /// <return>Returns the equivalent pixel value as a float.</return>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static float ToPixelsUsingMetrics(double dp)
    {
        return (float)Math.Ceiling((dp * _displayDensity) - 0.0000000001f);
    }

    /// <summary>
    /// Converts a pixel value to its equivalent density-independent value
    /// using the current display's density metrics.
    /// </summary>
    /// <param name="pixels">The pixel value to be converted.</param>
    /// <returns>The density-independent value (dp) equivalent to the given pixel value.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static double FromPixelsUsingMetrics(double pixels)
    {
        return pixels / _displayDensity;
    }
}