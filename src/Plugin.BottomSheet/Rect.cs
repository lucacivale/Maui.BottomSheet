namespace Plugin.BottomSheet;

/// <summary>
/// Represents a rectangle defined by its position and dimensions.
/// </summary>
public record Rect
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Rect"/> class.
    /// Represents a rectangular region defined by its position (X, Y) and dimensions (Width, Height).
    /// </summary>
    /// <param name="x">The X-coordinate of the rectangle.</param>
    /// <param name="y">The Y-coordinate of the rectangle.</param>
    /// <param name="width">The width of the rectangular region represented by the <see cref="Rect"/> structure.</param>
    /// <param name="height">The height of the rectangle.</param>
    public Rect(double x, double y, double width, double height)
    {
        X = x;
        Y = y;
        Width = width;
        Height = height;
    }

    /// <summary>
    /// Gets the X-coordinate of the rectangle.
    /// Represents the horizontal position of the top-left corner of the rectangle.
    /// </summary>
    public double X { get; }

    /// <summary>
    /// Gets the Y-coordinate of the rectangle.
    /// </summary>
    /// <remarks>
    /// This property represents the vertical position of the rectangle.
    /// </remarks>
    public double Y { get; }

    /// <summary>
    /// Gets the width of the rectangular region represented by the <see cref="Rect"/> structure.
    /// </summary>
    /// <remarks>
    /// The <c>Width</c> property defines the horizontal size of the rectangle in device-independent units.
    /// This value is typically used for computations involving layout, positioning, and rendering of elements
    /// within a graphical interface.
    /// </remarks>
    public double Width { get; }

    /// <summary>
    /// Gets the height of the rectangle.
    /// </summary>
    /// <remarks>
    /// Represents the vertical extent of the rectangle. Typically used in layout or positioning calculations
    /// along with the width, x-coordinate, and y-coordinate.
    /// </remarks>
    public double Height { get; }
}