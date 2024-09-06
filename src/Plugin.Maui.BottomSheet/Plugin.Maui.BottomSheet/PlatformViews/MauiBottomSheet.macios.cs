namespace Plugin.Maui.BottomSheet.PlatformViews;

using UIKit;

public class MauiBottomSheet : UIView
{
    private readonly IMauiContext _mauiContext;

    /// <inheritdoc />
    public MauiBottomSheet(IMauiContext mauiContext)
    {
        _mauiContext = mauiContext;
    }

    public void SetView(IBottomSheet virtualView)
    {
        throw new NotImplementedException();
    }

    public void Cleanup()
    {
        throw new NotImplementedException();
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