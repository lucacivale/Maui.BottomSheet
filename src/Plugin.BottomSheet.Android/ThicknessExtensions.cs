namespace Plugin.BottomSheet.Android;

internal static class ThicknessExtensions
{
    public static Thickness ToPixels(this Thickness thickness, Context context)
    {
        return new Thickness(
            context.ToPixels(thickness.Left),
            context.ToPixels(thickness.Top),
            context.ToPixels(thickness.Right),
            context.ToPixels(thickness.Bottom));
    }

    public static Thickness FromPixels(this Thickness thickness, Context context)
    {
        return new Thickness(
            context.FromPixels(thickness.Left),
            context.FromPixels(thickness.Top),
            context.FromPixels(thickness.Right),
            context.FromPixels(thickness.Bottom));
    }
}
