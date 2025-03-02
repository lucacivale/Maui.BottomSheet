namespace Plugin.Maui.BottomSheet.Platform.MaciOS;

/// <inheritdoc cref="Microsoft.Maui.Controls.Button" />
public sealed class CloseButton : Button, ICrossPlatformLayout
{
    /// <summary>
    /// Bindable property.
    /// </summary>
    public static readonly BindableProperty TintProperty =
        BindableProperty.Create(
            nameof(TintColor),
            typeof(Color),
            typeof(CloseButton));

    /// <summary>
    /// Gets or sets tint color.
    /// </summary>
    public Color TintColor { get => (Color)GetValue(TintProperty); set => SetValue(TintProperty, value); }

    /// <inheritdoc/>
    Size ICrossPlatformLayout.CrossPlatformMeasure(double widthConstraint, double heightConstraint)
    {
        return Size.Zero;
    }

    /// <inheritdoc/>
    Size ICrossPlatformLayout.CrossPlatformArrange(Rect bounds)
    {
        return Size.Zero;
    }
}