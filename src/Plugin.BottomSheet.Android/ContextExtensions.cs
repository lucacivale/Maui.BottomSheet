using System.Runtime.CompilerServices;
using Android.Util;

namespace Plugin.BottomSheet.Android;

internal static class ContextExtensions
{
    private static float _displayDensity = float.MinValue;

    public static float ToPixels(this Context self, double dp)
    {
        EnsureMetrics(self);

        return ToPixelsUsingMetrics(dp);
    }

    public static double FromPixels(this Context self, double pixels)
    {
        EnsureMetrics(self);

        return FromPixelsUsingMetrics(pixels);
    }

    private static void EnsureMetrics(Context context)
    {
        if (Math.Abs(_displayDensity - float.MinValue) > 0)
        {
            return;
        }

        using DisplayMetrics? metrics = context.Resources?.DisplayMetrics;

        _displayDensity = metrics?.Density ?? 1;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static float ToPixelsUsingMetrics(double dp)
    {
        return (float)Math.Ceiling((dp * _displayDensity) - 0.0000000001f);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static double FromPixelsUsingMetrics(double pixels)
    {
        return pixels / _displayDensity;
    }
}