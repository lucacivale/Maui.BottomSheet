namespace Plugin.Maui.BottomSheet.Platform.MaciOS;

using System.ComponentModel;

/// <summary>
/// Bottom sheet header.
/// </summary>
internal sealed class BottomSheetHeader : IDisposable
{
    private readonly WeakEventManager _eventManager = new();
    private readonly Plugin.Maui.BottomSheet.BottomSheetHeader _bottomSheetHeader;

    private View? _virtualHeaderView;

    private View? _virtualTopLeftButton;
    private Label? _virtualTitleView;
    private View? _virtualTopRightButton;
    private Grid? _virtualHeaderGridView;

    /// <summary>
    /// Initializes a new instance of the <see cref="BottomSheetHeader"/> class.
    /// </summary>
    /// <param name="bottomSheetHeader">Header.</param>
    public BottomSheetHeader(Plugin.Maui.BottomSheet.BottomSheetHeader bottomSheetHeader)
    {
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
    /// Header size changed
    /// </summary>
    public event EventHandler SizeChanged
    {
        add => _eventManager.AddEventHandler(value);
        remove => _eventManager.RemoveEventHandler(value);
    }

    /// <summary>
    /// Gets header view.
    /// </summary>
    public View View
    {
        get => _virtualHeaderView ??= CreateVirtualHeaderView();
    }

    /// <summary>
    /// Hide header.
    /// </summary>
    public void Hide()
    {
        _bottomSheetHeader.PropertyChanged -= BottomSheetHeaderOnPropertyChanged;

        if (_virtualHeaderView is not null)
        {
            _virtualHeaderView.SizeChanged -= VirtualHeaderViewOnSizeChanged;
        }

        Remove(ref _virtualHeaderView);
        Remove(ref _virtualTitleView);
        Remove(ref _virtualHeaderGridView);
        Remove(ref _virtualTopLeftButton);
        Remove(ref _virtualTopRightButton);
    }

    /// <summary>
    /// Sets the header title.
    /// </summary>
    /// <param name="title">Title text.</param>
    public void SetTitleText(string title)
    {
        if (_virtualTitleView?.Text == title
            || _bottomSheetHeader.HasHeaderView())
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(title))
        {
            Remove(ref _virtualTitleView);
            return;
        }

        if (_virtualTitleView is not null)
        {
            _virtualTitleView.Text = title;
        }
        else
        {
            _virtualTitleView = CreateTitleView();

            _virtualHeaderGridView ??= CreateHeaderGrid();
            _virtualHeaderGridView.Add(_virtualTitleView, 1);
        }
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private static Grid CreateHeaderGrid()
    {
        return new Grid()
        {
            ColumnSpacing = 10,
            RowDefinitions = [new RowDefinition(GridLength.Star)],
            ColumnDefinitions =
            [
                new ColumnDefinition(GridLength.Star),
                new ColumnDefinition(new GridLength(2, GridUnitType.Star)),
                new ColumnDefinition(GridLength.Star),
            ],
        };
    }

    private static void Remove(ref View? view)
    {
        Remove(view);
        view = null;
    }

    private static void Remove(ref Grid? view)
    {
        Remove(view);
        view = null;
    }

    private static void Remove(ref Label? view)
    {
        Remove(view);
        view = null;
    }

    private static void Remove(View? view)
    {
        if (view?.Parent is Layout parent)
        {
            parent.Remove(view);
        }

#if NET9_0_OR_GREATER
        view?.DisconnectHandlers();
#else
        view?.Handler?.DisconnectHandler();
#endif
    }

    private View CreateVirtualHeaderView()
    {
        if (_virtualHeaderView is not null)
        {
            _virtualHeaderView.SizeChanged -= VirtualHeaderViewOnSizeChanged;
        }

        if (_bottomSheetHeader.HasHeaderView()
            && _bottomSheetHeader.HeaderDataTemplate is not null)
        {
            _virtualHeaderView = _bottomSheetHeader.HeaderDataTemplate.CreateContent() as View;
        }
        else
        {
            _virtualHeaderGridView = CreateHeaderGrid();
            ConfigureHeader();

            _virtualHeaderView = _virtualHeaderGridView;
        }

        ArgumentNullException.ThrowIfNull(_virtualHeaderView);

        _virtualHeaderView.BindingContext = _bottomSheetHeader.BindingContext;
        _virtualHeaderView.Parent = _bottomSheetHeader.Parent;
        _virtualHeaderView.SizeChanged += VirtualHeaderViewOnSizeChanged;
        _bottomSheetHeader.PropertyChanged += BottomSheetHeaderOnPropertyChanged;

        return _virtualHeaderView;
    }

    private void ConfigureHeader()
    {
        if (_virtualHeaderGridView is null)
        {
            return;
        }

        Remove(_virtualTopLeftButton);
        Remove(_virtualTopRightButton);
        Remove(_virtualTitleView);
        if (_bottomSheetHeader.HasTopLeftButton())
        {
            _virtualTopLeftButton = _bottomSheetHeader.TopLeftButton;
            _virtualHeaderGridView.Add(_virtualTopLeftButton);
        }

        if (_bottomSheetHeader.HasTitle())
        {
            _virtualTitleView = CreateTitleView();
            _virtualHeaderGridView.Add(_virtualTitleView, 1);
        }

        if (_bottomSheetHeader.HasTopRightButton())
        {
            _virtualTopRightButton = _bottomSheetHeader.TopRightButton;
            _virtualHeaderGridView.Add(_virtualTopRightButton, 2);
        }
    }

    private Label CreateTitleView()
    {
        return new Label()
        {
            VerticalTextAlignment = TextAlignment.Center,
            HorizontalOptions = LayoutOptions.Fill,
            HorizontalTextAlignment = TextAlignment.Center,
            Text = _bottomSheetHeader.TitleText ?? string.Empty,
        };
    }

    private void VirtualHeaderViewOnSizeChanged(object? sender, EventArgs e)
    {
        _eventManager.HandleEvent(
            this,
            EventArgs.Empty,
            nameof(SizeChanged));
    }

    private void Dispose(bool isDisposing)
    {
        if (isDisposing)
        {
            return;
        }

        Hide();
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
                    _virtualHeaderGridView ??= CreateHeaderGrid();
                    ConfigureHeader();
                }

                break;
        }
    }
}