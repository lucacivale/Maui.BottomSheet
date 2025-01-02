#pragma warning disable SA1200
using Android.Content;
using Android.Widget;
using AGravityFlags = Android.Views.GravityFlags;
using ASpec = Android.Widget.GridLayout.Spec;
using AView = Android.Views.View;
using AViewGroup = Android.Views.ViewGroup;
#pragma warning restore SA1200

namespace Plugin.Maui.BottomSheet.Platform.Android;

using System.ComponentModel;
using System.Globalization;
using Microsoft.Maui.Platform;

// ReSharper disable RedundantNameQualifier
using TextAlignment = Microsoft.Maui.TextAlignment;
using View = Microsoft.Maui.Controls.View;

// ReSharper restore RedundantNameQualifier

/// <summary>
/// The header shown at the top of <see cref="MauiBottomSheet"/>.
/// </summary>
internal sealed class BottomSheetHeader : IDisposable
{
    private const int TitleViewHorizontalMargin = 40;

    private readonly WeakEventManager _eventManager = new();

    private readonly Context _context;
    private readonly IMauiContext _mauiContext;
    private readonly Plugin.Maui.BottomSheet.BottomSheetHeader _bottomSheetHeader;
    private readonly BottomSheetHeaderLayoutChangeListener _headerLayoutChangeListener;

    private GridLayout? _headerLayout;
    private AView? _topLeftView;
    private AView? _topRightView;
    private AView? _titleView;
    private AView? _headerView;

    private View? _virtualHeaderView;

    private AViewGroup.LayoutParams? _titleViewLayoutParams;
    private AViewGroup.LayoutParams? _topLeftViewLayoutParams;
    private AViewGroup.LayoutParams? _topRightViewLayoutParams;

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
        _bottomSheetHeader.PropertyChanged += BottomSheetHeaderOnPropertyChanged;
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
            else
            {
                if (_headerLayout is not null)
                {
                    ConfigureHeader(_headerLayout);
                }
            }

            return _headerView;
        }
    }

    /// <summary>
    /// Raise <see cref="LayoutChanged"/>.
    /// </summary>
    public void RaiseLayoutChangedEvent()
    {
        _eventManager.HandleEvent(this, EventArgs.Empty, nameof(LayoutChanged));
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
            _titleViewLayoutParams = CreateTitleViewLayoutParams();
            _headerLayout.AddView(_titleView, _titleViewLayoutParams);
        }
    }

    /// <summary>
    /// Removes the header from the BottomSheet.
    /// </summary>
    public void Remove()
    {
        _bottomSheetHeader.PropertyChanged -= BottomSheetHeaderOnPropertyChanged;

        RemoveFromParent(_headerLayout);
        RemoveView(ref _topLeftView, _bottomSheetHeader.TopLeftButton);
        RemoveView(ref _topRightView, _bottomSheetHeader.TopRightButton);
        RemoveView(ref _titleView, null);

        if (_bottomSheetHeader.HasHeaderView())
        {
            RemoveView(ref _headerView, _virtualHeaderView);
        }

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

    private static void RemoveView(ref GridLayout? view, View? mauiView)
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

    private static GridLayout.LayoutParams CreateHeaderViewLayoutParams(
        AGravityFlags gravity,
        ASpec? rowSpec,
        ASpec? columnSpec,
        int width)
    {
        var gridParams = new GridLayout.LayoutParams()
        {
            Width = width,
            RowSpec = rowSpec,
            ColumnSpec = columnSpec,
        };

        gridParams.SetGravity(gravity);

        return gridParams;
    }

    private GridLayout.LayoutParams CreateTitleViewLayoutParams()
    {
        var layoutParams = CreateHeaderViewLayoutParams(
            AGravityFlags.Center,
            GridLayout.InvokeSpec(0),
            GridLayout.InvokeSpec(1),
            Convert.ToInt32((_headerLayout?.Width * 0.50) - _context.ToPixels(TitleViewHorizontalMargin), CultureInfo.CurrentCulture));

        layoutParams.LeftMargin = Convert.ToInt32(_context.ToPixels(Convert.ToDouble(TitleViewHorizontalMargin) / 2));
        layoutParams.RightMargin = Convert.ToInt32(_context.ToPixels(Convert.ToDouble(TitleViewHorizontalMargin) / 2));

        if (!_bottomSheetHeader.HasTopLeftButton())
        {
            layoutParams.LeftMargin += HeaderButtonWidth();
        }

        return layoutParams;
    }

    private AView CreateHeader(Plugin.Maui.BottomSheet.BottomSheetHeader? bottomSheetHeader)
    {
        ArgumentNullException.ThrowIfNull(bottomSheetHeader);

        if (bottomSheetHeader.HasHeaderView()
            && bottomSheetHeader.HeaderDataTemplate!.CreateContent() is View headerView)
        {
            headerView.BindingContext = bottomSheetHeader.BindingContext;
            headerView.Parent = _bottomSheetHeader.Parent;

            _virtualHeaderView = headerView;

            var view = headerView.ToPlatform(_mauiContext);
            view.LayoutParameters = new AViewGroup.LayoutParams(AViewGroup.LayoutParams.MatchParent, AViewGroup.LayoutParams.WrapContent);

            return view;
        }

        _headerLayout = CreateHeaderLayout();

        ConfigureHeader(_headerLayout);

        return _headerLayout;
    }

    private TextView CreateTitleView(string? titleText)
    {
        Label title = new()
        {
            HorizontalTextAlignment = TextAlignment.Center,
            VerticalTextAlignment = TextAlignment.Center,
            FontAttributes = FontAttributes.Bold,
            FontSize = 20,
            Text = titleText ?? string.Empty,
        };

        var view = title.ToPlatform(_mauiContext);
        view.Id = AView.GenerateViewId();

        return (view as TextView)!;
    }

    private AView CreatePlatformView(View view)
    {
        view.BindingContext = _bottomSheetHeader.BindingContext;
        view.Parent = _bottomSheetHeader.Parent;
        return view.ToPlatform(_mauiContext);
    }

    private void ConfigureHeader(GridLayout headerLayout)
    {
        RemoveView(ref _topLeftView, _bottomSheetHeader.TopLeftButton);
        RemoveView(ref _titleView, null);
        RemoveView(ref _topRightView, _bottomSheetHeader.TopRightButton);

        if (_bottomSheetHeader.HasTopLeftButton())
        {
            _topLeftView = CreatePlatformView(_bottomSheetHeader.TopLeftButton!);

            _topLeftViewLayoutParams = CreateHeaderViewLayoutParams(
                AGravityFlags.Left,
                GridLayout.InvokeSpec(0),
                GridLayout.InvokeSpec(0),
                HeaderButtonWidth());

            headerLayout.AddView(_topLeftView, _topLeftViewLayoutParams);
        }

        if (_bottomSheetHeader.HasTitle())
        {
            _titleView = CreateTitleView(_bottomSheetHeader.TitleText);
            _titleViewLayoutParams = CreateTitleViewLayoutParams();
            headerLayout.AddView(_titleView, _titleViewLayoutParams);
        }

        if (_bottomSheetHeader.HasTopRightButton())
        {
            _topRightView = CreatePlatformView(_bottomSheetHeader.TopRightButton!);

            _topRightViewLayoutParams = CreateHeaderViewLayoutParams(
                AGravityFlags.Right,
                GridLayout.InvokeSpec(0),
                GridLayout.InvokeSpec(2),
                HeaderButtonWidth());

            headerLayout.AddView(_topRightView, _topRightViewLayoutParams);
        }
    }

    private GridLayout CreateHeaderLayout()
    {
        var grid = new GridLayout(_context)
        {
            RowCount = 1,
            ColumnCount = 3,
            LayoutParameters = new AViewGroup.MarginLayoutParams(AViewGroup.LayoutParams.MatchParent, AViewGroup.LayoutParams.WrapContent),
        };

        grid.AddOnLayoutChangeListener(_headerLayoutChangeListener);

        return grid;
    }

    private int HeaderButtonWidth()
    {
        return Convert.ToInt32(_headerLayout?.Width * 0.50, CultureInfo.CurrentCulture) / 2;
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
                    ConfigureHeader(_headerLayout);
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

        RemoveView(ref _headerLayout, null);
        _headerView = null;

        _headerLayoutChangeListener.Dispose();

        _titleViewLayoutParams?.Dispose();
        _topLeftViewLayoutParams?.Dispose();
        _topRightViewLayoutParams?.Dispose();

        _titleViewLayoutParams = null;
        _topLeftViewLayoutParams = null;
        _topRightViewLayoutParams = null;
    }
}