namespace Plugin.BottomSheet.Android;

/// <summary>
/// Creates and manages the drag handle view for bottom sheets.
/// </summary>
internal sealed class BottomSheetHandle : LinearLayout
{
    private const int Padding = 5;
    private const int CornerRadius = 8;
    private const string HandleColor = "#3d3d40";

    /// <summary>
    /// Initializes a new instance of the <see cref="BottomSheetHandle"/> class.
    /// </summary>
    /// <param name="context">The Android context.</param>
    public BottomSheetHandle(Context context)
        : base(context)
    {
        LayoutParameters = new ViewGroup.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent);

        int pixelPadding = Convert.ToInt32(context.ToPixels(Padding));

        SetPadding(
            pixelPadding,
            pixelPadding,
            pixelPadding,
            pixelPadding);

        GradientDrawable background = new();
        background.SetShape(ShapeType.Rectangle);
        background.SetColor(ColorStateList.ValueOf(Color.ParseColor(HandleColor)));
        background.SetCornerRadius(context.ToPixels(CornerRadius));

        Background = background;
        Id = _Microsoft.Android.Resource.Designer.Resource.Id.Plugin_BottomSheet_Android_Handle;
    }
}