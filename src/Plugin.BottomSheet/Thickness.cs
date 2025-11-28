namespace Plugin.BottomSheet;

/// <summary>
/// Represents a thickness value that defines the space or padding on all four sides (left, top, right, and bottom).
/// </summary>
public record Thickness
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Thickness"/> class.
    /// Represents a thickness with four sides: Left, Top, Right, and Bottom.
    /// This type is used to define uniform or non-uniform spacing or padding.
    /// </summary>
    /// <param name="uniform">The uniform thickness value to apply to all sides.</param>
    public Thickness(double uniform)
    {
        Left = Top = Right = Bottom = uniform;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Thickness"/> class.
    /// Represents the thickness of a rectangular boundary, defined by values for the left, top, right, and bottom edges.
    /// </summary>
    /// <param name="horizontal">The thickness value for the left and right edges.</param>
    /// <param name="vertical">The thickness value for the top and bottom edges.</param>
    public Thickness(double horizontal, double vertical)
    {
        Left = Right = horizontal;
        Top = Bottom = vertical;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Thickness"/> class.
    /// Represents a structure defining the thickness or padding values for the left, top, right, and bottom sides.
    /// </summary>
    /// <param name="left">The thickness value for the left edge.</param>
    /// <param name="top">The thickness value for the top edge.</param>
    /// <param name="right">The thickness value for the right edge.</param>
    /// <param name="bottom">The thickness value for the bottom edge.</param>
    public Thickness(double left, double top, double right, double bottom)
    {
        Left = left;
        Top = top;
        Right = right;
        Bottom = bottom;
    }

    /// <summary>
    /// Gets the left component of the thickness.
    /// Represents the measurement applied to the left side of a bounding rectangle.
    /// Typically used to define padding or margin on the left side in layouts and UI elements.
    /// </summary>
    public double Left { get; }

    /// <summary>
    /// Gets the top thickness value.
    /// </summary>
    /// <remarks>
    /// This property represents the thickness measurement for the top side in a layout or control.
    /// It is commonly used to define the spacing, margin, or padding for the top edge.
    /// </remarks>
    public double Top { get; }

    /// <summary>
    /// Gets the thickness value for the right side.
    /// </summary>
    /// <remarks>
    /// Represents the measurement for the right edge, which is part of the overall
    /// thickness structure used for defining uniform or individual side dimensions.
    /// </remarks>
    public double Right { get; }

    /// <summary>
    /// Gets the thickness for the bottom side.
    /// </summary>
    /// <remarks>
    /// The <c>Bottom</c> property specifies the measurement for the bottom edge
    /// of the thickness structure. It is used to define or retrieve the spacing,
    /// padding, or margin allocated at the bottom side.
    /// </remarks>
    public double Bottom { get; }
}