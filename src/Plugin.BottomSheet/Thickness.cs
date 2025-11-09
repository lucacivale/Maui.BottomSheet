namespace Plugin.BottomSheet;

internal record Thickness
{
    public Thickness(double uniform)
    {
        Left = Top = Right = Bottom = uniform;
    }

    public Thickness(double horizontal, double vertical)
    {
        Left = Right = horizontal;
        Top = Bottom = vertical;
    }

    public Thickness(double left, double top, double right, double bottom)
    {
        Left = left;
        Top = top;
        Right = right;
        Bottom = bottom;
    }

    public double Left { get; }

    public double Top { get; }

    public double Right { get; }

    public double Bottom { get; }
}