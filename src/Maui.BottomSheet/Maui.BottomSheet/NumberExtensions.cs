namespace Maui.BottomSheet;

internal static class NumberExtensions
{
    public static int RoundUpToNextInt(this double value)
    {
        return Convert.ToInt32(Math.Round(value, MidpointRounding.AwayFromZero));
    }

    public static int RoundUpToNextInt(this float value)
    {
        return Convert.ToInt32(Math.Round(value, MidpointRounding.AwayFromZero));
    }

}