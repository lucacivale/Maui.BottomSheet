#pragma warning disable SA1200
using Android.Content;
using Android.Widget;
using AColor = Android.Graphics.Color;
using AColorStateList = Android.Content.Res.ColorStateList;
using ATextAlignment = Android.Views.TextAlignment;
using AView = Android.Views.View;
using AViewGroup = Android.Views.ViewGroup;
#pragma warning restore SA1200

namespace Plugin.Maui.BottomSheet.Platform.Android;

using System.ComponentModel;
using _Microsoft.Android.Resource.Designer;
using Google.Android.Material.Button;
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

    private BottomSheetHeaderStyle _headerStyle;

    private RelativeLayout? _headerLayout;
    private AView? _topLeftView;
    private AView? _topRightView;

    private AView? _titleView;
    private View? _virtualTitleView;

    private AView? _headerView;

    private View? _virtualHeaderView;

    private string _titleText = string.Empty;

    /// <summary>
    /// Initializes a new instance of the <see cref="BottomSheetHeader"/> class.
    /// </summary>
    /// <param name="context"><see cref="Context"/>.</param>
    /// <param name="mauiContext"><see cref="IMauiContext"/>.</param>
    /// <param name="bottomSheetHeader"><see cref="Maui.BottomSheet.BottomSheetHeader"/>.</param>
    /// <param name="headerStyle">Style.</param>
    public BottomSheetHeader(
        Context context,
        IMauiContext mauiContext,
        Plugin.Maui.BottomSheet.BottomSheetHeader bottomSheetHeader,
        BottomSheetHeaderStyle headerStyle)
    {
        _headerLayoutChangeListener = new BottomSheetHeaderLayoutChangeListener(this);

        _context = context;
        _mauiContext = mauiContext;

        _bottomSheetHeader = bottomSheetHeader;
        _headerStyle = headerStyle;
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
    /// Close button clicked
    /// </summary>
    public event EventHandler CloseButtonClicked
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
    /// Set style.
    /// </summary>
    /// <param name="style">Style.</param>
    public void SetStyle(BottomSheetHeaderStyle style)
    {
        _headerStyle.PropertyChanged -= HeaderStyleOnPropertyChanged;
        _headerStyle = style;
        _headerStyle.PropertyChanged += HeaderStyleOnPropertyChanged;

        if (_virtualTitleView is not null)
        {
            _virtualTitleView.BindingContext = style;
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
            _titleView.Post(_titleView.RequestLayout);
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
            RemoveView(ref _titleView, _virtualHeaderView);
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
        RemoveView(ref _titleView, _virtualTitleView);

        RaiseLayoutChangedEvent();
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
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

    private void RemoveView(ref AView? view, View? mauiView)
    {
        RemoveView(view, mauiView);
        view = null;
    }

    private void RemoveView(ref RelativeLayout? view, View? mauiView)
    {
        RemoveView(view, mauiView);
        view = null;
    }

    private void RemoveView(AView? view, View? mauiView)
    {
        if (view is MaterialButton)
        {
            view.Click -= RaiseCloseButtonClicked;
        }

        RemoveFromParent(view);
        view?.LayoutParameters?.Dispose();

        mauiView?.DisconnectHandlers();
        view?.Dispose();
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
            Text = titleText ?? string.Empty,
            BindingContext = _headerStyle,
        };

        title.SetBinding(Label.TextColorProperty, static (BottomSheetHeaderStyle style) => style.TitleTextColor);
        title.SetBinding(Label.FontAttributesProperty, static (BottomSheetHeaderStyle style) => style.TitleTextFontAttributes);
        title.SetBinding(Label.FontFamilyProperty, static (BottomSheetHeaderStyle style) => style.TitleTextFontFamily);
        title.SetBinding(Label.FontSizeProperty, static (BottomSheetHeaderStyle style) => style.TitleTextFontSize);
        title.SetBinding(Label.FontAutoScalingEnabledProperty, static (BottomSheetHeaderStyle style) => style.TitleTextFontAutoScalingEnabled);

        _virtualTitleView = title;

        var view = _virtualTitleView.ToPlatform(_mauiContext);
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
        RemoveView(ref _titleView, _virtualTitleView);
        RemoveView(ref _topRightView, _bottomSheetHeader.TopRightButton);

        if (_bottomSheetHeader.HasTopLeftButton())
        {
            _topLeftView = CreatePlatformView(_bottomSheetHeader.TopLeftButton!);
            _topLeftView.LayoutParameters = CreateHeaderViewLayoutParams(LayoutRules.AlignParentLeft);

            _headerLayout?.AddView(_topLeftView);
        }

        if (_bottomSheetHeader.HasTopLeftCloseButton())
        {
            _topLeftView = CreateCloseButton();
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

        if (_bottomSheetHeader.HasTopRightCloseButton())
        {
            _topRightView = CreateCloseButton();
            _topRightView.LayoutParameters = CreateHeaderViewLayoutParams(LayoutRules.AlignParentRight);

            _headerLayout?.AddView(_topRightView);
        }
    }

    private MaterialButton CreateCloseButton()
    {
        var closeButton = new MaterialButton(_context);
        closeButton.SetIconResource(Resource.Drawable.mtrl_ic_cancel);
        closeButton.IconGravity = MaterialButton.IconGravityTextStart;
        closeButton.IconPadding = 0;
        closeButton.IconSize = Convert.ToInt32(_context.ToPixels((_headerStyle.CloseButtonHeightRequest + _headerStyle.CloseButtonWidthRequest) / 2));
        closeButton.IconTint = AColorStateList.ValueOf(_headerStyle.CloseButtonTintColor.ToPlatform());
        closeButton.BackgroundTintList = AColorStateList.ValueOf(AColor.Transparent);
        closeButton.Click += RaiseCloseButtonClicked;

        closeButton.SetHeight(Convert.ToInt32(_context.ToPixels(_headerStyle.CloseButtonHeightRequest)));
        closeButton.SetWidth(Convert.ToInt32(_context.ToPixels(_headerStyle.CloseButtonWidthRequest)));

        return closeButton;
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

    private void RaiseCloseButtonClicked(object? sender, EventArgs e)
    {
        _eventManager.HandleEvent(this, EventArgs.Empty, nameof(CloseButtonClicked));
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
            case nameof(Maui.BottomSheet.BottomSheetHeader.ShowCloseButton):
            case nameof(Maui.BottomSheet.BottomSheetHeader.CloseButtonPosition):
                if (e.PropertyName == nameof(CloseButtonPosition)
                    && _bottomSheetHeader.ShowCloseButton == false)
                {
                    return;
                }

                if (!_bottomSheetHeader.HasHeaderView())
                {
                    _headerLayout ??= CreateHeaderLayout();
                    ConfigureHeader();
                }

                break;
        }
    }

    private void HeaderStyleOnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        BottomSheetHeaderStyle style = (BottomSheetHeaderStyle)sender!;

        if (_bottomSheetHeader.ShowCloseButton
            && (_bottomSheetHeader.HeaderAppearance == BottomSheetHeaderButtonAppearanceMode.LeftAndRightButton
                || _bottomSheetHeader.HeaderAppearance == BottomSheetHeaderButtonAppearanceMode.LeftButton
                || _bottomSheetHeader.HeaderAppearance == BottomSheetHeaderButtonAppearanceMode.RightButton)
            && (e.PropertyName == nameof(BottomSheetHeaderStyle.CloseButtonHeightRequest)
                || e.PropertyName == nameof(BottomSheetHeaderStyle.CloseButtonWidthRequestProperty)
                || e.PropertyName == nameof(BottomSheetHeaderStyle.CloseButtonTintColor)))
        {
            MaterialButton button;

            if (_bottomSheetHeader.CloseButtonPosition == CloseButtonPosition.TopLeft)
            {
                button = (MaterialButton)_topLeftView!;
            }
            else
            {
                button = (MaterialButton)_topRightView!;
            }

            button.SetHeight(Convert.ToInt32(_context.ToPixels(_headerStyle.CloseButtonHeightRequest)));
            button.SetWidth(Convert.ToInt32(_context.ToPixels(_headerStyle.CloseButtonWidthRequest)));
            button.IconSize = Convert.ToInt32(_context.ToPixels((_headerStyle.CloseButtonHeightRequest + _headerStyle.CloseButtonWidthRequest) / 2));

            button.IconTint = AColorStateList.ValueOf(style.CloseButtonTintColor.ToPlatform());
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