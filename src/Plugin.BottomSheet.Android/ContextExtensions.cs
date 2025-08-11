using System.Runtime.CompilerServices;
using Android.Content;
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
}