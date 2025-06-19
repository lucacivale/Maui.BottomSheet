namespace Plugin.Maui.BottomSheet.Platform.MaciOS;

/// <summary>
/// A custom button control for closing bottom sheets with tint color customization.
/// </summary>
public sealed class CloseButton : Button, ICrossPlatformLayout
{
    /// <summary>
    /// Bindable property for the tint color of the close button.
    /// </summary>
    public static readonly BindableProperty TintProperty =
        BindableProperty.Create(
            nameof(TintColor),
            typeof(Color),
            typeof(CloseButton));

    /// <summary>
    /// Gets or sets the tint color of the close button.
    /// </summary>
    public Color TintColor { get => (Color)GetValue(TintProperty); set => SetValue(TintProperty, value); }

    /// <summary>
    /// Measures the desired size of the close button for cross-platform layout.
    /// </summary>
    /// <param name="widthConstraint">The width constraint for measurement.</param>
    /// <param name="heightConstraint">The height constraint for measurement.</param>
    /// <returns>The desired size as Size.Zero for custom layout handling.</returns>
    Size ICrossPlatformLayout.CrossPlatformMeasure(double widthConstraint, double heightConstraint)
    {
        return Size.Zero;
    }

    /// <summary>
    /// Arranges the close button within the specified bounds for cross-platform layout.
    /// </summary>
    /// <param name="bounds">The bounds rectangle for arrangement.</param>
    /// <returns>The arranged size as Size.Zero for custom layout handling.</returns>
    Size ICrossPlatformLayout.CrossPlatformArrange(Rect bounds)
    {
        return Size.Zero;
    }
}