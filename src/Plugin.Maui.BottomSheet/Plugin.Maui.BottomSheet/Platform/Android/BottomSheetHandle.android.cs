#pragma warning disable SA1200
using AColor = Android.Graphics.Color;
using AColorStateList = Android.Content.Res.ColorStateList;
using AContext = Android.Content;
using AGradientDrawable = Android.Graphics.Drawables.GradientDrawable;
using ALinearLayout = Android.Widget.LinearLayout;
using AShapeType = Android.Graphics.Drawables.ShapeType;
using AView = Android.Views.View;
using AViewGroup = Android.Views.ViewGroup;
#pragma warning restore SA1200

namespace Plugin.Maui.BottomSheet.Platform.Android;

using Microsoft.Maui.Platform;

/// <summary>
/// Handle view.
/// </summary>
internal sealed class BottomSheetHandle : IDisposable
{
    private readonly AContext.Context _context;
    private readonly AGradientDrawable _handleBackground;

    private AView? _handle;

    /// <summary>
    /// Initializes a new instance of the <see cref="BottomSheetHandle"/> class.
    /// </summary>
    /// <param name="context">Context.</param>
    public BottomSheetHandle(AContext.Context context)
    {
        _context = context;

        _handleBackground = new AGradientDrawable();
        _handleBackground.SetShape(AShapeType.Rectangle);
        _handleBackground.SetColor(AColorStateList.ValueOf(AColor.ParseColor("#3d3d40")));
        _handleBackground.SetCornerRadius(_context.ToPixels(8));
    }

    /// <summary>
    /// Finalizes an instance of the <see cref="BottomSheetHandle"/> class.
    /// </summary>
    ~BottomSheetHandle()
    {
        Dispose(false);
    }

    /// <summary>
    /// Gets handle view.
    /// </summary>
    public AView Handle
    {
        get
        {
            if (_handle is not null)
            {
                return _handle;
            }

            _handle = new ALinearLayout(_context)
            {
                LayoutParameters = new AViewGroup.LayoutParams(AViewGroup.LayoutParams.MatchParent, AViewGroup.LayoutParams.WrapContent),
            };

            var pixelPadding = Convert.ToInt32(_context.ToPixels(5));
            _handle.SetPadding(
                pixelPadding,
                pixelPadding,
                pixelPadding,
                pixelPadding);

            _handle.Background = _handleBackground;

            return _handle;
        }
    }

    /// <summary>
    /// Remove handle view.
    /// </summary>
    public void Remove()
    {
        Handle.RemoveFromParent();
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
    {
        if (!disposing)
        {
            return;
        }

        _handleBackground.Dispose();
        _handle?.Dispose();
        _handle = null;
    }
}