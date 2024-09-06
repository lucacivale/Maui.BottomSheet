namespace Plugin.Maui.BottomSheet.PlatformViews;

using Android.Content;
using AndroidView = Android.Views.View;

public class MauiBottomSheet : AndroidView
{
    private readonly IMauiContext _mauiContext;
    private readonly BottomSheetLayoutChangeListener _layoutChangeListener;

    private IBottomSheet? _virtualView;

    /// <inheritdoc />
    public MauiBottomSheet(IMauiContext mauiContext, Context context)
        : base(context)
    {
        _mauiContext = mauiContext;
        _layoutChangeListener = new BottomSheetLayoutChangeListener(this);
    }

    public void SetView(IBottomSheet virtualView)
    {
        _virtualView = virtualView;
    }

    public void Cleanup()
    {
    }

    public void SetIsCancelable()
    {
        throw new NotImplementedException();
    }

    public void SetHasHandle()
    {
        throw new NotImplementedException();
    }

    public void SetHeader()
    {
        throw new NotImplementedException();
    }

    public void SetIsOpen()
    {
        throw new NotImplementedException();
    }

    public void SetIsDraggable()
    {
        throw new NotImplementedException();
    }

    public void SetStates()
    {
        throw new NotImplementedException();
    }

    public void SetCurrentState()
    {
        throw new NotImplementedException();
    }

    public void SetPeek()
    {
        throw new NotImplementedException();
    }

    public void SetContentTemplate()
    {
        throw new NotImplementedException();
    }
}