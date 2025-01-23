namespace Plugin.Maui.BottomSheet.Platform.MaciOS;

/// <inheritdoc cref="Microsoft.Maui.Controls.Button" />
public sealed class CloseButton : Button, ICrossPlatformLayout
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CloseButton"/> class.
    /// </summary>
    public CloseButton()
    {
        HeightRequest = 40;
        WidthRequest = 40;
    }

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