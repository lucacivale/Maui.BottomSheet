using Google.Android.Material.Button;

namespace Plugin.BottomSheet.Android;

/// <summary>
/// Represents a button used to trigger a close action, intended for Android platforms,
/// and styled using the Material Design guidelines.
/// </summary>
/// <remarks>
/// This class extends <see cref="Google.Android.Material.Button.MaterialButton"/> to provide
/// a customizable close button with specific configurations for icon and appearance.
/// </remarks>
internal sealed class CloseButton : MaterialButton
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CloseButton"/> class.
    /// Represents a custom close button for Android platform that inherits from MaterialButton.
    /// This button is specifically designed to support features like customizable icon tint,
    /// size calculation based on height and width, and a transparent background.
    /// </summary>
    /// <param name="context">The context in which the button is being created.</param>
    public CloseButton(Context context)
        : base(context)
    {
        SetIconResource(_Microsoft.Android.Resource.Designer.Resource.Drawable.mtrl_ic_cancel);
        IconGravity = IconGravityTextStart;
        IconPadding = 0;
        BackgroundTintList = ColorStateList.ValueOf(Color.Transparent);
    }

    /// <summary>
    /// Gets or sets the tint color applied to the icon displayed within the button.
    /// </summary>
    /// <remarks>
    /// The property allows customization of the icon's color by accepting a nullable <see cref="Color"/> value.
    /// If a color is provided, it will be applied to the button's icon using a <see cref="ColorStateList"/>.
    /// If null, no tint is applied, and the icon's default color is used.
    /// </remarks>
    public new Color? IconTint
    {
        get
        {
            Color? color = null;

            if (base.IconTint is ColorStateList colorStateList)
            {
                color = new(colorStateList.DefaultColor);
            }

            return color;
        }

        set
        {
            if (value is Color color)
            {
                base.IconTint = ColorStateList.ValueOf(color);
            }
        }
    }

    /// <summary>
    /// Sets the height of the button in pixels and adjusts the icon size accordingly.
    /// </summary>
    /// <param name="pixels">The height in pixels to set for the button.</param>
    public override void SetHeight(int pixels)
    {
        base.SetHeight(pixels);

        SetIconSize();
    }

    /// <summary>
    /// Sets the width of the button in pixels and adjusts the icon size accordingly.
    /// </summary>
    /// <param name="pixels">The width of the button in pixels.</param>
    public override void SetWidth(int pixels)
    {
        base.SetWidth(pixels);

        SetIconSize();
    }

    /// <summary>
    /// Adjusts the size of the icon displayed on the button based on the minimum height
    /// and width dimensions of the button. The icon size is calculated as half of the
    /// average of the minimum height and width values.
    /// </summary>
    private void SetIconSize()
    {
        if (Context is null)
        {
            return;
        }

        IconSize = Convert.ToInt32((double)(MinHeight + MinWidth) / 2);
    }
}