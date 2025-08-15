using Google.Android.Material.Button;

namespace Plugin.BottomSheet.Android;

internal sealed class CloseButton : MaterialButton
{
    public CloseButton(Context context)
        : base(context)
    {
        SetIconResource(_Microsoft.Android.Resource.Designer.Resource.Drawable.mtrl_ic_cancel);
        IconGravity = IconGravityTextStart;
        IconPadding = 0;
        BackgroundTintList = ColorStateList.ValueOf(Color.Transparent);
    }

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

    public override void SetHeight(int pixels)
    {
        if (pixels <= 0)
        {
            return;
        }

        base.SetHeight(pixels);

        SetIconSize();
    }

    public override void SetWidth(int pixels)
    {
        if (pixels <= 0)
        {
            return;
        }

        base.SetWidth(pixels);

        SetIconSize();
    }

    private void SetIconSize()
    {
        if (Context is null)
        {
            return;
        }

        IconSize = Convert.ToInt32((double)(MinHeight + MinWidth) / 2);
    }
}