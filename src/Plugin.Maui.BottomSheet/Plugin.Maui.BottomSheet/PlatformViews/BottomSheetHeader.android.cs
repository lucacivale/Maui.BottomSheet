namespace Plugin.Maui.BottomSheet.PlatformViews;

using Android.Content;
using Android.Views;
using Android.Widget;
using Microsoft.Maui.Platform;

public class BottomSheetHeader
{
    private readonly IMauiContext _mauiContext;
    private readonly MauiBottomSheet _bottomSheet;
    private readonly BottomSheetLayoutChangeListener _layoutChangeListener;

    public BottomSheetHeader(IMauiContext mauiContext, MauiBottomSheet bottomSheet)
    {
        _mauiContext = mauiContext;
        _bottomSheet = bottomSheet;
        _layoutChangeListener = new BottomSheetLayoutChangeListener(bottomSheet);
    }

    public View CreateHeader(Plugin.Maui.BottomSheet.BottomSheetHeader _bottomSheetHeader)
    {
        if (_bottomSheetHeader.HeaderView is not null)
        {
            return _bottomSheetHeader.HeaderView.ToPlatform(_mauiContext);
        }
        
        var headerLayout = new LinearLayout(_bottomSheet.Context)
        {
            LayoutParameters = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent),
            Orientation = Orientation.Horizontal,
        };

        if (_bottomSheetHeader.HeaderView is not null)
        {
            headerLayout.AddView(_bottomSheetHeader.HeaderView.ToPlatform(_mauiContext));
        }

        headerLayout.AddOnLayoutChangeListener(_layoutChangeListener);

        return headerLayout;
    }
}