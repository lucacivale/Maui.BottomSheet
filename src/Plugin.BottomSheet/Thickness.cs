namespace Plugin.BottomSheet;

public struct Thickness
{
    public Thickness(int uniform)
    {
        Left = Top = Right = Bottom = uniform;
    }

    public Thickness(int horizontal, int vertical)
    {
        Left = Right = horizontal;
        Top = Bottom = vertical;
    }

    public Thickness(int left, int top, int right, int bottom)
    {
        Left = left;
        Top = top;
        Right = right;
        Bottom = bottom;
    }

    public int Left { get; }

    public int Top { get; }

    public int Right { get; }

    public int Bottom { get; }
}