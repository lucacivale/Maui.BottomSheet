namespace Plugin.Maui.BottomSheet.Platform.MaciOS;

using System.ComponentModel;

/// <summary>
/// Manages the header section of bottom sheets on macOS and iOS platforms.
/// </summary>
internal sealed class BottomSheetHeader : IDisposable
{
    private readonly WeakEventManager _eventManager = new();

    private Plugin.Maui.BottomSheet.BottomSheetHeader _bottomSheetHeader;
    private BottomSheetHeaderStyle _style;

    private View? _virtualHeaderView;

    private View? _virtualTopLeftButton;
    private Label? _virtualTitleView;
    private View? _virtualTopRightButton;
    private Grid? _virtualHeaderGridView;

    /// <summary>
    /// Initializes a new instance of the <see cref="BottomSheetHeader"/> class.
    /// </summary>
    /// <param name="bottomSheetHeader">The header configuration.</param>
    /// <param name="style">The header style settings.</param>
    public BottomSheetHeader(Plugin.Maui.BottomSheet.BottomSheetHeader bottomSheetHeader, BottomSheetHeaderStyle style)
    {
        _bottomSheetHeader = bottomSheetHeader;
        _style = style;
    }

    /// <summary>
    /// Finalizes an instance of the <see cref="BottomSheetHeader"/> class.
    /// </summary>
    ~BottomSheetHeader()
    {
        Dispose(false);
    }

    /// <summary>
    /// Occurs when the header size changes.
    /// </summary>
    public event EventHandler SizeChanged
    {
        add => _eventManager.AddEventHandler(value);
        remove => _eventManager.RemoveEventHandler(value);
    }

    /// <summary>
    /// Occurs when the close button in the header is clicked.
    /// </summary>
    public event EventHandler CloseButtonClicked
    {
        add => _eventManager.AddEventHandler(value);
        remove => _eventManager.RemoveEventHandler(value);
    }

    /// <summary>
    /// Gets the header view, creating it if it doesn't exist.
    /// </summary>
    public View? View => _virtualHeaderView;

    /// <summary>
    /// Sets the header style and updates related UI bindings.
    /// </summary>
    /// <param name="style">The new header style to apply.</param>
    public void SetStyle(BottomSheetHeaderStyle style)
    {
        _style = style;

        if (_virtualTitleView is not null)
        {
            _virtualTitleView.BindingContext = _style;
        }

        if (_virtualTopLeftButton is CloseButton)
        {
            _virtualTopLeftButton.BindingContext = _style;
        }
        else if (_virtualTopRightButton is CloseButton)
        {
            _virtualTopRightButton.BindingContext = _style;
        }
    }

    /// <summary>
    /// Creates the virtual header view based on configuration, either custom or default layout.
    /// </summary>
    /// <returns>The created header view.</returns>
    public View CreateVirtualHeaderView()
    {
        if (_virtualHeaderView is not null)
        {
            _virtualHeaderView.SizeChanged -= VirtualHeaderViewOnSizeChanged;
        }

        if (_bottomSheetHeader.HasHeaderView())
        {
            _virtualHeaderView = _bottomSheetHeader.CreateContent();
        }
        else
        {
            _virtualHeaderGridView = CreateHeaderGrid();
            ConfigureHeader();

            _virtualHeaderView = _virtualHeaderGridView;
            _virtualHeaderView.BindingContext = _bottomSheetHeader.BindingContext;
            _virtualHeaderView.Parent = _bottomSheetHeader.Parent;
        }

        ArgumentNullException.ThrowIfNull(_virtualHeaderView);

        _virtualHeaderView.SizeChanged += VirtualHeaderViewOnSizeChanged;
        _bottomSheetHeader.PropertyChanged += BottomSheetHeaderOnPropertyChanged;

        return _virtualHeaderView;
    }

    /// <summary>
    /// Hides the header and cleans up associated resources.
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
    /// Sets the title text for the header.
    /// </summary>
    /// <param name="title">The title text to display.</param>
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

    /// <summary>
    /// Releases all resources used by the header.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Creates a grid layout for organizing header elements with three columns.
    /// </summary>
    /// <returns>A configured Grid for header layout.</returns>
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

    /// <summary>
    /// Removes a view reference and sets it to null.
    /// </summary>
    /// <param name="view">The view reference to remove.</param>
    private void Remove(ref View? view)
    {
        Remove(view);
        view = null;
    }

    /// <summary>
    /// Removes a grid reference and sets it to null.
    /// </summary>
    /// <param name="view">The grid reference to remove.</param>
    private void Remove(ref Grid? view)
    {
        Remove(view);
        view = null;
    }

    /// <summary>
    /// Removes a label reference and sets it to null.
    /// </summary>
    /// <param name="view">The label reference to remove.</param>
    private void Remove(ref Label? view)
    {
        Remove(view);

        view = null;
    }

    /// <summary>
    /// Removes a view from its parent layout and disconnects handlers.
    /// </summary>
    /// <param name="view">The view to remove.</param>
    private void Remove(View? view)
    {
        if (view?.Parent is Layout parent)
        {
            parent.Remove(view);
        }

        if (view is CloseButton closeButton)
        {
            closeButton.Clicked -= OnClosedButtonClicked;
        }

        view?.DisconnectHandlers();

        if (view is not null)
        {
            view.BindingContext = null;
        }
    }

    /// <summary>
    /// Creates a close button with proper styling and event handling.
    /// </summary>
    /// <param name="layoutOptions">The layout options for button positioning.</param>
    /// <returns>A configured CloseButton instance.</returns>
    private CloseButton CreateCloseButton(LayoutOptions layoutOptions)
    {
        var button = new CloseButton()
        {
            HorizontalOptions = layoutOptions,
            BindingContext = _style,
        };

        button.SetBinding(VisualElement.HeightRequestProperty, static (BottomSheetHeaderStyle style) => style.CloseButtonHeightRequest);
        button.SetBinding(VisualElement.WidthRequestProperty, static (BottomSheetHeaderStyle style) => style.CloseButtonWidthRequest);
        button.SetBinding(CloseButton.TintProperty, static (BottomSheetHeaderStyle style) => style.CloseButtonTintColor);

        button.Clicked += OnClosedButtonClicked;

        return button;
    }

    /// <summary>
    /// Handles close button click events and raises the CloseButtonClicked event.
    /// </summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="e">The event arguments.</param>
    private void OnClosedButtonClicked(object? sender, EventArgs e)
    {
        _eventManager.HandleEvent(
            this,
            EventArgs.Empty,
            nameof(CloseButtonClicked));
    }

    /// <summary>
    /// Configures the header layout by adding appropriate buttons and title.
    /// </summary>
    private void ConfigureHeader()
    {
        if (_virtualHeaderGridView is null)
        {
            return;
        }

        Remove(ref _virtualTopLeftButton);
        Remove(ref _virtualTopRightButton);
        Remove(ref _virtualTitleView);

        if (_bottomSheetHeader.HasTopLeftButton())
        {
            _virtualTopLeftButton = _bottomSheetHeader.TopLeftButton;
            _virtualHeaderGridView.Add(_virtualTopLeftButton);
        }

        if (_bottomSheetHeader.HasTopLeftCloseButton())
        {
            _virtualTopLeftButton = CreateCloseButton(LayoutOptions.Start);
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

        if (_bottomSheetHeader.HasTopRightCloseButton())
        {
            _virtualTopRightButton = CreateCloseButton(LayoutOptions.End);
            _virtualHeaderGridView.Add(_virtualTopRightButton, 2);
        }
    }

    /// <summary>
    /// Creates a title label with proper styling and data binding.
    /// </summary>
    /// <returns>A configured Label for the title.</returns>
    private Label CreateTitleView()
    {
        var title = new Label()
        {
            VerticalTextAlignment = TextAlignment.Center,
            HorizontalOptions = LayoutOptions.Fill,
            HorizontalTextAlignment = TextAlignment.Center,
            Text = _bottomSheetHeader.TitleText ?? string.Empty,
            BindingContext = _style,
        };

        title.SetBinding(Label.TextColorProperty, static (BottomSheetHeaderStyle style) => style.TitleTextColor);
        title.SetBinding(Label.FontAttributesProperty, static (BottomSheetHeaderStyle style) => style.TitleTextFontAttributes);
        title.SetBinding(Label.FontFamilyProperty, static (BottomSheetHeaderStyle style) => style.TitleTextFontFamily);
        title.SetBinding(Label.FontSizeProperty, static (BottomSheetHeaderStyle style) => style.TitleTextFontSize);
        title.SetBinding(Label.FontAutoScalingEnabledProperty, static (BottomSheetHeaderStyle style) => style.TitleTextFontAutoScalingEnabled);

        return title;
    }

    /// <summary>
    /// Handles header view size changes and raises the SizeChanged event.
    /// </summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="e">The event arguments.</param>
    private void VirtualHeaderViewOnSizeChanged(object? sender, EventArgs e)
    {
        _eventManager.HandleEvent(
            this,
            EventArgs.Empty,
            nameof(SizeChanged));
    }

    /// <summary>
    /// Handles property changes on the bottom sheet header and updates the UI accordingly.
    /// </summary>
    /// <param name="sender">The event sender.</param>
    /// <param name="e">The property changed event arguments.</param>
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
                    _virtualHeaderGridView ??= CreateHeaderGrid();
                    ConfigureHeader();
                }

                break;
            case nameof(Maui.BottomSheet.BottomSheetHeader.BindingContext):
                if (_virtualHeaderView is not null)
                {
                    _virtualHeaderView.BindingContext = _bottomSheetHeader.BindingContext;
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

        _bottomSheetHeader = null!;
        _style = null!;
    }
}