#pragma warning disable SA1200
using AContext = Android.Content;
using AContextCompat = AndroidX.Core.Content.ContextCompat;
using ALayoutInflater = Android.Views.LayoutInflater;
using AView = Android.Views.View;
#pragma warning restore SA1200

// ReSharper disable once CheckNamespace
namespace Plugin.Maui.BottomSheet.Platforms.Android;

using _Microsoft.Android.Resource.Designer;
using Microsoft.Maui.Platform;

/// <summary>
/// Handle view.
/// </summary>
public sealed class BottomSheetHandle : IDisposable
{
    private readonly AContext.Context _context;

    private AView? _handle;

    /// <summary>
    /// Initializes a new instance of the <see cref="BottomSheetHandle"/> class.
    /// </summary>
    /// <param name="context">Context.</param>
    public BottomSheetHandle(AContext.Context context)
    {
        _context = context;
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
            if (_handle is null
                && ALayoutInflater.FromContext(_context) is ALayoutInflater inflater
                && inflater.Inflate(Resource.Layout.bottomSheetDragHandle, null) is AView view)
            {
                _handle = view;
                _handle.Background = AContextCompat.GetDrawable(_context, Resource.Drawable.round_rect_shape);
            }

            ArgumentNullException.ThrowIfNull(_handle);

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

        _handle?.Dispose();
        _handle = null;
    }
}