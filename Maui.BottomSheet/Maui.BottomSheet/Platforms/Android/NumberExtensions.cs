using Android.Content;
using Microsoft.Maui.Platform;

namespace Maui.BottomSheet.Platforms.Android;

internal static class NumberExtensions
{
    public static float ToPixels(this double value, Context? context)
    {
        if (context is null)
        {
            return (float)value;
        }

        return context.ToPixels(value);
    }

    public static float ToPixels(this int value, Context? context)
    {
        if (context is null)
        {
            return (float)value;
        }

        return context.ToPixels(value);
    }

}