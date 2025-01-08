using Android.Content;
using Android.Widget;
using ATextAlignment = Android.Views.TextAlignment;
using AView = Android.Views.View;
using AViewGroup = Android.Views.ViewGroup;

namespace Plugin.Maui.BottomSheet.Platform.Android;

using System.ComponentModel;
using Microsoft.Maui.Platform;

// ReSharper disable RedundantNameQualifier
using View = Microsoft.Maui.Controls.View;

// ReSharper restore RedundantNameQualifier

/// <summary>
/// The header shown at the top of <see cref="MauiBottomSheet"/>.
/// </summary>
internal sealed class BottomSheetHeader : IDisposable
{
    private readonly WeakEventManager _eventManager = new();

    private readonly Context _context;
    private readonly IMauiContext _mauiContext;
    private readonly Plugin.Maui.BottomSheet.BottomSheetHeader _bottomSheetHeader;
    private readonly BottomSheetHeaderLayoutChangeListener _headerLayoutChangeListener;

    private RelativeLayout? _headerLayout;
    private AView? _topLeftView;
    private AView? _topRightView;
    private AView? _titleView;
    private AView? _headerView;

    private View? _virtualHeaderView;

    private string _titleText = string.Empty;

    /// <summary>
    /// Initializes a new instance of the <see cref="BottomSheetHeader"/> class.
    /// </summary>
    /// <param name="context"><see cref="Context"/>.</param>
    /// <param name="mauiContext"><see cref="IMauiContext"/>.</param>
    /// <param name="bottomSheetHeader"><see cref="Maui.BottomSheet.BottomSheetHeader"/>.</param>
    public BottomSheetHeader(
        Context context,
        IMauiContext mauiContext,
        Plugin.Maui.BottomSheet.BottomSheetHeader bottomSheetHeader)
    {
        _headerLayoutChangeListener = new BottomSheetHeaderLayoutChangeListener(this);

        _context = context;
        _mauiContext = mauiContext;

        _bottomSheetHeader = bottomSheetHeader;
    }

    /// <summary>
    /// Finalizes an instance of the <see cref="BottomSheetHeader"/> class.
    /// </summary>
    ~BottomSheetHeader()
    {
        Dispose(false);
    }

    /// <summary>
    /// Header layout changed
    /// </summary>
    public event EventHandler LayoutChanged
    {
        add => _eventManager.AddEventHandler(value);
        remove => _eventManager.RemoveEventHandler(value);
    }

    /// <summary>
    /// Gets header view.
    /// </summary>
    public AView HeaderView
    {
        get
        {
            if (_headerView is null
                || _bottomSheetHeader.HasHeaderView())
            {
                _headerView = CreateHeader(_bottomSheetHeader);
            }

            return _headerView;
        }
    }

    /// <summary>
    /// Raise <see cref="LayoutChanged"/>.
    /// </summary>
    public void RaiseLayoutChangedEvent()
    {
        if (_titleView?.LayoutParameters is RelativeLayout.LayoutParams relativeLayoutParams
            && relativeLayoutParams.Width != Convert.ToInt32(_headerLayout?.Width * 0.50, System.Globalization.CultureInfo.CurrentCulture))
        {
            relativeLayoutParams.Width = Convert.ToInt32(_headerLayout?.Width * 0.50, System.Globalization.CultureInfo.CurrentCulture);
            _titleView.RequestLayout();
        }

        _eventManager.HandleEvent(this, EventArgs.Empty, nameof(LayoutChanged));
    }

    /// <summary>
    /// Create header view.
    /// </summary>
    /// <returns>Created header.</returns>
    public AView CreateHeader()
    {
        _headerView = CreateHeader(_bottomSheetHeader);
        ConfigureHeader();

        return _headerView;
    }

    /// <summary>
    /// Sets the header title.
    /// </summary>
    /// <param name="title">Title text.</param>
    public void SetTitleText(string title)
    {
        if (_titleText == title
            || _bottomSheetHeader.HasHeaderView())
        {
            return;
        }

        _titleText = title;

        if (string.IsNullOrWhiteSpace(_titleText))
        {
            RemoveView(ref _titleView, null);
            return;
        }

        if (_titleView is TextView titleView)
        {
            titleView.Text = _titleText;
        }
        else if (_titleView is null
            && _headerLayout is not null)
        {
            _titleView = CreateTitleView(_titleText);
            _titleView.LayoutParameters = CreateTitleViewLayoutParams();
            _headerLayout.AddView(_titleView);
        }
    }

    /// <summary>
    /// Removes the header from the BottomSheet.
    /// </summary>
    public void Remove()
    {
        _bottomSheetHeader.PropertyChanged -= BottomSheetHeaderOnPropertyChanged;

        _headerView?.RemoveOnLayoutChangeListener(_headerLayoutChangeListener);
        if (_headerLayout is not null)
        {
            RemoveView(ref _headerLayout, null);
            _headerView = null;
        }
        else
        {
            RemoveView(ref _headerView, _virtualHeaderView);
        }

        RemoveView(ref _topLeftView, _bottomSheetHeader.TopLeftButton);
        RemoveView(ref _topRightView, _bottomSheetHeader.TopRightButton);
        RemoveView(ref _titleView, null);

        RaiseLayoutChangedEvent();
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private static void RemoveView(ref AView? view, View? mauiView)
    {
        RemoveView(view, mauiView);
        view = null;
    }

    private static void RemoveView(ref RelativeLayout? view, View? mauiView)
    {
        RemoveView(view, mauiView);
        view = null;
    }

    private static void RemoveView(AView? view, View? mauiView)
    {
        RemoveFromParent(view);
        view?.LayoutParameters?.Dispose();

#if NET9_0_OR_GREATER
        mauiView?.DisconnectHandlers();
#else
        mauiView?.Handler?.DisconnectHandler();
#endif
        view?.Dispose();
    }

    private static void RemoveFromParent(AView? view)
    {
        if (view?.Parent is not null)
        {
            view.RemoveFromParent();
        }
    }

    private static RelativeLayout.LayoutParams CreateHeaderViewLayoutParams(LayoutRules layoutRule)
    {
        var layoutParams = new RelativeLayout.LayoutParams(AViewGroup.LayoutParams.WrapContent, AViewGroup.LayoutParams.WrapContent);
        layoutParams.AddRule(layoutRule);

        return layoutParams;
    }

    private static RelativeLayout.LayoutParams CreateTitleViewLayoutParams()
    {
        var layoutParams = new RelativeLayout.LayoutParams(AViewGroup.LayoutParams.WrapContent, AViewGroup.LayoutParams.WrapContent)
        {
            LeftMargin = 20,
            RightMargin = 20,
        };

        layoutParams.AddRule(LayoutRules.CenterInParent);

        return layoutParams;
    }

    private AView CreateHeader(Plugin.Maui.BottomSheet.BottomSheetHeader? bottomSheetHeader)
    {
        ArgumentNullException.ThrowIfNull(bottomSheetHeader);

        _bottomSheetHeader.PropertyChanged += BottomSheetHeaderOnPropertyChanged;

        if (bottomSheetHeader.HasHeaderView()
            && bottomSheetHeader.HeaderDataTemplate!.CreateContent() is View headerView)
        {
            headerView.BindingContext = bottomSheetHeader.BindingContext;
            headerView.Parent = _bottomSheetHeader.Parent;

            _virtualHeaderView = headerView;

            var view = headerView.ToPlatform(_mauiContext);
            view.LayoutParameters = new AViewGroup.LayoutParams(AViewGroup.LayoutParams.MatchParent, AViewGroup.LayoutParams.WrapContent);
            view.AddOnLayoutChangeListener(_headerLayoutChangeListener);
            return view;
        }

        _headerLayout = CreateHeaderLayout();

        ConfigureHeader();

        return _headerLayout;
    }

    private TextView CreateTitleView(string? titleText)
    {
        Label title = new()
        {
            FontAttributes = FontAttributes.Bold,
            FontSize = 20,
            Text = titleText ?? string.Empty,
        };

        var view = title.ToPlatform(_mauiContext);
        view.Id = AView.GenerateViewId();

        if (view is TextView textView)
        {
            textView.TextAlignment = ATextAlignment.Center;
        }

        return (view as TextView)!;
    }

    private AView CreatePlatformView(View view)
    {
        view.BindingContext = _bottomSheetHeader.BindingContext;
        view.Parent = _bottomSheetHeader.Parent;

        var platformView = view.ToPlatform(_mauiContext);
        platformView.Id = AView.GenerateViewId();

        return platformView;
    }

    private void ConfigureHeader()
    {
        RemoveView(ref _topLeftView, _bottomSheetHeader.TopLeftButton);
        RemoveView(ref _titleView, null);
        RemoveView(ref _topRightView, _bottomSheetHeader.TopRightButton);

        if (_bottomSheetHeader.HasTopLeftButton())
        {
            _topLeftView = CreatePlatformView(_bottomSheetHeader.TopLeftButton!);
            _topLeftView.LayoutParameters = CreateHeaderViewLayoutParams(LayoutRules.AlignParentLeft);

            _headerLayout?.AddView(_topLeftView);
        }

        if (_bottomSheetHeader.HasTitle())
        {
            _titleView = CreateTitleView(_bottomSheetHeader.TitleText);
            _titleView.LayoutParameters = CreateTitleViewLayoutParams();

            _headerLayout?.AddView(_titleView);
        }

        if (_bottomSheetHeader.HasTopRightButton())
        {
            _topRightView = CreatePlatformView(_bottomSheetHeader.TopRightButton!);
            _topRightView.LayoutParameters = CreateHeaderViewLayoutParams(LayoutRules.AlignParentRight);

            _headerLayout?.AddView(_topRightView);
        }
    }

    private RelativeLayout CreateHeaderLayout()
    {
        var linearLayout = new RelativeLayout(_context)
        {
            LayoutParameters = new LinearLayout.LayoutParams(AViewGroup.LayoutParams.MatchParent, AViewGroup.LayoutParams.WrapContent),
        };

        linearLayout.AddOnLayoutChangeListener(_headerLayoutChangeListener);

        return linearLayout;
    }

    private void BottomSheetHeaderOnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        switch (e.PropertyName)
        {
            case nameof(Maui.BottomSheet.BottomSheetHeader.TitleText):
                if (!_bottomSheetHeader.HasHeaderView())
                {
                    SetTitleText(_bottomSheetHeader.TitleText ?? string.Empty);
                }

                break;
            case nameof(Maui.BottomSheet.BottomSheetHeader.HeaderAppearance):
                if (!_bottomSheetHeader.HasHeaderView())
                {
                    _headerLayout ??= CreateHeaderLayout();
                    ConfigureHeader();
                }

                break;
        }
    }

    private void Dispose(bool disposing)
    {
        if (!disposing)
        {
            return;
        }

        Remove();

        _headerLayoutChangeListener.Dispose();
    }
}